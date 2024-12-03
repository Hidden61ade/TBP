using System.Collections;
using System.Collections.Generic;
using Interactable;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private Button m_button;
    void Start()
    {
        m_button = GetComponentInChildren<Button>();

        m_button.onClick.AddListener(() =>
        {
            SceneController.Instance.SwitchScene("SampleScene");
        });
    }
}
