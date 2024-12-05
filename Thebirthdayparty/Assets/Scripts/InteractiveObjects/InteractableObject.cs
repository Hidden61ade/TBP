using UnityEngine;
using QFramework;
using Unity.VisualScripting;
namespace Interactable
{
    public class InteractableObject : MonoBehaviour
    {
        public bool isOnThis;
        private bool hovered1 = false;
        private GameObject temp;
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
            temp = Instantiate(interactiveData.prefab, transform);
        }

        public string DataName;
        private InteractiveData interactiveData;
        private void Start()
        {
            interactiveData = Resources.Load<InteractiveData>("ScriptableObjects/InteractiveObjects/" + DataName);
            GetComponent<InteractAppearance>().iconName = interactiveData.obj_name;
            TypeEventSystem.Global.Register<InteractEnd>(e =>
            {
                if (!temp.IsUnityNull())
                {
                    Destroy(temp);
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (isOnThis)
            {
                GetComponent<InteractAppearance>().IsHighlighted = true;
            }else{
                GetComponent<InteractAppearance>().IsHighlighted = false;
            }
        }
    }
    public class InteractStart { }
    public class InteractEnd { }
}
