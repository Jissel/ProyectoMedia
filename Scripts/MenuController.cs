using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public GameObject scrMain;
	public GameObject scrConfiguracion;
	public GameObject btnIniciar;
	public GameObject btnConfiguracion;
	public GameObject btnSalir;
	public GameObject btnBackConf;
	private CanvasGroup confCanvas;

	// Use this for initialization
	void Start () {
        //inicializar gameobjects
        scrMain = GameObject.Find ("MainScreen");
		scrConfiguracion = GameObject.Find ("ConfiguracionScreen");
		btnIniciar = GameObject.Find ("BtnIniciar");
		btnConfiguracion = GameObject.Find ("BtnConfiguracion");
		btnSalir = GameObject.Find ("BtnSalir");
		btnBackConf = GameObject.Find ("BtnBackConf");

		//ocultar menu
		scrConfiguracion.SetActive(false);
		//visibilidad
		confCanvas = scrConfiguracion.GetComponent<CanvasGroup>();
		confCanvas.alpha = 1; //oculto en la interfaz pero visible cuando se utilice

        //listeners
        botonIniciar();
        botonSalir();
		botonConf ();
		botonBackConf ();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void botonIniciar(){
        //listener para iniciar el xplora
        btnIniciar.GetComponent<Button> ().onClick.AddListener(()=> {
            SceneManager.LoadScene("tablero", LoadSceneMode.Single);
        });
    }

	void botonConf(){
		//listener que desactiva la pantalla anterior y activa la pantalla siguiente
		btnConfiguracion.GetComponent<Button> ().onClick.AddListener (() => {
			scrConfiguracion.SetActive(true);
			scrMain.SetActive(false);
		});
	}

	void botonBackConf(){
		//listener que desactiva la pantalla anterior y activa la pantalla siguiente
		btnBackConf.GetComponent<Button> ().onClick.AddListener (() => {
			scrMain.SetActive(true);
			scrConfiguracion.SetActive(false);
		});
	}

	void botonSalir(){
		//listener que sale del programa
		btnSalir.GetComponent<Button> ().onClick.AddListener (()=>{
			salir();
		});
	}

	void salir(){
		//si esta en el editor, hace pausa, sino sale del programa
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit ();
		#endif
	}
}
