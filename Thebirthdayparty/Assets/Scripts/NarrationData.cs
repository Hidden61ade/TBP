using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrationData", menuName = "Narration/Narration Data")]
public class NarrationData : ScriptableObject
{
    public NarrationEvent[] narrationEvents;
}
