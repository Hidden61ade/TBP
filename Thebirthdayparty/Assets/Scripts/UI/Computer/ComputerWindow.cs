using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComputerWindow : MonoBehaviour
{
    Vector3 lastMousePos;
    Vector2 direction;
    public Button button;
    private bool isDragging = false; // 是否开始拖动
    private Vector3 initialPosition; // 初始位置
    private RectTransform buttonRectTransform; // 按钮的RectTransform

    public void Quit(){
        ComputerUI.Instance.RemoveWindow(gameObject);
        gameObject.SetActive(false);
    }
    private void Start()
    {
        buttonRectTransform = button.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 检查鼠标是否点击在按钮区域
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverButton())
            {
                isDragging = true;
                lastMousePos = Input.mousePosition;
                initialPosition = transform.position;
                ComputerUI.Instance.OnWindowClicked(gameObject);
            }
        }

        // 拖动逻辑
        if (isDragging)
        {
            direction = (Vector2)(Input.mousePosition - lastMousePos);
            transform.position = initialPosition + (Vector3)direction;
        }

        // 结束拖动
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    // 判断鼠标是否点击在按钮区域
    private bool IsMouseOverButton()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(buttonRectTransform, Input.mousePosition, null, out Vector2 localPoint);
        return buttonRectTransform.rect.Contains(localPoint);
    }
}
