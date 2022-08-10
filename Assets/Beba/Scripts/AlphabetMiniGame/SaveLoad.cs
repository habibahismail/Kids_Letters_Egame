using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace bebaSpace.AlphabetMiniGame
{
    public class SaveLoad : MonoBehaviour
    {
        public static void Save<T>(T objectToSave, string key)
        {
            string path = ReturnSavePath();
            Directory.CreateDirectory(path);

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = new FileStream(path + key + ".dat", FileMode.Create))
            {
                formatter.Serialize(fileStream, objectToSave);

            }

        }

        public static T Load<T>(string key)
        {
            string path = ReturnSavePath(key);

            BinaryFormatter formatter = new BinaryFormatter();
            T returnValue = default(T);

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                returnValue = (T)formatter.Deserialize(fileStream);

            }

            return returnValue;
        }

        public static bool SaveExist(string key)
        {
            string path = ReturnSavePath(key);
            return File.Exists(path);
        }

        public static void DeleteAllSavedFiles()
        {
            string path = ReturnSavePath();
            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Delete(true);
            Directory.CreateDirectory(path);
        }


        public static string ReturnSavePath(string key)
        {
            string path = Application.persistentDataPath + "/saves/" + key + ".dat";
            return path;
        }

        public static string ReturnSavePath()
        {
            string path = Application.persistentDataPath + "/saves/";
            return path;
        }
    }

}
