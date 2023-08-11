using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public static class ProfileManager
    {
        private static readonly string ProfileFilePath = Application.persistentDataPath + "/profile.json";
        
        public static ProfileData CurrentProfile { get; private set; }
        
        private static void NewProfile()
        {
            var newProfile = new ProfileData();
            var jsonString = ProfileDataToString(newProfile);
            File.WriteAllText(ProfileFilePath, jsonString);
            
            CurrentProfile = newProfile;
        }
        
        public static void Save()
        {
            var jsonString = ProfileDataToString(CurrentProfile);
            // Debug.Log(jsonString);
            File.WriteAllText(ProfileFilePath, jsonString);
        }
        
        private static string ProfileDataToString(ProfileData profileData)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            return JsonConvert.SerializeObject(profileData, Formatting.Indented, settings);
        }
        
        private static ProfileData StringToProfileData (string profileDataString)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };
            
            return JsonConvert.DeserializeObject<ProfileData>(profileDataString, settings);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeMethodLoad()
        {
            if (File.Exists(ProfileFilePath))
            {
                var profileString = File.ReadAllText(ProfileFilePath);
                CurrentProfile = StringToProfileData(profileString);
            }
            else
            {
                NewProfile();
            }
        }
    }
}