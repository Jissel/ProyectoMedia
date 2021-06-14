using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPersistent : MonoBehaviour {
	private static ControlPersistent instance = null;
	public bool cargarTemp;
    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else if(instance != this){
            Destroy(this.gameObject);
            return;
        }
    }

	// Use this for initialization
	void Start () {
		cargarTemp = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(cargarTemp){
            cargarTemp = false;
            StartCoroutine(SceneLoaded());
        }
	}
	IEnumerator SceneLoaded(){
        //verifica que se cargue la escena
        yield return new WaitUntil(() => SceneManager.GetSceneByName("tablero").isLoaded);
        if(SceneManager.GetSceneByName("tablero").isLoaded){
            Debug.Log("cargado");
        }

        //espera que se cargue todo
        yield return new WaitForSeconds(1);

        GameObject.Find("Code").GetComponent<SaveScript>().LoadBefore("MapasGuardados\\tempGSceBk.xplora2");
    }//fSceneLoaded
}
