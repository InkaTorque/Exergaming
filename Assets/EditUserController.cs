using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditUserController : MonoBehaviour {

    public Text name, last, age, sex;
    public Toggle cifosisToggle, hiperlordosisToggle, escoliosisToggle, coxaVrToggle, coxaVLToggle;
    public Dropdown cifodrop, hiperdrop, escodrop, cvrdrop, cvldrop;

    private bool[] initialPathologyToggle;
    private int[] initialPathologyLevel;

    private string obtainPacientPathologyQueryPath = "http://localhost/exergaming/getPacientPathologyList.php";

    void OnEnable()
    {
        initialPathologyToggle = new bool[5];
        initialPathologyLevel = new int[5];
        FillPanelData();
    }

    private void FillPanelData()
    {
        StartCoroutine(GetPatientInfo());
    }

    IEnumerator GetPatientInfo()
    {
        WWWForm getPatientForm = new WWWForm();
        getPatientForm.AddField("patientID", FormManager.instance.currentSelectedPatient.id);
        WWW getPatientRequest = new WWW(obtainPacientPathologyQueryPath,getPatientForm);
        yield return getPatientRequest;
        Debug.Log("REQUEST = " + getPatientRequest.text);
        if(getPatientRequest.text!="ERROR")
        {
            string patientData = getPatientRequest.text;
            string[] patientRows = patientData.Split(';');
            string str;
            for(int i=0;i<patientRows.Length;i++)
            {
                str = patientRows[i];
                if(str.Length>2)
                {
                    string[] patientColumns = str.Split('|');
                    switch (patientColumns[1])
                    {
                        case "Cifosis":
                            initialPathologyToggle[0] = true;
                            initialPathologyLevel[0] = int.Parse(patientColumns[2]); break;
                        case "Hiperlordosis":
                            initialPathologyToggle[1] = true;
                            initialPathologyLevel[1] = int.Parse(patientColumns[2]); break;
                        case "Escoliosis":
                            initialPathologyToggle[2] = true;
                            initialPathologyLevel[2] = int.Parse(patientColumns[2]); break;
                        case "Coxa Vara":
                            initialPathologyToggle[3] = true;
                            initialPathologyLevel[3] = int.Parse(patientColumns[2]); break;
                        case "Coxa Valga":
                            initialPathologyToggle[4] = true;
                            initialPathologyLevel[4] = int.Parse(patientColumns[2]); break;
                    }

                }
            }
        }
        FillFields();

    }

    private void FillFields()
    {
        name.text = FormManager.instance.currentSelectedPatient.name;
        last.text = FormManager.instance.currentSelectedPatient.lastname;
        age.text = FormManager.instance.currentSelectedPatient.age.ToString();
        sex.text = FormManager.instance.currentSelectedPatient.sex;
        cifosisToggle.isOn = initialPathologyToggle[0];
        hiperlordosisToggle.isOn = initialPathologyToggle[1];
        escoliosisToggle.isOn = initialPathologyToggle[2];
        coxaVrToggle.isOn = initialPathologyToggle[3];
        coxaVLToggle.isOn = initialPathologyToggle[4];
        cifodrop.value = initialPathologyLevel[0]-1;
        hiperdrop.value = initialPathologyLevel[1] - 1;
        escodrop.value = initialPathologyLevel[2] - 1;
        cvrdrop.value = initialPathologyLevel[3] - 1;
        cvldrop.value = initialPathologyLevel[4] - 1;
    }

    public void ClearFields()
    {
        cifosisToggle.isOn = false;
        hiperlordosisToggle.isOn = false;
        escoliosisToggle.isOn = false;
        coxaVLToggle.isOn = false;
        coxaVrToggle.isOn = false;
        cifodrop.value = 0;
        hiperdrop.value = 0;
        escodrop.value = 0;
        cvrdrop.value = 0;
        cvldrop.value = 0;
    }

    public void SaveData()
    {

    }


}
