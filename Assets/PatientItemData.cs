using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PatientItemData : MonoBehaviour {

    public PatientStruct pilStruct;
    public Text labelTxt,shadowTxt;

    [HideInInspector]
    public MyPatientsController mpc;

    public void FillInformation()
    {
        labelTxt.text = pilStruct.name + " " + pilStruct.lastname;
        shadowTxt.text = labelTxt.text;
    }

    public void ShowPatientDetail()
    {
        mpc.ShowDetail(pilStruct);
    }
}
