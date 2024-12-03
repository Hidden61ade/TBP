using System.Collections;
using System.Collections.Generic;
using Interactable;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class ComputerUI : MonoBehaviour
{
    private Button m_quitButton;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_quitButton = transform.Find("QuitButton").GetComponent<Button>();

        m_quitButton.onClick.AddListener(() =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            TypeEventSystem.Global.Send<InteractEnd>();
            SceneController.Instance.SwitchSceneTo("SampleScene");
        });
    }
}
