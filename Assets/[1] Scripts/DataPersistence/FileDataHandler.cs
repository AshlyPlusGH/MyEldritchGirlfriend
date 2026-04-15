using UnityEngine;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor.Timeline;
using UnityEditor;

public class FileDataHandler : MonoBehaviour
{
    private string dataDirPath = "";
    private string dataFileName = "";
    
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // create the directory the file will be written on
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize C# game data into JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when tring to save data file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from JSON back to C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }

            catch(Exception e) 
            {
                Debug.LogError("Error occured while trying to load data from file: " + fullPath + "\n" + e);
            }


        }
        return loadedData;
    }
}
