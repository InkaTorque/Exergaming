using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TreatmentItem
{
    public string name;
    public int reps, time;
    public bool done;

    public TreatmentItem()
    {

    }
}

[System.Serializable]
public class MinigameList
{
    public string name;
    public GameObject container;
}

public class GameManager : MonoBehaviour {

    public List<TreatmentItem> treatment;
    public static GameManager instance;
    public Dictionary<string,int> currentPathologyDict;
    public MinigameList[] minigameList;
    public Dictionary<string, GameObject> minigameDict;
    public bool debug;

    void Start()
    {
        minigameDict = new Dictionary<string, GameObject>();
        instance = this;
        for(int i=0;i<minigameList.Length;i++)
        {
            minigameDict.Add(minigameList[i].name, minigameList[i].container);
        }
    }

	public void LaunchExergame(string response, Dictionary<string,int> dict)
    {
        Debug.Log("RSPONSE = " + response);
        currentPathologyDict = dict;
        treatment = new List<TreatmentItem>();
        string[] exercises = response.Split(';');
        int level;
        foreach(string exr in exercises)
        {
            if(exr.Length>2)
            {
                string[] results = exr.Split('|');
                TreatmentItem ti = new TreatmentItem();
                ti.name = results[1];
                ti.done = false;
                level = currentPathologyDict[results[0]];
                ti.reps = int.Parse(results[3 + level + (level - 2)]);
                ti.time = int.Parse(results[3 + level + (level - 1)]);
                treatment.Add(ti);

            }
        }
        stratRandomGame();
    }

    public void stratRandomGame()
    {
        Debug.Log("STARTING GAME");
        int gameIndex = 0;
        string exerciseName;
        if(debug)
        {
            exerciseName = treatment[gameIndex].name;
            Debug.Log("starting game " + exerciseName);
            minigameDict[exerciseName].GetComponent<MinigameOverseer>().StartGame(treatment[gameIndex].time, treatment[gameIndex].reps);
        }
        else
        {
            do
            {
                System.Random rnd = new System.Random();
                gameIndex = rnd.Next(0, treatment.Count);
            } while (treatment[gameIndex].done == true);
            treatment[gameIndex].done = true;
            exerciseName = treatment[gameIndex].name;
            Debug.Log("starting game " + exerciseName);
            minigameDict[exerciseName].GetComponent<MinigameOverseer>().StartGame(treatment[gameIndex].time, treatment[gameIndex].reps);

        }
    }
}
