using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class LoadingPanel : MonoSingleton<LoadingPanel>
{
    private Transform filler;
    // Start is called before the first frame update
    void Start()
    {
        filler = transform.Find("Area/Filler");
    }
    public void SetPercentage(float arg)
    {
        var temp = arg;
        temp = Mathf.Clamp(temp, 0, 100);
        filler.transform.localPosition = new Vector3(temp-100,0,0);
    }

}
