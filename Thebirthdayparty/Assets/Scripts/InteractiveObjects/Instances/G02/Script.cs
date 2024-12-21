using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using Interactable;
namespace G02
{
    public class Script : MonoBehaviour
    {
        public string[] content;
        bool isAbleToQuit = false;
        private void Start()
        {
            TypeEventSystem.Global.Register<InteractTextParagraphQuited>(e =>
            {
                TypeEventSystem.Global.Send<InteractTextQuited>();
                isAbleToQuit = true;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            StartCoroutine(IERunning());
        }
        IEnumerator IERunning()
        {
            TypeEventSystem.Global.Send<InteractStart>();
            if (content.Length > 0)
            {
                InteractPanel.Instance.Show(content);
            }
            yield return new WaitUntil(() => isAbleToQuit);
            TypeEventSystem.Global.Send<NextPeriodEvent>();
            TypeEventSystem.Global.Send<RequireSaveGame>();
            TypeEventSystem.Global.Send<InteractEnd>();
        }
    }
}