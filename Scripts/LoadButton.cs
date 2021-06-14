using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadButton : MonoBehaviour {
	GameObject btn;
	public SaveScript sv;
	string path;
	string nombre;
    bool downFile;
    bool fadmin;
    bool online;


	// Use this for initialization
	void Start () {
		btn = gameObject;
		sv = GameObject.Find("Code").GetComponent<SaveScript>();
        downFile = true; //por ahora
        fadmin = false;

		//cuando se pulsa el boton se carga un mapa
		btn.GetComponent<Button>().onClick.AddListener(()=>{
			nombre = btn.GetComponentInChildren<Text>().text;

            //Debug.Log(nombre);

            online = sv.online;

			
            if (nombre.Contains("[*]"))
            {
                path = Application.persistentDataPath+"\\MapasGuardados\\admin\\" + nombre; //Busca mapas protegidos
                fadmin = true;

            }
            else
            {
                if(!online){
                    path = Application.persistentDataPath+"\\MapasGuardados\\" + nombre; 
                }else{
                    path = Application.persistentDataPath+"\\MapasGuardados\\" + GestionBD.single.nombreUsuario + "\\" + nombre; //Busca mapas del usuario
                }
                
                
            }
            path = path.Replace(" [*]", "");
            nombre = nombre.Replace(" [*]", "");
            
            if(Application.internetReachability != NetworkReachability.NotReachable){
                
            }
                   

            //descargar archivo
            /* if(downFile){
                //si hay conexion a internet descargar el archivo que se encuentra en la red
                Debug.Log("Descargar archivo");
                sv.DownloadWrapper(nombre+ ".xplora2",fadmin);
            } *//* else{ //sino usar el local
                sv.LoadBefore(path); //Carga mapas del usuario
            } */
            sv.LoadBefore(path); //Carga mapas del usuario
             //sv.panelLoad.SetActive(false);
             sv.CerrarLoadYBorrarBotones();
        });
	}
}
