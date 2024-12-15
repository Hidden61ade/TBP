using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using Interactable;
namespace G03
{
    public class Script : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(IERunning());
        }
        IEnumerator IERunning()
        {
            TypeEventSystem.Global.Send<InteractStart>();
            Debug.Log("Matt's napkin.");
            yield return new WaitForSeconds(3);
            Debug.Log("......");
            TypeEventSystem.Global.Send<NextPeriodEvent>();
            TypeEventSystem.Global.Send<RequireSaveGame>();
            TypeEventSystem.Global.Send<InteractEnd>();
        }
    }
}