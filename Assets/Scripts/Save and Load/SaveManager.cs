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
    
    private void Start()
    {
        saveManagers = FindSaveManagers();
        dataHandler = new DataHandler(Application.persistentDataPath, fileName, dataEncryption);
        
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No Save Data Found!");
            NewGame();
        }
        
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
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

    [ContextMenu("Delete Saved File")]
    private void DeleteSavedData()
    {
        dataHandler = new DataHandler(Application.persistentDataPath, fileName, dataEncryption);
        dataHandler.Delete();
    }
}   
