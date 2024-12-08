using System.Collections;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ComputerMsg
{
    public class Message : MonoBehaviour
    {
        public RectTransform childRect;
        public void SetText(string str)
        {
            StartCoroutine(IESetText(str));
        }
        IEnumerator IESetText(string str)
        {
            GetComponentInChildren<TextMeshProUGUI>().SetText(str);
            yield return new WaitForSeconds(0.01f);
            Vector2 childWorldSize = childRect.rect.size;
            Debug.Log(childRect.rect.size);
            // 获取父物体的 RectTransform 并修改高度
            RectTransform rt = GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;
            size.y = childWorldSize.y;
            rt.sizeDelta = size;
            // gameObject.SetActive(false);
            // Invoke(nameof(Reactivate),0.01f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt.parent.GetComponent<RectTransform>());
        }
        void Reactivate(){
            gameObject.SetActive(true);
        }
    }
}
