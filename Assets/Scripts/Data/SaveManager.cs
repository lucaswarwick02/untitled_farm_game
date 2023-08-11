using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public static class SaveManager
    {
        public static event Action OnSaveChange;
        
        public static readonly string SavesFolderPath = Application.persistentDataPath + "/saves/";
        
        public static SaveData CurrentSave { get; private set; }
        
        public static void NewSave()
        {
            var newSave = new SaveData(Guid.NewGuid().ToString());
            var jsonString = SaveDataToString(newSave);
            var filename = newSave.guid + ".json";
            File.WriteAllText(SavesFolderPath + filename, jsonString);
            
            ProfileManager.CurrentProfile.mostRecentSave = SavesFolderPath + filename;
            ProfileManager.Save();

            Load(SavesFolderPath + filename);
        }
        
        public static void Load(string filepath)
        {
            var fileContents = File.ReadAllText(filepath);
            CurrentSave = StringToSaveData(fileContents);

            Save();

            OnSaveChange?.Invoke();
        }
        
        public static void Save()
        {
            CurrentSave.lastSaveTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            
            var jsonString = SaveDataToString(CurrentSave);
            var filename = CurrentSave.guid + ".json";
            File.WriteAllText(SavesFolderPath + filename, jsonString);
        }
        
        public static SaveData Peek(string filepath)
        {
            var fileContents = File.ReadAllText(filepath);
            return StringToSaveData(fileContents);
        }
        
        public static bool DoesSaveExist(string filepath)
        {
            return File.Exists(filepath);
        }
        
        private static SaveData StringToSaveData (string saveDataString)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };
            
            return JsonConvert.DeserializeObject<SaveData>(saveDataString, settings);
        }
        
        private static string SaveDataToString (SaveData saveData)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            return JsonConvert.SerializeObject(saveData, Formatting.Indented, settings);
        }

        public static string GuidToPath(string guid)
        {
            return SavesFolderPath + guid + ".json";
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad() {
            if (!File.Exists(SavesFolderPath))
            {
                Directory.CreateDirectory(SavesFolderPath);
            }
        }
    }
}