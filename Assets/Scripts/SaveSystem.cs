using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveProgressNew(int maxAmountOfScenes, int currentSceneIndex)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressData progressData = new ProgressData(maxAmountOfScenes, currentSceneIndex);
        formatter.Serialize(stream, progressData);
        stream.Close();
    }

    public static void SaveProgress(ProgressData progressData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressData _progressData = new ProgressData(progressData);
        formatter.Serialize(stream, _progressData);
        stream.Close();
    }

    public static ProgressData LoadProgress()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ProgressData progress = formatter.Deserialize(stream) as ProgressData;
            stream.Close();
            return progress;
        }
        else
        {
            return null;
        }
    }
}
