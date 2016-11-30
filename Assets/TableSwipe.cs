using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableSwipe : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject goodPFBS;

    private bool readyToSpawn, waitingForPlayerInput, initialBarSetupDone, toTheRight, stopMoving, minigameActive;
    private GameObject currentBalloon;

    public GameObject activationBar,cake;
    public RectTransform arrow;
    public float barMovementTime;
    private float startTime, progressTime;

    public float spawnRate;
    private float spawnTimer;
    private List<GameObject> antes;

    public void StartMinigame()
    {
        readyToSpawn = true;
        waitingForPlayerInput = false;
        initialBarSetupDone = false;
        toTheRight = true;
        stopMoving = false;
        minigameActive = true;
        spawnTimer = 0f;
        antes = new List<GameObject>();
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
        if (progressTime >= 0.45 && progressTime <= 0.65)
        {
            foreach(GameObject ant in antes)
            {
                ant.GetComponent<AntMovement>().Eliminate();
            }
            antes.Clear();
            GameManager.instance.Score(true);
        }
        else
        {
            GameManager.instance.Score(false);
        }
    }

    void Update()
    {
        if (minigameActive)
        {
            spawnTimer = spawnTimer + Time.deltaTime;
            if(spawnTimer>=spawnRate)
            {
                Spawn();
                spawnTimer = 0f;
            }
            ShowBar();
        }
    }

    private void Spawn()
    {
        System.Random rnd = new System.Random();
        GameObject go = GameObject.Instantiate(goodPFBS);
        go.transform.position = spawnPoints[rnd.Next(0, spawnPoints.Length)].transform.position;
        go.transform.LookAt(cake.transform);
        go.GetComponent<AntMovement>().target = cake.transform;
        antes.Add(go);
    }

    private void ShowBar()
    {
        if (!stopMoving)
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
}
