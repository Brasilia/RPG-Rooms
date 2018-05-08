using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Form : MonoBehaviour {

    public void GoToForm()
    {
        //TODO add correct link
        Application.OpenURL("http://unity3d.com/");
    }
    public void Continue()
    {
        GameManager gm = GameManager.instance;
        gm.LoadNewBatch();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
