using System.Collections;
using UnityEngine;
using TMPro;
using QFramework;
using System;

public class TextPrinter : MonoBehaviour
{
    public bool interactable = true;
    public TextMeshProUGUI textMeshPro;  // TextMeshProUGUI组件
    public float typingSpeed = 0.05f;    // 每个字符之间的间隔时间

    private int currentTextIndex = 0;    // 当前正在显示的文本索引
    private bool isTyping = false;       // 当前是否在打印动画中
    private bool isAbleToMove = false;
    private Coroutine typingCoroutine;    // 当前正在执行的协程
    public string[] textArray;
    void Update()
    {
        if (!interactable)
        {
            return;
        }
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // 如果正在打印中，跳过当前文本动画，直接显示完整文本
                StopTyping();
            }
            else
            {
                isAbleToMove = true;
            }
        }
    }

    // 启动打印一组文本的过程
    [InspectorButton("打印")]
    public void PrintTextArray(bool showAll)
    {
        // 如果当前有文本在打印，停止当前的打印
        if (isTyping)
        {
            StopTyping();
        }

        // 清空文本并重置索引
        textMeshPro.text = "";
        currentTextIndex = 0;

        // 开始打印文本数组中的第一条文本
        StartCoroutine(PrintTexts(textArray, showAll));
    }

    // 打印一组文本的协程
    private IEnumerator PrintTexts(string[] intextArray, bool showAll)
    {
        // 确定打印范围
        int startIndex = 0;
        int endIndex = 0;
        int currentCycle = GameSaveManager.Instance.currentSave.currentCycle;

        switch (currentCycle)
        {
            case 1:
                startIndex = 0;
                endIndex = Math.Min(3, textArray.Length);
                break;
            case 2:
                startIndex = 3;
                endIndex = Math.Min(6, textArray.Length);
                break;
            case 3:
                startIndex = 6;
                endIndex = Math.Min(9, textArray.Length);
                break;
            default:
                // 如果cycle > 3，显示最后3条文本
                startIndex = Math.Max(0, textArray.Length - 3);
                endIndex = textArray.Length;
                break;
        }

        // 如果起始索引超出数组范围，显示最后3条
        if (startIndex >= textArray.Length)
        {
            startIndex = Math.Max(0, textArray.Length - 3);
            endIndex = textArray.Length;
        }

        // 设置当前文本索引为起始位置
        currentTextIndex = startIndex;
        if (showAll)
        {
            currentTextIndex = 0;
            endIndex = textArray.Length;
        }
        // 在确定的范围内打印文本
        while (currentTextIndex < endIndex)
        {
            isTyping = true;
            typingCoroutine = StartCoroutine(TypeText(textArray[currentTextIndex]));

            yield return new WaitUntil(() => !isTyping);
            isAbleToMove = false;
            yield return new WaitUntil(() => isAbleToMove);
        }

        TypeEventSystem.Global.Send<InteractTextParagraphQuited>();
    }

    // 逐字打印文本的协程
    private IEnumerator TypeText(string text)
    {
        isAbleToMove = false;
        textMeshPro.text = "";  // 清空当前文本

        // 遍历每个字符，逐个显示
        for (int i = 0; i < text.Length; i++)
        {
            textMeshPro.text += text[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        // 打印完当前文本
        isTyping = false;
        isAbleToMove = false;
        currentTextIndex++;  // 当前文本索引加1
    }

    // 停止当前打印动画，直接显示完整文本
    private void StopTyping()
    {
        if (typingCoroutine != null)
        {
            // 如果有正在执行的打印协程，停止它
            StopCoroutine(typingCoroutine);
            textMeshPro.text = textArray[currentTextIndex];  // 直接显示完整文本
            isTyping = false;
            isAbleToMove = false;
            currentTextIndex++;  // 当前文本索引加1
        }
    }

    // 检查循环的方法
    private bool CheckCycleCondition(int requiredCycle)
    {
        return GameSaveManager.Instance.currentSave.currentCycle >= requiredCycle;
    }
}
