using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ProcessPDF : MonoBehaviour {

    GameObject btn;
    public ProcessDatabase procDB;
    public ProcessList proclist;
    public string path;
    string folderPath;

    // Use this for initialization
    void Start() {

        btn = gameObject;

        procDB = GameObject.Find("Code").GetComponent<ProcessDatabase>();
        proclist = GameObject.Find("Code").GetComponent<ProcessList>();
        

        folderPath = Application.dataPath+ "/StreamingAssets/" + path;
        
        
        btn.GetComponent<Button>().onClick.AddListener(() => {

            Debug.Log("file:///" + Path.GetFullPath(folderPath));

            Application.OpenURL("file:///"+Path.GetFullPath(folderPath));
            Debug.Log("--> "+ folderPath);

        });
       
        

    }





}

	


