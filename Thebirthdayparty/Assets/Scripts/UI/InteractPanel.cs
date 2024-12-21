using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class InteractPanel : MonoSingleton<InteractPanel>
{
    public CanvasGroup canvasGroup;
    public TextPrinter textPrinter;
    public GameObject canvasObj;
    private bool isActive;
    private void Start()
    {
        TypeEventSystem.Global.Register<InteractTextQuited>(e =>
        {
            Disable();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    public void Activate()
    {
        canvasObj.SetActive(true);
        StartCoroutine(CanvasFader(1));
        textPrinter.interactable = true;
        isActive = true;
    }
    public void Disable()
    {
        StartCoroutine(CanvasFader(0, () => { canvasObj.SetActive(false); isActive = false; }));
        textPrinter.interactable = false;
    }
    IEnumerator CanvasFader(float target, Action action = null)
    {
        while (Mathf.Abs(canvasGroup.alpha - target) > 0.05f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, target, 5 * Time.deltaTime);
            yield return null;
        }
        canvasGroup.alpha = target;
        action?.Invoke();
    }
    public void Show(string[] contents, bool showAll = false)
    {
        if (!isActive)
        {
            Activate();
        }
        textPrinter.textArray = contents;
        textPrinter.PrintTextArray(showAll);
    }
}
public class InteractTextsStart { }
public class InteractTextQuited { }
public class InteractTextParagraphQuited { }
