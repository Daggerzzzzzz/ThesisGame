using System;
using System.IO;
using UnityEngine;

public class DataHandler
{
   private readonly string dataDirPath = "";
   private readonly string dataFileName = "";

   private bool dataEncryption = false;
   private string codeWord = "awikhatdog";

   public DataHandler(string dataDirPath, string dataFileName, bool dataEncryption)
   {
      this.dataDirPath = dataDirPath;
      this.dataFileName = dataFileName;
      this.dataEncryption = dataEncryption;
   }

   public void Save(GameData data)
   {
      string fullPath = Path.Combine(dataDirPath, dataFileName);

      try
      {
         Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

         string dataToStore = JsonUtility.ToJson(data, true);

         if (dataEncryption)
         {
            dataToStore = EncryptDecrypt(dataToStore);
         }

         using FileStream stream = new FileStream(fullPath, FileMode.Create);
         using StreamWriter writer = new StreamWriter(stream);
         writer.Write(dataToStore);
         
      }
      catch (Exception e)
      {
         Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
      }
   }

   public GameData Load()
   {
      string fullPath = Path.Combine(dataDirPath, dataFileName);

      GameData loadData = null;

      if (File.Exists(fullPath))
      {
         try
         {
            string dataToLoad = "";
            
            using FileStream stream = new FileStream(fullPath, FileMode.Open);
            using StreamReader reader = new StreamReader(stream);
            dataToLoad = reader.ReadToEnd();
            
            if (dataEncryption)
            {
               dataToLoad = EncryptDecrypt(dataToLoad);
            }

            loadData = JsonUtility.FromJson<GameData>(dataToLoad);
         }
         catch (Exception e)
         {
            Debug.LogError("Error on trying to load data to file: " + fullPath + "\n" + e);
         }
      }
      return loadData;
   }

   public void Delete()
   {
      string fullPath = Path.Combine(dataDirPath, dataFileName);

      if (File.Exists(fullPath))
      {
         File.Delete(fullPath);
      }
   }

   private string EncryptDecrypt(string data)
   {
      string modifiedData = "";

      for (int i = 0; i < data.Length; i++)
      {
         modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
      }

      return modifiedData;
   }
}
