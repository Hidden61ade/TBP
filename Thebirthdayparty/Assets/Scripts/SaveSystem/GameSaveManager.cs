// // 在其他脚本中使用存档系统
// public class GameplayManager : MonoBehaviour
// {
//     void Start()
//     {
//         // 加载存档
//         GameSaveManager.Instance.LoadGame();
//     }

//     void OnInteractWithItem(string itemId)
//     {
//         // 记录物品交互
//         GameSaveManager.Instance.AddInteractedItem(itemId);
        
//         // 更新好感度
//         GameSaveManager.Instance.UpdateAffinity("Adam", 1.0f);
        
//         // 添加到收藏
//         GameSaveManager.Instance.AddToCollection(itemId, "documents");
//     }

//     void OnDayEnd()
//     {
//         // 推进游戏天数
//         GameSaveManager.Instance.AdvanceDay();
//     }
// }
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }
    public GameSaveData currentSave { get; private set; }
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(currentSave, Formatting.Indented);
            File.WriteAllText(saveFilePath, jsonData);
            Debug.Log("Game saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public void LoadGame()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                currentSave = JsonConvert.DeserializeObject<GameSaveData>(jsonData);
                Debug.Log("Game loaded successfully");
            }
            else
            {
                currentSave = new GameSaveData();
                SaveGame();
                Debug.Log("New game save created");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
            currentSave = new GameSaveData();
        }
    }

    public void UpdateAffinity(string character, float amount)
    {
        if (currentSave.affinity.ContainsKey(character))
        {
            currentSave.affinity[character] += amount;
            SaveGame();
        }
    }

    public void AddInteractedItem(string itemId)
    {
        currentSave.interactedItems.Add(itemId);
        SaveGame();
    }

    public void AdvanceDay()
    {
        currentSave.currentDay++;
        if (currentSave.currentDay > 7)
        {
            currentSave.currentDay = 1;
            currentSave.currentCycle++;
        }
        SaveGame();
    }

    public void SetPeriod(string period)
    {
        currentSave.currentPeriod = period;
        SaveGame();
    }

    public void AddToCollection(string itemId, string collectionType)
    {
        switch (collectionType)
        {
            case "documents":
                currentSave.collections.documents.Add(itemId);
                break;
            case "photos":
                currentSave.collections.photos.Add(itemId);
                break;
            case "music":
                currentSave.collections.music.Add(itemId);
                break;
            case "specialItems":
                currentSave.collections.specialItems.Add(itemId);
                break;
        }
        SaveGame();
    }
}