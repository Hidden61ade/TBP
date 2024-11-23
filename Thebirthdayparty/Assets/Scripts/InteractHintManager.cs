using UnityEngine;
using QFramework;

public class InteractHintManager : MonoSingleton<InteractHintManager>
{
    public GameObject Icon;
    public GameObject Player;
    public float Range;
    public GameObject AllocIcon(){
        return Instantiate(Icon,transform);
    }
}
