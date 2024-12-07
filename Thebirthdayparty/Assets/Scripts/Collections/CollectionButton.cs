using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    [SerializeField] private GameObject collectionPanel;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleCollection);
    }

    private void ToggleCollection()
    {
        collectionPanel.SetActive(!collectionPanel.activeSelf);
    }
}