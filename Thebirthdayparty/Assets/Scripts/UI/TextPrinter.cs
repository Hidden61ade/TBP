using System.Collections;
using UnityEngine;
using TMPro;
using QFramework;

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
        if(!interactable){
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
    public void PrintTextArray()
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
        StartCoroutine(PrintTexts(textArray));
    }

    // 打印一组文本的协程
    private IEnumerator PrintTexts(string[] intextArray)
    {
        while (currentTextIndex < textArray.Length)
        {
            // 开始打印当前文本
            isTyping = true;
            typingCoroutine = StartCoroutine(TypeText(textArray[currentTextIndex]));

            // 等待当前文本打印完毕后，再显示下一条文本
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
}
