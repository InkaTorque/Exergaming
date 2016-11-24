using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PacientPathologyItemData : MonoBehaviour {

    public PathologyListItem pli;
    public Text pathoName, pathoLevel,nameShadow,levelShadow;

    public void FillInformation()
    {
        pathoName.text = pli.name;
        pathoLevel.text = pli.level.ToString();
        nameShadow.text = pathoName.text;
        levelShadow.text = pathoLevel.text;
    }
}
