using UnityEngine;
using System.Collections;

public class Shotz : MonoBehaviour {

    public Vector2[] spawnPoints;
    public GameObject goodPFBS,bullseye,smokePFB,bullet;
    public float threshold;
    public Transform spawnPoint;

    private bool readyToSpawn, waitingForPlayerInput, initialBarSetupDone, toTheRight, stopMoving, minigameActive;

    private GameObject currentTarget;

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
        minigameActive = false;
    }

    public void GestureDone()
    {
        SpawnBullet();
        Evaluate();
    }

    private void Evaluate()
    {
        if ((bullseye.transform.position.y <= currentTarget.transform.position.y + threshold || bullseye.transform.position.y>= currentTarget.transform.position.y - threshold) && 
            (bullseye.transform.position.z <= currentTarget.transform.position.z + threshold || bullseye.transform.position.z>= currentTarget.transform.position.z - threshold))
        {
            GameManager.instance.Score(true);
        }
        else
        {
            GameManager.instance.Score(false);
        }
    }

    void Update()
    {
      
    }

    private void SpawnBullet()
    {
        GameObject go = GameObject.Instantiate(bullet);
        go.transform.position = spawnPoint.transform.position;
        GameObject.Instantiate(smokePFB);
    }

    private void SpawnTarget()
    {
        GameObject go = GameObject.Instantiate(goodPFBS);
        System.Random rnd = new System.Random();
        int gameIndex = rnd.Next(0, spawnPoints.Length-1);
        go.transform.position = new Vector3(963.45f, spawnPoints[gameIndex].y, spawnPoints[gameIndex].x);
        currentTarget = go;
        GameObject.Instantiate(smokePFB);
    }
}
