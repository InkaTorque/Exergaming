using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public struct PatientStruct
{
    public string name, lastname,sex;
    public int age,id;
}

public class AddUserController : MonoBehaviour {

    public InputField nameInput, lastInput, ageInput;
    public Toggle femaleToggle,maleToggle,cifosisToggle,hiperlordosisToggle,escoliosisToggle,coxaVrToggle,coxaVLToggle;
    public Dropdown cifodrop, hiperdrop, escodrop, cvrdrop, cvldrop;
    public GameObject errorPanel;
    public Text errorDescription;
    private bool validForm;
    string errorString = "";

    PatientStruct currentCreatedPatient;

    private string createUserURL = "http://localhost/exergaming/createPatient.php";
    private string linkPatientAndTheraphistURL = "http://localhost/exergaming/assignTherapist.php";
    private string getNewPatientID = "http://localhost/exergaming/getNewPatientID.php";
    private string assignPathologyURL = "http://localhost/exergaming/assignPathologies.php";

    void OnEnable()
    {
        ClearFields();
    }

    public void SaveNewPatient()
    {
        validForm = true;
        errorString = "";
        if(nameInput.text =="")
        {
            Debug.Log("FALTA NOMBRE");
            errorString = errorString + "Nombre , ";
            validForm = false;
        }

        if (lastInput.text == "")
        {
            Debug.Log("FALTA APELLIDO");
            errorString = errorString + "Apellido , ";
            validForm = false;
        }

        if (ageInput.text == "")
        {
            Debug.Log("FALTA EDAD");
            errorString = errorString + "Edad , ";
            validForm = false;
        }

        if (!cifosisToggle.isOn && !hiperlordosisToggle.isOn && !escoliosisToggle.isOn && !coxaVrToggle.isOn && !coxaVLToggle.isOn)
        {
            Debug.Log("FALTA PATOLOGIA");
            errorString = errorString + "Patologias ";
            validForm = false;

        }

        if(validForm)
        {
            StartCoroutine(CretePacientEntry());
        }
        else
        {
            LaunchErrorPanel();
        }
    }

    public void LaunchErrorPanel()
    {
        errorDescription.text = "Los siguientes campos : " + errorString + " deben ser llenados antes de poder registrar un nuevo paciente.";
        errorPanel.SetActive(true);
    }

    public void DisableErrorPanel()
    {
        errorPanel.SetActive(false);
        errorString = "";
    }

    IEnumerator CretePacientEntry()
    {
        WWWForm generalForm = new WWWForm();
        PatientStruct ps = new PatientStruct();
        generalForm.AddField("name", nameInput.text);
        ps.name = nameInput.text;
        generalForm.AddField("lastName", lastInput.text);
        ps.lastname = lastInput.text;
        generalForm.AddField("age", int.Parse(ageInput.text));
        ps.age = int.Parse(ageInput.text);
        if(maleToggle.isOn)
        {
            ps.sex = "M";
            generalForm.AddField("sex", "M");
        }
        else
        {
            ps.sex = "F";
            generalForm.AddField("sex", "F");
        }
        WWW pathologyListtRequest = new WWW(createUserURL, generalForm);
        yield return pathologyListtRequest;
        if(pathologyListtRequest.text!="ERROR")
        {
            Debug.Log("Insertado correctament");
            Debug.Log("ID");
            Debug.Log(pathologyListtRequest.text);
            ps.id = int.Parse(pathologyListtRequest.text);
            currentCreatedPatient = ps;
            WriteTherapist();
        }
        else
        {
            Debug.Log("ERROR CREANDO PACIENTE");
        }
    }

    public void WriteTherapist()
    {
        StartCoroutine(AssignTherapist());
    }

    public void WritePathologies()
    {
        StartCoroutine(AssignPathologies());
    }

    IEnumerator AssignTherapist()
    {
        WWWForm generalForm = new WWWForm();
        generalForm.AddField("therapist", FormManager.instance.currentTheraphist.id);
        generalForm.AddField("patient", currentCreatedPatient.id.ToString());
        WWW assignRequest = new WWW(linkPatientAndTheraphistURL, generalForm);
        yield return assignRequest;
        if(assignRequest.text!="ERROR")
        {
            Debug.Log("ASIGNADO TERAPISTA CORRECTAMENTE");
            WritePathologies();
        }
        else
        {
            Debug.Log("ERROR ASIGNANDO TERAPEUTA");
        }
    }

    IEnumerator AssignPathologies()
    {
        WWWForm generalForm = new WWWForm();
        generalForm.AddField("patientID", currentCreatedPatient.id);
        if(cifosisToggle.isOn)
        {
            generalForm.AddField("cifo", cifodrop.value+1);
        }
        if (hiperlordosisToggle.isOn)
        {
            generalForm.AddField("hyper", hiperdrop.value+1);
        }
        if(escoliosisToggle.isOn)
        {
            generalForm.AddField("esco", escodrop.value+1);
        }
        if(coxaVrToggle.isOn)
        {
            generalForm.AddField("vr", cvrdrop.value+1);
        }
        if(coxaVLToggle.isOn)
        {
            generalForm.AddField("vl", cvldrop.value+1);
        }
        WWW pathoRequest = new WWW(assignPathologyURL, generalForm);
        yield return pathoRequest;
        Debug.Log("RESPONSE DE PATOLOGIA = " + pathoRequest.text);
        if(!pathoRequest.text.Contains("ERROR"))
        {
            Debug.Log("ASIGNADO PATOLOGIAS");
            ClearFields();
            FormManager.instance.goToMyPatients();
        }
        else
        {
            Debug.Log("ERROR LINKEANDO PATOLOGIAS");
        }
    }

    public void ClearFields()
    {
        nameInput.text = "";
        lastInput.text = "";
        ageInput.text = "";
        maleToggle.isOn = true;
        femaleToggle.isOn = false;
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
	
}
