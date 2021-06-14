using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnIrAVideo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("BtnVideo").GetComponent<Button>().onClick.AddListener(()=>{
			//antes debe guardar el estado actual del tablero para poder recuperarlo luego
			GameObject.Find("Code").GetComponent<SaveScript>().Save("tempGSceBk"); 
			//Va a la scene del video
			SceneManager.LoadScene("video1", LoadSceneMode.Single);
		});
	}
}
