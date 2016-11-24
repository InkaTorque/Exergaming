using UnityEngine;
using System.Collections;

public struct TherapistStruct
{
    public int id;
    public string first, last;
}

public class FormManager : MonoBehaviour {

    public static FormManager instance;
    public GameObject loginCanvas, myPatientsCanvas, editPacientCanvas, addPacientCanvas;
    public TherapistStruct currentTheraphist;
    public PatientItemListStruct currentSelectedPatient;

	// Use this for initialization
	void Start () {
        instance = this; goToLogin();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void goToLogin()
    {
        loginCanvas.SetActive(true);
        myPatientsCanvas.SetActive(false);
        addPacientCanvas.SetActive(false);
    }

    public void goToMyPatients()
    {
        loginCanvas.SetActive(false);
        myPatientsCanvas.SetActive(true);
        addPacientCanvas.SetActive(false);
    }

    public void goToCreateUser()
    {
        loginCanvas.SetActive(false);
        myPatientsCanvas.SetActive(false);
        addPacientCanvas.SetActive(true);
    }

}
