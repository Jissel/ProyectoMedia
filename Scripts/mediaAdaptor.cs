using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mediaAdaptor : MonoBehaviour {

	public GameObject logo;
	private Vector3 posicion;
	private float posY;
	private float posZ;
    public int ancho = 1024;

	// Use this for initialization
	void Start () {
		Debug.Log("Screen Width : " + Screen.width);
        logo = GameObject.Find ("logo");
		//posicion inicial del objeto
		posicion = logo.transform.localPosition;
		//Debug.Log ("inicial "+ posicion.x);
		//guarda las posiciones iniciales de Y, Z
		posY = posicion.y;
		posZ = posicion.z;

		//asigna la posicion
		logo.transform.localPosition = new Vector3(0f,posY,posZ);

		//ajusta la posicion segun el tamaño de la pantalla
		screenSize ();
	}

	void Update(){
		//screenSize ();
	}
	
	void screenSize(){
		//ajusta los objetos segun el tamaño de la pantalla al estilo de los media queries

		if (Screen.width >= ancho) { //pantallas grandes
			logo.transform.localPosition = new Vector3(-350f,350f,posZ);
		} else if (Screen.width < ancho){ //pantallas pequeñas
			logo.transform.localPosition = new Vector3(0f,350f,posZ);
        }
	}
}
