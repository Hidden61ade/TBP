using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class InteractAppearance : MonoBehaviour
{
    Transform player;
    float range = 4;
    private bool hasRendered = false;
    private GameObject m_icon;
    private void OnEnable()
    {
        player = InteractHintManager.Instance.Player.transform;
        range = InteractHintManager.Instance.Range;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < range)
        {
            if (!hasRendered)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                this.m_icon = InteractHintManager.Instance.AllocIcon();
                m_icon.GetComponent<RectTransform>().anchoredPosition3D  = pos;
                hasRendered = true;
            }
            else
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                m_icon.GetComponent<RectTransform>().anchoredPosition3D  = pos;
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
    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,4);
    }
}
