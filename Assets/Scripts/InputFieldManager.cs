using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    public Canvas mainCanvas;         // UI Canvas
    public InputField inputField;     // InputField
    public Text outputText;           // Text to display the result
    private ChatController chatController;
    private bool isCanvasActive = false; // Canvas 활성화 상태

    void Start()
    {
        // 초기 상태로 Canvas를 비활성화
        mainCanvas.gameObject.SetActive(false);
        chatController = GetComponent<ChatController>();

    }

    void Update()
    {
        // Tap 키를 눌러 Canvas 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isCanvasActive = !isCanvasActive;

            // Canvas가 활성화되면 InputField에 포커스
            if (isCanvasActive)
            {
                mainCanvas.gameObject.SetActive(isCanvasActive);
                inputField.text = ""; // 입력 필드 초기화
                inputField.ActivateInputField();
            }
            else
            {
                inputField.text = "";
                mainCanvas.gameObject.SetActive(false);

            }
        }
        if (isCanvasActive)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                chatController.OnSendButtonClicked();
                inputField.ActivateInputField();
            }   
        }

    }

}
