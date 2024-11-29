using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    [SerializeField] private NarrationEvent[] possibleNarrations;
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private string requiredTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered && triggerOnce) return;
        
        if (string.IsNullOrEmpty(requiredTag) || other.CompareTag(requiredTag))
        {
            NarrationManager.Instance.TriggerNarration(possibleNarrations);
            hasTriggered = true;
        }
    }

    public void TestTrigger()
    {
        NarrationManager.Instance.TriggerNarration(possibleNarrations);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }
}