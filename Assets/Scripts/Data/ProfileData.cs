using System;

namespace Data
{
    [Serializable]
    public class ProfileData
    {
        public string mostRecentSave;
        
        public ProfileData()
        {
            mostRecentSave = "";
        }
    }
}