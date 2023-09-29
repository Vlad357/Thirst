using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Aquapunk
{

    public class LoadManager : MonoBehaviour
    {
        private string _filePath;

        private void Start()
        {
            _filePath = Application.persistentDataPath + "/save.gamesave";
            LoadGame();
        }

        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = new FileStream(_filePath, FileMode.Create);

            Save save = new Save();

            save.SaveWater(FindObjectOfType<Player>());

            bf.Serialize(fileStream, save);
            fileStream.Close();
        }

        public void LoadGame()
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(_filePath, FileMode.Open);

            Save save = (Save)bf.Deserialize(fs);

            fs.Close();

            FindObjectOfType<Player>().WaterCounter = save.count;
            FindObjectOfType<Player>().UpdateWaterCount();
        }
    }

    [System.Serializable]
    public class Save
    {
        //[System.Serializable]
        //public struct WaterCount
        //{
        //    public float count;
        //
        //    public WaterCount(float count)
        //    {
        //        this.count = count;
        //    }
        //}

        public float count;

        public void SaveWater(Player player)
        {
            count = player.WaterCounter;
        }


    }
}
