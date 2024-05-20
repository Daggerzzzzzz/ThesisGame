using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : SingletonMonoBehavior<SaveManager>
{
    [SerializeField]
    private string fileName;
    [SerializeField]
    private bool dataEncryption;
    
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private DataHandler dataHandler;

    protected override void Awake()
    {
        base.Awake();
        saveManagers = FindSaveManagers();
        dataHandler = new DataHandler(Application.persistentDataPath, fileName, dataEncryption);
        LoadGame();
    }
    
    private void NewGame()
    {
        gameData = new GameData();
    }

    private void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No Save Data Found!");
            NewGame();
            return;
        }
        
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    private void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindSaveManagers()
    {
        IEnumerable<ISaveManager> _saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(_saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }

    [ContextMenu("Delete Saved File")]
    public void DeleteSavedData()
    {
        dataHandler = new DataHandler(Application.persistentDataPath, fileName, dataEncryption);
        dataHandler.Delete();
    }
}   
