using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScriptAnimCargaCaminos : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(AnimarTexto());
	}
	
	private IEnumerator AnimarTexto(){
		//esta funcion hace que el texto salga letra por letra infinitamente
		string texto = "Cargando caminos...";
		while(true){
			for(int i=0; i<texto.Length; i++){
				GameObject.Find("CargandoCaminos").GetComponent<Text>().text = texto.Substring(0,i);
				yield return new WaitForSeconds(0.02f);
			}
			yield return null;
		}
	}
}
