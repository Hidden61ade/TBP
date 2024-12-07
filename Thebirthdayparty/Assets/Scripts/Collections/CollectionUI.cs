using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; 

public class CollectionUI : MonoBehaviour
{
    [Header("Main Grid View")]
    [SerializeField] private GameObject gridContainer;
    [SerializeField] private GameObject collectionItemPrefab;
    [SerializeField] private GameObject lockedItemPrefab;

    [Header("Grid Settings")]
    [SerializeField] private int columnsCount = 4;
    [SerializeField] private float spacing = 15f;
    [SerializeField] private float cellSize = 150f;
    
    [Header("Pagination")]
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button prevPageButton;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private int itemsPerPage = 12; // 每页显示的物品数量

    [Header("Detail View")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image detailImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionL1;
    [SerializeField] private TextMeshProUGUI descriptionL2;
    [SerializeField] private Button closeDetailButton;

    private int currentPage = 0;
    private CollectionData[] allCollections;
    private List<GameObject> currentPageItems = new List<GameObject>();

     private void Start()
    {
        SetupGridLayout();
        
        if (closeDetailButton != null)
            closeDetailButton.onClick.AddListener(CloseDetailView);
            
        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(NextPage);
            
        if (prevPageButton != null)
            prevPageButton.onClick.AddListener(PreviousPage);
        
        if (CollectionManager.Instance != null)
        {
            allCollections = CollectionManager.Instance.GetAllCollections();
            if (allCollections != null && allCollections.Length > 0)
            {
                Debug.Log($"Loaded {allCollections.Length} collections");
                UpdatePage();
            }
            else
            {
                Debug.LogWarning("No collections data available!");
            }
        }
        else
        {
            Debug.LogError("CollectionManager instance not found!");
        }
    }

    private void SetupGridLayout()
    {
        // 确保网格容器有GridLayoutGroup组件
        GridLayoutGroup gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = gridContainer.AddComponent<GridLayoutGroup>();
        }

        // 设置网格布局属性
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columnsCount;
        gridLayout.spacing = new Vector2(spacing, spacing);
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
        gridLayout.padding = new RectOffset(20, 20, 20, 20);
    }

    private void UpdatePage()
    {
        Debug.Log("Updating page...");
        // 清除当前页面的物品
        foreach (var item in currentPageItems)
        {
            if (item != null)
                Destroy(item);
        }
        currentPageItems.Clear();

        if (allCollections == null || allCollections.Length == 0)
        {
            Debug.LogWarning("No collections to display");
            return;
        }

        // 计算当前页的起始和结束索引
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, allCollections.Length);

        Debug.Log($"Displaying items from index {startIndex} to {endIndex}");

        // 加载当前页的物品
        for (int i = startIndex; i < endIndex; i++)
        {
            var collection = allCollections[i];
            GameObject itemObj;

            if (collection == null)
            {
                Debug.LogError($"Collection at index {i} is null!");
                continue;
            }

            if (collection.isUnlocked)
            {
                if (collectionItemPrefab == null)
                {
                    Debug.LogError("Collection item prefab is not assigned!");
                    continue;
                }

                itemObj = Instantiate(collectionItemPrefab, gridContainer.transform);
                Image itemImage = itemObj.GetComponent<Image>();
                if (itemImage != null && collection.itemIcon != null)
                {
                    itemImage.sprite = collection.itemIcon;
                }
                else
                {
                    Debug.LogWarning($"Missing image component or icon for collection {collection.itemName}");
                }

                var currentCollection = collection;
                Button button = itemObj.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => ShowDetailView(currentCollection));
                }
            }
            else
            {
                if (lockedItemPrefab == null)
                {
                    Debug.LogError("Locked item prefab is not assigned!");
                    continue;
                }

                itemObj = Instantiate(lockedItemPrefab, gridContainer.transform);
                TextMeshProUGUI unlockText = itemObj.GetComponentInChildren<TextMeshProUGUI>();
                if (unlockText != null)
                {
                    unlockText.text = collection.unlockCondition;
                }
            }

            currentPageItems.Add(itemObj);
        }

        // 更新页码显示
        if (pageText != null)
        {
            int totalPages = Mathf.CeilToInt((float)allCollections.Length / itemsPerPage);
            pageText.text = $"Page {currentPage + 1}/{totalPages}";
            Debug.Log($"Updated page text: {pageText.text}");
        }

        // 更新按钮状态
        if (prevPageButton != null)
            prevPageButton.interactable = currentPage > 0;
        
        if (nextPageButton != null)
        {
            int totalPages = Mathf.CeilToInt((float)allCollections.Length / itemsPerPage);
            nextPageButton.interactable = currentPage < totalPages - 1;
        }
    }

     private void NextPage()
    {
        currentPage++;
        UpdatePage();
    }

    private void PreviousPage()
    {
        currentPage--;
        UpdatePage();
    }
    
    private void LoadCollections()
    {
        var collections = CollectionManager.Instance.GetAllCollections();
        foreach (var collection in collections)
        {
            GameObject itemObj;
            if (collection.isUnlocked)
            {
                itemObj = Instantiate(collectionItemPrefab, gridContainer.transform);
                itemObj.GetComponent<Image>().sprite = collection.itemIcon;
                itemObj.GetComponent<Button>().onClick.AddListener(() => ShowDetailView(collection));
            }
            else
            {
                itemObj = Instantiate(lockedItemPrefab, gridContainer.transform);
                itemObj.GetComponentInChildren<TextMeshProUGUI>().text = collection.unlockCondition;
            }
        }
    }

    private void ShowDetailView(CollectionData data)
    {
        detailPanel.SetActive(true);
        detailImage.sprite = data.itemFullImage;
        itemNameText.text = data.itemName;
        descriptionL1.text = data.descriptionL1;
        descriptionL2.text = data.descriptionL2;
    }

    private void CloseDetailView()
    {
        detailPanel.SetActive(false);
    }
}