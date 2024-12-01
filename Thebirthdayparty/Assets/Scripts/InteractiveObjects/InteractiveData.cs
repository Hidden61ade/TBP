using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    [CreateAssetMenu(fileName = "InteractiveData", menuName = "InteractiveData")]
    public class InteractiveData : ScriptableObject
    {
        public enum Type
        {
            Narration,
            Book,
            Obtainable,
            Invalid
        }
        public int id;
        public string obj_name;
        public Type type;
        public GameObject prefab;
    }
}
