using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interactable;
using MoonSharp.Interpreter;
using QFramework;
using UnityEngine;

public class Script : MonoBehaviour
{
    Transform controlledObj;
    Camera controlledCam;
    Vector3 startPos;
    Quaternion startRot;
    private void Start()
    {
        controlledObj = transform.parent.parent.Find("Camera");
        controlledCam = controlledObj.GetComponent<Camera>();
        startPos = controlledObj.position;
        startRot = controlledObj.rotation;
        var temp = Camera.main.transform;
        controlledObj.position = temp.position;
        controlledObj.rotation = temp.rotation;
        TypeEventSystem.Global.Send<InteractStart>();
        StartCoroutine(GoToComputer());
    }
    IEnumerator GoToComputer()
    {
        controlledCam.depth = 2;
        InteractHintManager.Instance.gameObject.SetActive(false);
        yield return null;
        while (true)
        {
            controlledObj.position = Vector3.Lerp(controlledObj.position, startPos, 0.04f);
            controlledObj.rotation = Quaternion.Lerp(controlledObj.rotation, startRot, 0.04f);
            yield return null;
            if ((controlledObj.rotation.eulerAngles - startRot.eulerAngles).magnitude < 0.02 &&
                (controlledObj.position - startPos).magnitude < 0.02)
            {
                controlledObj.position = startPos;
                controlledObj.rotation = startRot;
                break;
            }
        }
        yield return null;
        float tempf = 0.01f;
        while (true)
        {
            controlledCam.fieldOfView -= tempf;
            tempf += 0.002f;
            yield return null;
            if (controlledCam.fieldOfView < 14)
            {
                break;
            }
        }
    }
}
