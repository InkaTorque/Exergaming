using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TherapistStruct
{
    public int id;
    public string first, last;
}

public class FormManager : MonoBehaviour {

    public static FormManager instance;
    public GameObject loginCanvas, myPatientsCanvas, editPacientCanvas, addPacientCanvas;
    public TherapistStruct currentTheraphist;
    public PatientStruct currentSelectedPatient;

    public Dictionary<string, int> pathologyDict;

    private string getTreatmentURL = "http://localhost/exergaming/getTreatment.php";

	// Use this for initialization
	void Start () {
        instance = this; goToLogin();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void disableAllViews()
    {
        loginCanvas.SetActive(false);
        myPatientsCanvas.SetActive(false);
        addPacientCanvas.SetActive(false);
        editPacientCanvas.SetActive(false);
    }

    public void goToLogin()
    {
        loginCanvas.SetActive(true);
        myPatientsCanvas.SetActive(false);
        addPacientCanvas.SetActive(false);
        editPacientCanvas.SetActive(false);
    }

    public void goToMyPatients()
    {
        loginCanvas.SetActive(false);
        myPatientsCanvas.SetActive(true);
        addPacientCanvas.SetActive(false);
        editPacientCanvas.SetActive(false);
    }

    public void goToCreateUser()
    {
        loginCanvas.SetActive(false);
        myPatientsCanvas.SetActive(false);
        addPacientCanvas.SetActive(true);
        editPacientCanvas.SetActive(false);
    }
    public void goToEditUser()
    {
        loginCanvas.SetActive(false);
        myPatientsCanvas.SetActive(false);
        addPacientCanvas.SetActive(false);
        editPacientCanvas.SetActive(true);
    }

    public void InitializeExergaming()
    {
        StartCoroutine(GetTreatment());
    }

    IEnumerator GetTreatment()
    {
        WWWForm treatmentForm = new WWWForm();
        treatmentForm.AddField("patientID", currentSelectedPatient.id);
        WWW treatmentRequest = new WWW(getTreatmentURL,treatmentForm);
        yield return treatmentRequest;
        if(treatmentRequest.text!="ERROR")
        {
            GameManager.instance.LaunchExergame(treatmentRequest.text, pathologyDict);
        }
    }

}
