using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModoOffline : MonoBehaviour {
	private Button btnSave;

	// Use this for initialization
	void Start () {
		//Debug.Log("Modo offline "+GestionBD.offline);

		//desabilitar botones y funciones
		if(GestionBD.offline){
			//StartCoroutine(Desabilitar());
		}
	}

	private IEnumerator Desabilitar(){
		yield return new WaitForSeconds(1f);
		btnSave = GameObject.Find("ButtonSave").GetComponent<Button>();
		btnSave.interactable = false;
		/* GameObject.Find("ButtonLoad").GetComponent<Button>().interactable = false;
		GameObject.Find("BtnVideo").GetComponent<Button>().interactable = false; */
		yield break;
	}
	

}
