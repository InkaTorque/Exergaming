using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TreatmentItem
{
    public string name;
    public int reps, time;
    public bool done,available;

    public TreatmentItem()
    {

    }
}

[System.Serializable]
public class MiniGameListDetail
{
    public GameObject container;
    public bool available;
    public float desiredCertanty;
}

[System.Serializable]
public class MinigameList
{
    public string name;
    public MiniGameListDetail detail;
}

public class GameManager : MonoBehaviour {

    public List<TreatmentItem> treatment;
    public static GameManager instance;
    public Dictionary<string,int> currentPathologyDict;
    public MinigameList[] minigameList;
    public Dictionary<string, MiniGameListDetail> minigameDict;
    public bool debug;

    private int availableCounter,usedCounter;

    void Start()
    {
        minigameDict = new Dictionary<string, MiniGameListDetail>();
        instance = this;
        availableCounter = 0;
        for(int i=0;i<minigameList.Length;i++)
        {
            minigameDict.Add(minigameList[i].name, minigameList[i].detail);
            if(minigameList[i].detail.available==true)
            {
                availableCounter++;
            }
        }
    }

	public void LaunchExergame(string response, Dictionary<string,int> dict)
    {
        Debug.Log("REsponse "+response);
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
                ti.available = minigameDict[ti.name].available;
                level = currentPathologyDict[results[0]];
                ti.reps = int.Parse(results[3 + level + (level - 2)]);
                ti.time = int.Parse(results[3 + level + (level - 1)]);
                treatment.Add(ti);

            }
        }
        usedCounter = 0;
        stratRandomGame();
    }

    public void stratRandomGame()
    {
        Debug.Log("STARTING GAME");
        int gameIndex = 0;
        string exerciseName;
        bool gDone, gAvailable;
        if(usedCounter==availableCounter)
        {
            FormManager.instance.goToMyPatients();
        }
        else
        {
            if (debug)
            {
                exerciseName = treatment[gameIndex].name;
                Debug.Log("starting game " + exerciseName);
                minigameDict[exerciseName].container.GetComponent<MinigameOverseer>().StartGame(treatment[gameIndex].time, treatment[gameIndex].reps);
                treatment[gameIndex].done = true;
                usedCounter++;
            }
            else
            {
                do
                {
                    System.Random rnd = new System.Random();
                    gameIndex = rnd.Next(0, treatment.Count);
                    gDone = treatment[gameIndex].done;
                    gAvailable = treatment[gameIndex].available;
                } while (gDone || !gAvailable);

                treatment[gameIndex].done = true;
                exerciseName = treatment[gameIndex].name;
                Debug.Log("starting game " + exerciseName);
                usedCounter++;
                minigameDict[exerciseName].container.GetComponent<MinigameOverseer>().StartGame(treatment[gameIndex].time, treatment[gameIndex].reps);

            }
        }
    }
}
