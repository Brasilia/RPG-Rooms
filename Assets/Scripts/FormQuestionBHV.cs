using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FormQuestionBHV : MonoBehaviour {

    public int value;
    public Toggle[] toggles;
    public Text questionText;
    public Text descriptionText;

    private FormQuestionData questionData;



    void Awake()
    {
        toggles = GetComponentsInChildren<Toggle>().ToArray<Toggle>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeValue(Toggle selected)
    {
        if (!selected.isOn)
        {
            return;
        }
        value = int.Parse(selected.GetComponentInChildren<Text>().text);
        if (selected.isOn)
        {
            foreach (Toggle t in toggles)
            {
                if (t != selected)
                {
                    t.isOn = false;
                }
            }
        }
    }

    public void LoadData(FormQuestionData q)
    {
        questionData = q;
        questionText.text = q.question;
        descriptionText.text = q.description;
    }

}
