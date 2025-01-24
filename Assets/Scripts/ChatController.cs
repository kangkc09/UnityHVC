using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    public InputField userInputField; // ���� �ؽ�Ʈ �Է�
    public Text chatOutputText;      // ChatGPT ���� ���
    private ChatGPTManager chatGPTManager;

    void Start()
    {
        chatGPTManager = GetComponent<ChatGPTManager>();
        if (chatGPTManager == null)
        {
            Debug.LogError("ChatGPTManager�� ������� �ʾҽ��ϴ�!");
        }

        if (userInputField == null || chatOutputText == null)
        {
            Debug.LogError("UI ��Ұ� ������� �ʾҽ��ϴ�!");
        }
        userInputField.ActivateInputField();
    }

    public void OnSendButtonClicked()
    {
        if (userInputField == null || chatOutputText == null)
        {
            Debug.LogError("UI ��Ұ� ������� �ʾҽ��ϴ�!");
            return;
        }

        string userInput = userInputField.text; // ���� �Է� ��������
        if (string.IsNullOrEmpty(userInput))
        {
            Debug.LogWarning("�� �Է��� ó������ �ʽ��ϴ�.");
            return;
        }

        chatOutputText.text = "User: " + userInput; // ���� �Է� ���
        Debug.Log($"����� �Է�: {userInput}");

        // ChatGPT ȣ��
        StartCoroutine(chatGPTManager.GetChatGPTResponse(userInput, (response) =>
        {
            chatOutputText.text += "\nChatGPT: " + response; // ���� ���
        }));

        userInputField.text = ""; // �Է� �ʵ� �ʱ�ȭ
    }
}