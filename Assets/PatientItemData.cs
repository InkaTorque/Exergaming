using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PatientItemData : MonoBehaviour {

    public PatientItemListStruct pilStruct;
    public Text labelTxt;

    [HideInInspector]
    public MyPatientsController mpc;

    public void FillInformation()
    {
        labelTxt.text = pilStruct.name + " " + pilStruct.lastname;
    }

    public void ShowPatientDetail()
    {
        mpc.ShowDetail(pilStruct);
    }
}
