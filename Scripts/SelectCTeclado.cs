using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCTeclado : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;
	private bool btnSelected;

	// Use this for initialization
	void Start () {
		btnSelected = false;
	}

	// Update is called once per frame
	void Update () {
		//esto ve el input, por ejemplo el teclado o un control, si se selecciona el eje vertical
		if((Input.GetAxisRaw("Vertical") != 0) && (btnSelected == false)){
			//aqui coloca un objeto para utilizarse con eventos, el primer item seleccionado es el que se coloca aqui
			//por ejemplo el start button
			eventSystem.SetSelectedGameObject (selectedObject);
			btnSelected = true;
		}
	}

	private void OnDisable(){
		btnSelected = false;
	}
}
