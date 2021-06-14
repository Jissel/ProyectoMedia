using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
using System.Linq;

public class ProcessDatabase : MonoBehaviour {

    public List<Process> ProcDatabase = new List<Process>();
    private JsonData proceso;

    public string type;


	// Use this for initialization
	void Start () {



        #if UNITY_STANDALONE
                    for(int i=0; i<12; i++){
                        proceso = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Proceso"+i+".json", System.Text.Encoding.UTF8));
                        //print(Application.dataPath + "/StreamingAssets/Proceso"+i+".json");
                        if(i<6 || i>9){ //los procesos de soporte son diferentes
                            ConstructProcessDatabase(false);
                        }else{
                            //lee un campo mas en el json, campo de soporte para saber cual tipo de proceso es
                            ConstructProcessDatabase(true);
                        }
                    }

                    /* proceso = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Procesos.json", System.Text.Encoding.Default));
                    ConstructProcessDatabase(); */
           
        #elif UNITY_ANDROID

                            /* TextAsset file = Resources.Load("Procesos") as TextAsset;
                            string jsonString = file.ToString ();
                            proceso = JsonMapper.ToObject(jsonString);
                            ConstructProcessDatabase(); */
           
        #endif


        //Debug.Log("Elemento: "+ProcDatabase[0].Name);
    }

    public Process FetchProcessByName(string name)
    {        
        for (int i = 0; i < proceso.Count; i++)
            if (ProcDatabase[i].Name == name)
                return ProcDatabase[i];
        return null;
    }

    void ConstructProcessDatabase(bool soporte)
    {        
        for (int i = 0; i < proceso.Count; i++)
        {
            int iterador = i;
            if(soporte == false){
                ProcDatabase.Add(new Process( proceso[iterador]["name"].ToString(), proceso[iterador]["tipo"].ToString(), (bool)proceso[iterador]["variable"], proceso[iterador]["file"].ToString(), proceso[iterador]["id"].ToString() 
                ));
            }else if(soporte == true){
                ProcDatabase.Add(new Process( proceso[iterador]["name"].ToString(), proceso[iterador]["tipo"].ToString(), (bool)proceso[iterador]["variable"], proceso[iterador]["file"].ToString(), proceso[iterador]["id"].ToString(),proceso[iterador]["soporte"].ToString()
                ));
            }
            
            //print(iterador+" "+ProcDatabase.Last().Id+" "+(int)proceso[iterador]["id"]);
        }
    }

}

public class Process
{
    public string Name;
    public string Tipo;
    public bool Variable;
    public string File;
    public string Id;
    public string Soporte;

    public Process(string name, string tipo, bool variable, string file, string id)
    {
        this.Name = name;
        this.Tipo = tipo;
        this.Variable = variable;
        this.File = file;
        this.Id = id;
    }
    public Process(string name, string tipo, bool variable, string file, string id, string soporte)
    {
        this.Name = name;
        this.Tipo = tipo;
        this.Variable = variable;
        this.File = file;
        this.Id = id;
        this.Soporte = soporte;
    }


}