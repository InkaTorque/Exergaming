using UnityEngine;
using System.Collections;

public class VerticalMovement : MonoBehaviour {

    public float initialY, finalY;

    public float movementDuration;

    public bool movementStarted;
    private bool positiveMovement, setUpDone;
    private float startTime,time;

	public void StartMovement()
    {
        movementStarted = true;
        positiveMovement = true;
        setUpDone = false;
    }

    public void EndGame()
    {
        movementStarted = false;
    }

    void Update()
    {
        if(movementStarted)
        {
            Move();
        }
    }

    public void Move()
    {
        if(positiveMovement)
        {
            if(setUpDone)
            {
                time = (Time.time - startTime) / movementDuration;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Lerp(initialY, finalY, time), gameObject.transform.position.z);
                if(time>=1)
                {
                    positiveMovement = false;
                    setUpDone = false;
                }
            }
            else
            {
                startTime = Time.time;
                setUpDone = true;
            }
        }
        else
        {
            if (setUpDone)
            {
                time = (Time.time - startTime) / movementDuration;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Lerp(finalY, initialY, time), gameObject.transform.position.z);
                if(time>=1)
                {
                    positiveMovement = true;
                    setUpDone = false;
                }
            }
            else
            {
                startTime = Time.time;
                setUpDone = true;
            }
        }
    }
}
