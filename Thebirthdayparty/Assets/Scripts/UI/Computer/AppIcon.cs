using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class AppIcon : MonoBehaviour
{
    Action action;
    float tolerance = 0.5f;
    bool timer = false;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            CheckDoubleClick();
        });
    }
    public void SetAction(Action action)
    {
        this.action = action;
    }
    void CheckDoubleClick()
    {
        if (timer)
        {
            action?.Invoke();
            return;
        }
        timer = true;
        Invoke(nameof(Erase), tolerance);
    }
    void Erase()
    {
        timer = false;
    }

}
