using UnityEngine;
using System.Reflection;
using System;
using QFramework;
using Unity.VisualScripting;
using TMPro.EditorUtilities;
namespace Interactable
{
    public class InteractableObject : MonoBehaviour
    {
        public bool isOnThis;
        private bool hovered1 = false;
        #region On Hover
        public void OnHover()
        {
            hovered1 = true;
        }
        private void LateUpdate()
        {
            isOnThis = hovered1;
            hovered1 = false;
        }
        #endregion
        public void OnTriggered()
        {
            Debug.Log("Triggered");
            GameObject temp = Instantiate(interactiveData.prefab, transform);
        }
        public string DataName;
        private InteractiveData interactiveData;
        private void Start()
        {
            interactiveData = Resources.Load<InteractiveData>("ScriptableObjects/InteractiveObjects/" + DataName);
        }
    }
    public class InteractStart{}
    public class InteractEnd{}
}
