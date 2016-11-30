using UnityEngine;
using System.Collections;

public class BaloonMinigame : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] goodPFBS;

    private bool readyToSpawn, waitingForPlayerInput,initialBarSetupDone,toTheRight,stopMoving,minigameActive;
    private GameObject currentBalloon;

    public GameObject activationBar;
    public RectTransform arrow;
    public float barMovementTime;
    private float startTime,progressTime;

	public void StartMinigame()
    {
        readyToSpawn = true;
        waitingForPlayerInput = false;
        initialBarSetupDone = false;
        toTheRight = true;
        stopMoving = false;
        minigameActive = true;
    }

    public void EndMinigame()
    {
        activationBar.SetActive(false);
        minigameActive = false;
    }

    public void GestureDone()
    {
        Evaluate();
    }

    private void Evaluate()
    {
        stopMoving = true;
        if(progressTime>=0.45 && progressTime<=0.65)
        {
            currentBalloon.GetComponent<BaloonMovement>().Eliminate(true);
            GameManager.instance.Score(true);
        }
        else
        {
            currentBalloon.GetComponent<BaloonMovement>().Eliminate(false);
            GameManager.instance.Score(false);
        }
    }

    void Update()
    {
        if(minigameActive)
        {
            if (readyToSpawn)
            {
                SpawnBalloon();
            }
            else
            {
                if (waitingForPlayerInput)
                {
                    ShowBar();
                }
            }
        }
    }

    private void SpawnBalloon()
    {
        System.Random rnd = new System.Random();
        int rndIndex = rnd.Next(0,goodPFBS.Length-1);
        GameObject go = GameObject.Instantiate(goodPFBS[rndIndex]);
        go.transform.position = spawnPoints[rnd.Next(0, spawnPoints.Length)].transform.position;
        go.GetComponent<BaloonMovement>().bm = this;
        readyToSpawn = false;
        currentBalloon = go;
        waitingForPlayerInput = true;
        activationBar.SetActive(true);
    }

    private void ShowBar()
    {
        if(!stopMoving)
        {
            
            if (!initialBarSetupDone)
            {
                activationBar.SetActive(true);
                initialBarSetupDone = true;
                startTime = Time.time;
            }
            else
            {
                if (toTheRight)
                {
                    progressTime = (Time.time - startTime) / barMovementTime;
                    arrow.anchoredPosition = new Vector2(Mathf.Lerp(-500, 500, progressTime), arrow.anchoredPosition.y);
                    if (progressTime >= 1)
                    {
                        toTheRight = false;
                        startTime = Time.time;
                    }
                }
                else
                {
                    progressTime = (Time.time - startTime) / barMovementTime;
                    arrow.anchoredPosition = new Vector2(Mathf.Lerp(500, -500, progressTime), arrow.anchoredPosition.y);
                    if (progressTime >= 1)
                    {
                        toTheRight = true;
                        startTime = Time.time;
                    }
                }
            }

        }

    }

    public void SpawnNextBalloon()
    {
        waitingForPlayerInput = false;
        readyToSpawn = true;
        stopMoving = false;
    }
}
