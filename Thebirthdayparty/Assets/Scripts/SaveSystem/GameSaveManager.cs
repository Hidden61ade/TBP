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
using QFramework;

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
    private void Start() {
        TypeEventSystem.Global.Register<RequireSaveGame>(e=>{
            SaveGame();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
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
                if ((!currentSave.firstPlayed) && GameTimeManager.Instance.GetCurrentDay() == 1 && GameTimeManager.Instance.GetCurrentPeriod() == GameTime.Morning)
                {
                    currentSave.ResetObjectsInfo();
                    Debug.Log("New cycle. Object infomation reset.");
                    SaveGame();
                }
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
        if (currentSave.interactedItems.Contains(itemId))
        {
            return;
        }
        currentSave.interactedItems.Add(itemId);
    }
    public bool HasInteracted(string itemId)
    {
        return currentSave.interactedItems.Contains(itemId);
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

    public void AddToCollection(string itemId, string collectionType, int lockedMode = 1)
    {
        switch (collectionType)
        {
            case "documents":
                currentSave.collections.documents.Add(new CollectionState(itemId, lockedMode));
                break;
            case "photos":
                currentSave.collections.photos.Add(new CollectionState(itemId, lockedMode));
                break;
            case "music":
                currentSave.collections.music.Add(new CollectionState(itemId, lockedMode));
                break;
            case "specialItems":
                currentSave.collections.specialItems.Add(new CollectionState(itemId, lockedMode));
                break;
        }
    }
    public CollectionState GetCollectionState(string itemId, string collectionType)
    {
        return collectionType switch
        {
            "documents" => currentSave.collections.documents.FirstOrDefault(doc => doc.ID == itemId),
            "photos" => currentSave.collections.photos.FirstOrDefault(doc => doc.ID == itemId),
            "music" => currentSave.collections.music.FirstOrDefault(doc => doc.ID == itemId),
            "specialItems" => currentSave.collections.specialItems.FirstOrDefault(doc => doc.ID == itemId),
            _ => null,
        };
    }
    public void UnlockCollection(string itemId, string collectionType, int lockedMode = 1)
    {
        switch (collectionType)
        {
            case "documents":
                if (currentSave.collections.documents.TryGetValue(GetCollectionState(itemId, collectionType), out CollectionState value))
                {
                    value.lockedStates = lockedMode;
                }
                else
                {
                    AddToCollection(itemId, collectionType);
                }
                break;
            case "photos":
                if (currentSave.collections.photos.TryGetValue(GetCollectionState(itemId, collectionType), out CollectionState value1))
                {
                    value1.lockedStates = lockedMode;
                }
                else
                {
                    AddToCollection(itemId, collectionType);
                }
                break;
            case "music":
                if (currentSave.collections.music.TryGetValue(GetCollectionState(itemId, collectionType), out CollectionState value2))
                {
                    value2.lockedStates = lockedMode;
                }
                else
                {
                    AddToCollection(itemId, collectionType);
                }
                break;
            case "specialItems":
                if (currentSave.collections.specialItems.TryGetValue(GetCollectionState(itemId, collectionType), out CollectionState value3))
                {
                    value3.lockedStates = lockedMode;
                }
                else
                {
                    AddToCollection(itemId, collectionType);
                }
                break;
            default: break;
        }
    }
}