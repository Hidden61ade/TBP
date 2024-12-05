using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace Interactable
{
    public partial class InteractAppearance : MonoBehaviour
    {
        Transform player;
        public float image_range;
        [HideInInspector] public string iconName;
        public bool IsHighlighted
        {
            get
            {
                return _highlighted;
            }
            set
            {
                if (IsHighlighted && (!value))
                {
                    DeHighlight();
                    _highlighted = value;
                }
                else if ((!IsHighlighted) && value)
                {
                    EnHighlight();
                    _highlighted = value;
                }
            }
        }
        private bool _highlighted;
        private string m_text;
        private float range;
        private bool hasRendered = false;
        private GameObject m_icon;
        private void OnEnable()
        {
            player = InteractHintManager.Instance.Player.transform;
            range = InteractHintManager.Instance.Range;
        }
        private void OnDisable() {
            if(!m_icon.IsUnityNull()){
                Destroy(m_icon);
            }
        }
        void Update()
        {
            m_text = iconName;
            if (IsHighlighted)
            {
                m_text = "<i><u>" + iconName;
            }
            if (Vector3.Dot(player.forward, transform.position - player.position) <= 0)
            {
                if (!m_icon.IsUnityNull())
                {
                    Destroy(m_icon);
                    hasRendered = false;
                }
                return;
            }
            if (Vector3.Distance(transform.position, player.position) < range)
            {
                if (!hasRendered)
                {
                    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                    this.m_icon = InteractHintManager.Instance.AllocIcon();
                    m_icon.GetComponentInChildren<TextMeshProUGUI>().text = m_text;
                    m_icon.GetComponent<RectTransform>().anchoredPosition3D = pos;
                    hasRendered = true;
                }
                else
                {
                    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                    m_icon.GetComponent<RectTransform>().anchoredPosition3D = pos;
                }
            }
            else
            {
                if (hasRendered)
                {
                    Destroy(m_icon);
                    hasRendered = false;
                }
            }
        }
        public void UpdateText()
        {
            if (m_icon.IsUnityNull())
            {
                return;
            }
            m_icon.GetComponentInChildren<TextMeshProUGUI>().text = m_text;
        }
        private void EnHighlight()
        {
            m_text = "<u><i>" + iconName;
            UpdateText();
        }
        private void DeHighlight()
        {
            m_text = iconName;
            UpdateText();
        }
        [ExecuteAlways]
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, image_range);
        }
    }
}
