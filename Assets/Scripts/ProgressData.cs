using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public bool[] levels;
    public ProgressData(int maxAmountOfScenes, int currentSceneIndex)
    {
        levels = new bool[maxAmountOfScenes];
        int index = 0;
        while (index <= currentSceneIndex)
        {
            levels[index] = true;
            index++;
        }
    }
    public ProgressData(ProgressData progressData)
    {
        this.levels = progressData.levels;
    }
}
