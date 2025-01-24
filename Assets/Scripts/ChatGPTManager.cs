using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTManager : MonoBehaviour
{
    private const string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string apiKey;

    void Start()
    {
        // .env ���� �ε�
        EnvLoader.LoadEnvFile(); // .env ���� �б�
        apiKey = EnvLoader.GetEnvVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Ű�� �������� �ʾҽ��ϴ�! .env ������ Ȯ���ϼ���.");
        }
        else
        {
            Debug.Log("API Ű�� ���������� �ε�Ǿ����ϴ�.");
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
            onResponse?.Invoke("API Ű�� �������� �ʾҽ��ϴ�.");
            yield break;
        }

        Debug.Log("ChatGPT API ȣ�� �غ� ��...");

        // ��û ������ ����
        ChatGPTRequest requestData = new ChatGPTRequest
        {
            messages = new Message[]
            {
                new Message { role = "user", content = prompt }
            }
        };

        // JSON ����ȭ
        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log($"��û ������ ���� �Ϸ�: {jsonData}");

        // HTTP ��û ����
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            Debug.Log("ChatGPT API ȣ�� ��...");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("ChatGPT API ȣ�� ����!");
                Debug.Log($"���� ������: {request.downloadHandler.text}");

                ChatGPTResponse responseData = JsonUtility.FromJson<ChatGPTResponse>(request.downloadHandler.text);
                if (responseData.choices != null && responseData.choices.Length > 0)
                {
                    string reply = responseData.choices[0].message.content;
                    Debug.Log($"ChatGPT ����: {reply}");
                    onResponse?.Invoke(reply.Trim());
                }
                else
                {
                    Debug.LogWarning("ChatGPT�κ��� ��ȿ�� ������ ���� ���߽��ϴ�.");
                    onResponse?.Invoke("ChatGPT ������ �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError($"ChatGPT API ȣ�� ����: {request.error}");
                Debug.LogError($"���� ����: {request.downloadHandler.text}");
                onResponse?.Invoke($"Error: {request.downloadHandler.text}");
            }
        }
    }
}