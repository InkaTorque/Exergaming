using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PacientPathologyItemData : MonoBehaviour {

    public PathologyListItem pli;
    public Text pathoName, pathoLevel;

    public void FillInformation()
    {
        pathoName.text = pli.name;
        pathoLevel.text = pli.level.ToString();
    }
}
