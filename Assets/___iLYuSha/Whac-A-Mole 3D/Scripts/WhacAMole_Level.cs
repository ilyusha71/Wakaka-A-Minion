using System.Collections.Generic;
using UnityEngine;

/* 遊戲難度水平 */
public partial class WhacAMole : MonoBehaviour
{
    List<int> levelBox = new List<int>();
    private int magnitudeLevel;

    void InitializeLevel()
    {
        SetLevel1();
        SetLevel2();
        SetLevel3();

        // 以下未來做正式版
        //Level1();
        //Level2();
        //Level3();
        //Level4();
        //Level5();
        //capture += 500;
    }
    void EnterLevel1()
    {
        magnitudeLevel = 6;
        delay = Time.time + Random.Range(1, 4) * 0.197f;
    }
    void EnterLevel2()
    {
        magnitudeLevel = 11;
        delay = Time.time + Random.Range(1, 4) * 0.173f;
    }
    void EnterLevel3()
    {
        magnitudeLevel = 15;
        delay = Time.time + Random.Range(1, 4) * 0.163f;
    }



    void SetLevel1()
    {
        int countLevelBox = levelBox.Count;
        levelBox.Add((int)CharacterIndex.Minion);
        levelBox.Add((int)CharacterIndex.Minion);
        levelBox.Add((int)CharacterIndex.SpongeBob);
        levelBox.Add((int)CharacterIndex.SpongeBob);
        levelBox.Add((int)CharacterIndex.Mole);
        levelBox.Add((int)CharacterIndex.Dorara);
        Debug.LogWarning("Level 1 Data: " + countLevelBox + " to " + levelBox.Count);
        Probability();
    }
    void SetLevel2()
    {
        int countLevelBox = levelBox.Count;
        levelBox.Add((int)CharacterIndex.Minion);
        levelBox.Add((int)CharacterIndex.SpongeBob);
        levelBox.Add((int)CharacterIndex.Dorara);
        levelBox.Add((int)CharacterIndex.Faceless);
        levelBox.Add((int)CharacterIndex.WangNiMa);
        Debug.LogWarning("Level 2 Data: " + countLevelBox + " to " + levelBox.Count);
        Probability();
    }
    void SetLevel3()
    {
        int countLevelBox = levelBox.Count;
        levelBox.Add((int)CharacterIndex.Mole);
        levelBox.Add((int)CharacterIndex.Faceless);
        levelBox.Add((int)CharacterIndex.WangNiMa);
        levelBox.Add((int)CharacterIndex.MengZong);
        Debug.LogWarning("Level 3 Data: " + countLevelBox + " to " + levelBox.Count);
        Probability();
    }
    void Level4()
    {
        int countLevelBox = levelBox.Count;
        levelBox.Add((int)CharacterIndex.Dorara);
        levelBox.Add((int)CharacterIndex.Faceless);
        levelBox.Add((int)CharacterIndex.WangNiMa);
        Debug.LogWarning("Level 4 Data: " + countLevelBox + " to " + levelBox.Count);
        Probability();
    }
    void Level5()
    {
        int countLevelBox = levelBox.Count;
        levelBox.Add((int)CharacterIndex.WangNiMa);
        levelBox.Add((int)CharacterIndex.MengZong);
        levelBox.Add((int)CharacterIndex.Pandaman);
        Debug.LogWarning("Level 5 Data: " + countLevelBox + " to " + levelBox.Count);
        Probability();
    }

    void Probability()
    {
        int[] counters = new int[countCharacter];
        for (int i = 0; i < levelBox.Count; i++)
        {
            counters[levelBox[i]]++;
        }

        for (int j = 0; j < counters.Length; j++)
        {
            Debug.Log(((CharacterIndex)j).ToString() + ": " + counters[j] + " times, " + (float)counters[j] * 100 / (float)levelBox.Count + "%");
        }
    }

}
