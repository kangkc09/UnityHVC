using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    public InputField userInputField; // 유저 텍스트 입력
    public Text chatOutputText;      // ChatGPT 응답 출력
    private ChatGPTManager chatGPTManager;

    void Start()
    {
        chatGPTManager = GetComponent<ChatGPTManager>();
        if (chatGPTManager == null)
        {
            Debug.LogError("ChatGPTManager가 연결되지 않았습니다!");
        }

        if (userInputField == null || chatOutputText == null)
        {
            Debug.LogError("UI 요소가 연결되지 않았습니다!");
        }
        userInputField.ActivateInputField();
    }

    public void OnSendButtonClicked()
    {
        if (userInputField == null || chatOutputText == null)
        {
            Debug.LogError("UI 요소가 연결되지 않았습니다!");
            return;
        }

        string userInput = userInputField.text; // 유저 입력 가져오기
        if (string.IsNullOrEmpty(userInput))
        {
            Debug.LogWarning("빈 입력은 처리하지 않습니다.");
            return;
        }

        chatOutputText.text = "User: " + userInput; // 유저 입력 출력
        Debug.Log($"사용자 입력: {userInput}");

        // ChatGPT 호출
        StartCoroutine(chatGPTManager.GetChatGPTResponse(userInput, (response) =>
        {
            chatOutputText.text += "\nChatGPT: " + response; // 응답 출력
        }));

        userInputField.text = ""; // 입력 필드 초기화
    }
}