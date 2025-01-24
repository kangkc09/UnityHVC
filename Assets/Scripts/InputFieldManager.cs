using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    public Canvas mainCanvas;         // UI Canvas
    public InputField inputField;     // InputField
    public Text outputText;           // Text to display the result
    private ChatController chatController;
    private bool isCanvasActive = false; // Canvas Ȱ��ȭ ����

    void Start()
    {
        // �ʱ� ���·� Canvas�� ��Ȱ��ȭ
        mainCanvas.gameObject.SetActive(false);
        chatController = GetComponent<ChatController>();

    }

    void Update()
    {
        // Tap Ű�� ���� Canvas Ȱ��ȭ/��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isCanvasActive = !isCanvasActive;

            // Canvas�� Ȱ��ȭ�Ǹ� InputField�� ��Ŀ��
            if (isCanvasActive)
            {
                mainCanvas.gameObject.SetActive(isCanvasActive);
                inputField.text = ""; // �Է� �ʵ� �ʱ�ȭ
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
