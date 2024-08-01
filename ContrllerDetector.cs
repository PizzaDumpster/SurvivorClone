using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerDetector : MonoBehaviour
{
    public TMP_Text statusText;
    private bool wasControllerConnected = false;

    void Update()
    {
        bool isControllerConnected = IsControllerConnected();

        if (isControllerConnected != wasControllerConnected)
        {
            UpdateStatusText(isControllerConnected);
            wasControllerConnected = isControllerConnected;
        }
    }

    bool IsControllerConnected()
    {
        string[] joystickNames = Input.GetJoystickNames();
        return joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]);
    }

    void UpdateStatusText(bool isConnected)
    {
        if (statusText != null)
        {
            statusText.text = isConnected ? "Controller Connected" : "Controller Disconnected";
            statusText.color = isConnected ? Color.green : Color.red;
        }
        else
        {
            Debug.LogWarning("Status Text component is not assigned!");
        }
    }
}
