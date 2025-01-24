using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTManager : MonoBehaviour
{
    private const string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string apiKey;

    void Start()
    {
        // .env 파일 로드
        EnvLoader.LoadEnvFile(); // .env 파일 읽기
        apiKey = EnvLoader.GetEnvVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API 키가 설정되지 않았습니다! .env 파일을 확인하세요.");
        }
        else
        {
            Debug.Log("API 키가 성공적으로 로드되었습니다.");
        }
    }

    [System.Serializable]
    public class ChatGPTRequest
    {
        public string model = "gpt-3.5-turbo";
        public Message[] messages;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class ChatGPTResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    public IEnumerator GetChatGPTResponse(string prompt, System.Action<string> onResponse)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            onResponse?.Invoke("API 키가 설정되지 않았습니다.");
            yield break;
        }

        Debug.Log("ChatGPT API 호출 준비 중...");

        // 요청 데이터 생성
        ChatGPTRequest requestData = new ChatGPTRequest
        {
            messages = new Message[]
            {
                new Message { role = "user", content = prompt }
            }
        };

        // JSON 직렬화
        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log($"요청 데이터 생성 완료: {jsonData}");

        // HTTP 요청 생성
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            Debug.Log("ChatGPT API 호출 중...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("ChatGPT API 호출 성공!");
                Debug.Log($"응답 데이터: {request.downloadHandler.text}");

                ChatGPTResponse responseData = JsonUtility.FromJson<ChatGPTResponse>(request.downloadHandler.text);
                if (responseData.choices != null && responseData.choices.Length > 0)
                {
                    string reply = responseData.choices[0].message.content;
                    Debug.Log($"ChatGPT 응답: {reply}");
                    onResponse?.Invoke(reply.Trim());
                }
                else
                {
                    Debug.LogWarning("ChatGPT로부터 유효한 응답을 받지 못했습니다.");
                    onResponse?.Invoke("ChatGPT 응답이 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"ChatGPT API 호출 실패: {request.error}");
                Debug.LogError($"응답 내용: {request.downloadHandler.text}");
                onResponse?.Invoke($"Error: {request.downloadHandler.text}");
            }
        }
    }
}