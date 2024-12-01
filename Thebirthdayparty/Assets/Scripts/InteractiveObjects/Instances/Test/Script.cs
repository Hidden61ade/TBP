using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
namespace Interactable
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
            Debug.Log("A potted plant.");
            yield return null;
            while(true){
                if(Input.GetMouseButtonDown(0)) break;
                yield return null;
            }
            Debug.Log("没啥, 就是盆植物");
            yield return null;
            while(true){
                if(Input.GetMouseButtonDown(0)) break;
                yield return null;
            }
            Debug.Log("一盆植物");
            yield return null;
            while(true){
                if(Input.GetMouseButtonDown(0)) break;
                yield return null;
            }
            Debug.Log("......");
            TypeEventSystem.Global.Send<InteractEnd>();
        }
    }
}