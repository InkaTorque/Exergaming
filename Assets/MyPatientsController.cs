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
    public Text patientDetailHeader, patientDetailHeaderShadow, welcomeMessage, welcomeMessageShadow;

    private string obtainPatientsQueryPath = "http://localhost/exergaming/getPatientList.php";
    private string obtainPacientPathologyQueryPath = "http://localhost/exergaming/getPacientPathologyList.php";

    private Dictionary<string, int> pathologyDict;

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
                    PatientStruct pil = new PatientStruct();
                    pil.id = int.Parse(itemData[0]);
                    pil.name = itemData[1];
                    pil.lastname = itemData[2];
                    pil.sex = itemData[3];
                    pil.age = int.Parse(itemData[4]);
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

    public void ShowDetail(PatientStruct ps)
    {
        patientDetailHeader.text = "Patologias de : " + ps.name + " " + ps.lastname;
        patientDetailHeaderShadow.text = patientDetailHeader.text;
        FormManager.instance.currentSelectedPatient = ps;
        StartCoroutine(getPathologyListByPacient(ps.id));
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
            resultData = resultString.Split(';');
            pathologyDict = new Dictionary<string,int>();
            pathologyDict.Clear();
            foreach (string rd in resultData)
            {
                if (rd.Length > 2)
                {
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
                    pathologyDict.Add(pli.name,pli.level);

                }
            }
            FormManager.instance.pathologyDict = pathologyDict;
            patientDetailPanel.SetActive(true);
        }
        else
        {
            Debug.Log("NO HAY PATOLOGIAS REGISTRADAS");
        }
    }
}
