using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct PatientItemListStruct
{
    public int id;
    public string name, lastname;
}

public struct PathologyListItem
{
    public int id,level;
    public string name;
}

public class MyPatientsController : MonoBehaviour {

    public GameObject patientItemListPfb,pathologyItemListPfb,patientListContainer,patientDetailPanel,pathologyListContainer;
    public Text patientDetailHeader, welcomeMessage, welcomeMessageShadow;

    private string obtainPatientsQueryPath = "http://localhost/exergaming/getPatientList.php";
    private string obtainPacientPathologyQueryPath = "http://localhost/exergaming/getPacientPathologyList.php";

    private List<PathologyListItem> currentPathologyList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        welcomeMessage.text = "Bienvenido : " + FormManager.instance.currentTheraphist.first + " " + FormManager.instance.currentTheraphist.last;
        welcomeMessageShadow.text = welcomeMessage.text;
        PopulateListOfPatients();
    }

    private void PopulateListOfPatients()
    {
        StartCoroutine(ObtainPatientData());
    }

    IEnumerator ObtainPatientData()
    {
        WWWForm patientListForm = new WWWForm();
        patientListForm.AddField("therapistID", FormManager.instance.currentTheraphist.id);
        WWW patientListRequest = new WWW(obtainPatientsQueryPath, patientListForm);
        yield return patientListRequest;
        string resultString;
        string[] resultData;
        string[] itemData;
        if (patientListRequest.text != "NO PATIENTS")
        {
            foreach (Transform child in patientListContainer.transform)
            {
                Destroy(child.gameObject);
            }
            resultString = patientListRequest.text;
            resultData = resultString.Split(';');
            foreach(string rd in resultData)
            {
                if(rd.Length>2)
                {
                    itemData = rd.Split('|');
                    PatientItemListStruct pil = new PatientItemListStruct();
                    pil.id = int.Parse(itemData[0]);
                    pil.name = itemData[1];
                    pil.lastname = itemData[2];
                    GameObject patientItemList = GameObject.Instantiate(patientItemListPfb);
                    patientItemList.GetComponent<RectTransform>().SetParent(patientListContainer.GetComponent<RectTransform>());
                    patientItemList.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    PatientItemData pid = patientItemList.GetComponent<PatientItemData>();
                    pid.pilStruct = pil;
                    pid.mpc = this;
                    pid.FillInformation();

                }
            }
        }
    }

    public void ShowDetail(PatientItemListStruct pils)
    {
        patientDetailHeader.text = "Patologias de : "+pils.name +" "+pils.lastname;
        FormManager.instance.currentSelectedPatient = pils;
        StartCoroutine(getPathologyListByPacient(pils.id));
    }

    IEnumerator getPathologyListByPacient(int id)
    {
        WWWForm pathologyListForm = new WWWForm();
        pathologyListForm.AddField("patientID", id);
        WWW pathologyListtRequest = new WWW(obtainPacientPathologyQueryPath, pathologyListForm);
        yield return pathologyListtRequest;
        string resultString;
        string[] resultData;
        string[] itemData;
        if (pathologyListtRequest.text != "NO PATOLOGIES")
        {
            foreach (Transform child in pathologyListContainer.transform)
            {
                Destroy(child.gameObject);
            }
            resultString = pathologyListtRequest.text;
            Debug.Log("RESULTADO = " + resultString);
            resultData = resultString.Split(';');
            currentPathologyList = new List<PathologyListItem>();
            currentPathologyList.Clear();
            foreach (string rd in resultData)
            {
                if (rd.Length > 2)
                {
                    Debug.Log("RD = "+rd);
                    itemData = rd.Split('|');
                    PathologyListItem pli = new PathologyListItem();
                    pli.id = int.Parse(itemData[0]);
                    pli.name = itemData[1];
                    pli.level = int.Parse(itemData[2]);
                    GameObject pathologyItemListUI = GameObject.Instantiate(pathologyItemListPfb);
                    pathologyItemListUI.GetComponent<RectTransform>().SetParent(pathologyListContainer.GetComponent<RectTransform>());
                    pathologyItemListUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    PacientPathologyItemData ppid = pathologyItemListUI.GetComponent<PacientPathologyItemData>();
                    ppid.pli = pli;
                    ppid.FillInformation();
                    currentPathologyList.Add(pli);

                }
            }
            patientDetailPanel.SetActive(true);
        }
        else
        {
            Debug.Log("NO HAY PATOLOGIAS REGISTRADAS");
        }
    }
}
