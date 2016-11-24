using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginController : MonoBehaviour {

    public InputField usernameTxt,passwordTxt;
    public GameObject errorPanel;
    public Text errorText;
    private string username, password;

    private string loginURL = "http://localhost/exergaming/login.php";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Login()
    {
        username = usernameTxt.text;
        password = passwordTxt.text;
        StartCoroutine(sendLogin(username,password));
    }

    IEnumerator sendLogin(string username , string password)
    {
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("usernamePost", username);
        WWW loginRequest = new WWW(loginURL,loginForm);
        yield return loginRequest;
        string resultString;
        string[] resultData;
        if(loginRequest.text ==  "NO USER")
        {
            errorText.text = "DICHO USUARIO NO SE ENCUENTRA REGISTRADO";
            errorPanel.SetActive(true);
        }
        else 
        {
            resultString = loginRequest.text;
            resultData = resultString.Split('|');
            if (resultData[1] != password)
            {
                errorText.text = "EL USUARIO Y CONTRASEÑA NO CONCUERDAN";
                errorPanel.SetActive(true);
            }
            else
            {
                FormManager.instance.currentTheraphist.id = int.Parse(resultData[0]);
                FormManager.instance.currentTheraphist.first = resultData[2];
                FormManager.instance.currentTheraphist.last = resultData[3];
                FormManager.instance.goToMyPatients();
            }
        }
    }
    
    public void DisableErrorPanel()
    {
        errorPanel.SetActive(false);
    }
}
