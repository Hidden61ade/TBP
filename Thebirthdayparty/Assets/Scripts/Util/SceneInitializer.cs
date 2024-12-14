using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    private void Start() {
        TypeEventSystem.Global.Send(new OnDayEventTriggered(GameTimeManager.Instance.GetCurrentEvent()));
    }
}
