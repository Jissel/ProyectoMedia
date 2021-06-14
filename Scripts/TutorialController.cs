using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
	GameObject panelBtn, baseBtn, panelBtnTut, baseBtnTut, btnInv, imagenFondo, btnExitTut;
	GameObject textTitulo, textDescr, flecha, flecha2, btnVideo, btnCam, panelStats, numeros, btnFiltros;
	public Image flechaClon2;
	Image clon2,clon3;
	Menu menuComp;
	bool paso1,paso2,paso3,paso4,paso5,paso6,paso7,paso8,paso9,paso10,paso11,paso12,paso13,paso14,paso15,colocado,colocados3,paso2punto5,controlFiltros;
	public bool tutorial;
	public bool controlNormal;
	int sumaGranjas;

	// Use this for initialization
	void Start () {
		//desabilito todos los botones
		StartCoroutine(DesabilitarBotones());
		//inicializacion
		paso1 = false;
		paso2 = false;
		paso2punto5 = false;
		paso3 = false;
		paso4 = false;
		paso5 = false;
		paso6 = false;
		paso7 = false;
		paso8 = false;
		paso9 = false;
		paso10 = false;
		paso11 = false;
		paso12 = false;
		paso13 = false;
		paso14 = false;
		paso15 = false;
		sumaGranjas = 0;
		colocado = false;
		colocados3 = false;
		controlNormal = false;
		//asignacion de gameobject
		panelStats = GameObject.Find("PanelStatistics");		
		panelBtn = GameObject.Find("PanelButton");
		baseBtn = GameObject.Find("BaseButton");
		btnExitTut = GameObject.Find("BtnExitTut");
		imagenFondo = GameObject.Find("FondoTut");
		panelBtn.SetActive(false);
		baseBtn.SetActive(false);
		panelBtnTut = GameObject.Find("PanelButtonTutorial");
		baseBtnTut = GameObject.Find("BaseButtonTutorial");
		textTitulo = GameObject.Find("TextTitulo");
		textDescr = GameObject.Find("TextDescripcion");
		flecha = GameObject.Find("flecha");
		flecha2 = GameObject.Find("flecha2");
		flecha2.SetActive(false);
		btnInv = GameObject.Find("ButtonInventory");
		menuComp = GameObject.Find("Code").GetComponent<Menu>();
		btnCam = GameObject.Find("BtnCamino");
		btnVideo = GameObject.Find("BtnVideo");
		numeros = GameObject.Find("NumerosTut");
		btnFiltros = GameObject.Find("BtnFiltroCaminos");
		controlFiltros = false;
		numeros.SetActive(false);
		btnVideo.SetActive(false);

		//animacion de salto del boton
		baseBtnTut.GetComponent<Animator>().SetTrigger("salto");
		//Listener del boton del menu
		//Paso 0: hacer click en el boton del menu
		baseBtnTut.GetComponent<Button>().onClick.AddListener(()=>{
			baseBtnTut.GetComponent<Animator>().SetTrigger("click");
			btnInv.GetComponent<Animator>().SetTrigger("tutorial");
			btnInv.GetComponent<Button>().enabled = true;
			paso1 = true;
		});
		btnExitTut.GetComponent<Button>().onClick.AddListener(()=>{
			//Desabilita el tutorial y coloca todo normal
			ModoNormal();
		});

		/* if (GestionBD.single.scoreUsuario == 1)
        {
            tutorial = true;
        }

        GameObject.Find("TxtSession").GetComponent<Text>().text = GestionBD.single.nombreUsuario; */

		if(tutorial){
			btnFiltros.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(tutorial){			
			if(paso1){ //si se completo el primer paso
				//hacer click en el boton inventario
				//Desabilito el boton del menu
				btnCam.SetActive(false);
				baseBtnTut.GetComponent<Button>().enabled = false;
				//cambio los textos y la flecha
				textTitulo.GetComponent<Text>().text = "¡Bien!";
				textDescr.GetComponent<Text>().text = "Ahora vamos a crear unas granjas\nHaz click aquí para abrir el inventario";
				flecha.transform.localEulerAngles = new Vector3(0,0,-25);
				//btnInv.GetComponent<Animator>().SetTrigger("click");
				btnInv.GetComponent<Button>().onClick.AddListener(()=>{
					btnInv.GetComponent<Animator>().SetTrigger("click");
					paso2 = true;					
				});
				paso1= false;
			}else if (paso2){
				//muestra el inventario
				//oculta la imagen
				menuComp.ModificarXY = false;
				btnInv.GetComponent<Button>().enabled = false;
				imagenFondo.SetActive(false);
				textTitulo.SetActive(false);
				flecha.SetActive(false);
				textDescr.transform.localPosition = new Vector3(165f,-127f,0);
				textDescr.GetComponent<Text>().text = "Para crear una granja\nselecciona alguna del Inventario\nY arrástrala al terreno";
				GameObject.Find("ButtonClose").GetComponent<Button>().enabled = false;
				paso2=false;
			}else if(!colocado && (GameObject.Find("Tablero").GetComponent<ControllerBoard>().cantidades[0] > 0 || GameObject.Find("Tablero").GetComponent<ControllerBoard>().cantidades[1] > 0 || GameObject.Find("Tablero").GetComponent<ControllerBoard>().cantidades[2] > 0)){
				//si se ha colocado alguna granja
				//colocar dos granjas
				Mensaje("","¡Excelente!\nArrastra otra granja\npara ahora tener dos\n(o más si deseas)");
				if(sumaGranjas > 1){
					colocado = true;
					paso2punto5 = true;	
				}
				
			}else if(paso2punto5){
				//colocar que se cierre el inventario
				GameObject.Find("ButtonClose").GetComponent<Button>().enabled = true;
				textDescr.GetComponent<Text>().text = "Para avanzar haz click en la X\npara cerrar el panel del\ninventario";
				GameObject.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(()=>{
					if(paso2punto5){
						paso3 = true;
						paso2punto5 = false;
					}
				});
			}else if(paso3){
				//granja colocada
				textTitulo.SetActive(true);
				menuComp.ModificarXY = false;
				textTitulo.GetComponent<Text>().text = "¡Genial!"; 
				textTitulo.transform.localPosition = new Vector3(174f,-38f,0);
				textDescr.GetComponent<Text>().text = "Puedes mover la cámara con\nW - A - S - D y el zoom con\nQ - E";				
				StartCoroutine(MensajeTemporizado("","Ahora haz click sobre una granja\npara mostrar el Menú\nde Componente",4));
				//esperar que se haga click sobre la granja
				StartCoroutine(EsperarClickComp());
				paso3 = false;
			}else if(paso4){
				//menu componentes, boton info
				panelBtnTut.SetActive(false);
				baseBtnTut.SetActive(false);
				//desabilitar los botones del menu de componentes, menos el primero
				StartCoroutine(DesabilitarBotonesComp());
				//flechas
				flecha.SetActive(true);
				flecha.transform.localPosition = new Vector3(-273f,114f,0);
				flecha.transform.localEulerAngles = new Vector3(0,0,-131f);
				//texto
				textDescr.GetComponent<Text>().text = "Haz click aquí\npara ver\ninformación";
				textDescr.transform.localPosition = new Vector3(-341f,29,0);
				//esperar a que se cierre la info
				StartCoroutine(EsperarClickCompInfo());

				paso4 = false;
			}else if(paso5){
				//boton edition
				//activar boton siguiente y desabilitar anterior
				GameObject.Find("Edition").GetComponent<Button>().enabled = true;
				GameObject.Find("Information").GetComponent<Button>().enabled = false;
				//flechas
				flecha.SetActive(true);
				flecha.transform.localPosition = new Vector3(-320f,5f,0);
				flecha.transform.localEulerAngles = new Vector3(0,0,-127f);
				//texto
				textDescr.GetComponent<Text>().text = "Haz click aquí";
				textDescr.transform.localPosition = new Vector3(-341f,-43f,0);				
				//esperar
				StartCoroutine(EsperarClickCompEdition());
				paso5 = false;
			}else if(paso6){
				//boton guardar comp
				GameObject.Find("ButtonEditColor").GetComponent<Button>().enabled = false;
				GameObject.Find("ButtonCerrarEdition").GetComponent<Button>().enabled = false;
				Debug.Log("paso 6");
				Mensaje("","Ingresa un\n nombre y\nguarda");
				textDescr.transform.localPosition = new Vector3(282f,-110f,0);
				flecha.transform.localPosition = new Vector3(126f,-40f,0);
				flecha.transform.localEulerAngles = new Vector3(180,0,32f);
				StartCoroutine(EsperarClickCompGuardar());
				paso6 = false;
			}else if(paso7){
				//boton cambiar color
				Debug.Log("paso 7");
				GameObject.Find("ButtonEditColor").GetComponent<Button>().enabled = true;
				Mensaje("","Ahora cambiemos\nel COLOR");
				textDescr.transform.localPosition = new Vector3(318f,-168f,0);
				flecha.transform.localPosition = new Vector3(213f,-94f,0);
				flecha.transform.localEulerAngles = new Vector3(-180,0,63f);
				GameObject.Find("ButtonCerrarEdition").GetComponent<Button>().enabled = true;
				StartCoroutine(EsperarClickCompColor());				
				paso7 = false;
			}else if(paso8){
				//boton rayos x
				Debug.Log("paso 8");
				//activar boton siguiente y desabilitar anterior
				GameObject.Find("Invisible").GetComponent<Button>().enabled = true;
				GameObject.Find("Edition").GetComponent<Button>().enabled = false;
				//flechas
				flecha.SetActive(true);
				flecha.transform.localPosition = new Vector3(-218f,-188f,0);
				flecha.transform.localEulerAngles = new Vector3(180,0,-173f);
				//texto
				Mensaje("","Para observar\nel interior del\ncomponente");
				textDescr.transform.localPosition = new Vector3(-341f,-112f,0);
				StartCoroutine(EsperarClickCompInvisible());
				paso8 = false;
			}else if(paso9){
				//rotar
				Debug.Log("paso 9");
				//activar boton siguiente y desabilitar anterior
				GameObject.Find("Rotate").GetComponent<Button>().enabled = true;
				GameObject.Find("Invisible").GetComponent<Button>().enabled = false;
				//flechas
				flecha.SetActive(true);
				flecha.transform.localPosition = new Vector3(227f,-169f,0);
				flecha.transform.localEulerAngles = new Vector3(0,0,26f);
				//texto
				Mensaje("","Presiona aquí\npara rotar\nel componente");
				textDescr.transform.localPosition = new Vector3(329f,-92f,0);
				StartCoroutine(EsperarClickCompRotar());
				paso9 = false;
			}else if(paso10){
				//mostrar mensaje de rotacion
				Debug.Log("paso 10");
				//texto
				Mensaje("¡Así se hace!","Puedes rotar\nen cuatro\ndirecciones\nposibles");
				textTitulo.transform.localPosition = new Vector3(332f,30f,0);
				StartCoroutine(OtrasFunciones());
				paso10 = false;
			}else if(paso11){
				//Salir del menu comp
				Debug.Log("paso 11");
				paso11 = false;
				paso12 = true;
			}else if(paso12){
				btnFiltros.SetActive(true);
				//parte de tutorial de los caminos
				Destroy(clon2);
				btnCam.SetActive(true);
				flecha.SetActive(true);
				GameObject.Find("TextDescripcion2").SetActive(false);
				flecha.transform.localPosition = new Vector3(338f,143f,0);
				flecha.transform.localEulerAngles = new Vector3(0,0,240f);
				textDescr.transform.localPosition = new Vector3(304f,55f,0);
				Mensaje("","Haz Click aquí para\nactivar la simulación");
				int btnCamTutVeces = 0;
				//listener boton caminos
				btnCam.GetComponent<Button>().onClick.AddListener(()=>{
					if(btnCamTutVeces == 0){
						paso13 = true;
					}else if(btnCamTutVeces == 1){
						paso14 = true;
					}
					btnCamTutVeces++;
				});

				paso12 = false;
			}else if(paso13){
				//tutorial simulacion
				StartCoroutine(TutSimulacion());
				paso13 = false;
			}else if(paso14){
				//otras funciones
				Debug.Log("otras");
				imagenFondo.SetActive(true);
				//GameObject.Find("TextDescripcion2").SetActive(false);
				panelStats.SetActive(true);
				panelBtn.SetActive(true);
				baseBtn.SetActive(true);
				flecha.SetActive(false);
				StartCoroutine(FuncionesMenu()); 
				paso14 = false;
			}
			
			//suma de cantidad de granjas colocadas en el mapa
			sumaGranjas = (GameObject.Find("Tablero").GetComponent<ControllerBoard>().cantidades[0] + GameObject.Find("Tablero").GetComponent<ControllerBoard>().cantidades[1] + GameObject.Find("Tablero").GetComponent<ControllerBoard>().cantidades[2]);
		}else{
			//si no hay mas pasos del tutorial se vuelve al modo normal
			if(!controlNormal){
				controlNormal = true;
				ModoNormal();
			}
		}
	}//fUpdate

	private IEnumerator DesabilitarBotones(){
		//desabilita los botones del menu principal
		yield return new WaitForSeconds(1);
		for(int i=0;i<panelBtnTut.transform.childCount;i++){
			panelBtnTut.transform.GetChild(i).GetComponent<Button>().enabled = false;
		}
	}//fDesabilitarBotones
	private IEnumerator DesabilitarBotonesComp(){
		//desabilita los botones del menu de componentes
		yield return new WaitForSeconds(1);
		for(int i=2;i<GameObject.Find("ContainerPanel").transform.childCount;i++){
			GameObject.Find("ContainerPanel").transform.GetChild(i).GetComponent<Button>().enabled = false;
		}
		//desabilitar click fuera
		menuComp.ready = true;
	}//DesabilitarBotonesComp
	private IEnumerator MensajeTemporizado(string mensaje1, string mensaje2, int tiempo){
		//muestra un mensaje por cierta cantidad de tiempo
		yield return new WaitForSeconds(tiempo);		
		textTitulo.GetComponent<Text>().text = mensaje1;		
		textDescr.GetComponent<Text>().text = mensaje2;
		menuComp.ModificarXY = true;
	}//fMensajeTemporizado
	private void Mensaje(string mensaje1, string mensaje2){
		//En este metodo se utiliza para ingresar mensajes por pantalla en el tutorial
		textTitulo.GetComponent<Text>().text = mensaje1;		
		textDescr.GetComponent<Text>().text = mensaje2;
	}//fMensaje
	private IEnumerator EsperarClickComp(){
		//esperar click menu componentes
		while(menuComp.ready == true){
			//
			yield return null;
		}
		yield return null;
		paso4 = true;
		//desabilitar cerrar
		menuComp.ready = true;
	}//fEsperarClickComp
	private IEnumerator EsperarClickCompExit(){
		//salir menu componente
		while(menuComp.ready == false){
			//
			yield return null;
		}
		yield return null;
		paso11 = true;
		//paso6 = true;
	}//fEsperarClickCompExit
	private IEnumerator EsperarClickCompGuardar(){
		//boton guardar
		while(menuComp.guardadoTut == false){
			//
			yield return null;
		}
		yield return null;
		paso7 = true;
	}//fEsperarClickCompGuardar
	private IEnumerator EsperarClickCompInfo(){
		//boton info
		while(menuComp.buttonInfo == false){
			//
			yield return null;
		}
		Debug.Log("Abrio info");
		btnExitTut.SetActive(false);
		Mensaje("","Haz click en\nla X para cerrar");
		textDescr.transform.localPosition = new Vector3(315f,150f,0);
		flecha.transform.localPosition = new Vector3(145f,148f,0);
		flecha.transform.localEulerAngles = new Vector3(180,0,-353f);
		while(menuComp.buttonInfo == true){
			//
			yield return null;
		}
		yield return null;
		btnExitTut.SetActive(true);
		Debug.Log("Cerro info");
		paso5 = true;
	}//fEsperarClickCompInfo
	private IEnumerator EsperarClickCompEdition(){
		//boton edicion
		while(menuComp.buttonEdition == false){
			//
			yield return null;
		}
		btnExitTut.SetActive(false);
		yield return null;
		paso6 = true;
	}//fEsperarClickCompEdition
	private IEnumerator EsperarClickCompColor(){
		//boton color
		while(menuComp.buttonColor == false){
			//
			yield return null;
		}
		Mensaje("","Selecciona un\nColor y luego\nhaz click\nen la x");
		textDescr.transform.localPosition = new Vector3(318f,22f,0);
		flecha.SetActive(false);
		while(menuComp.buttonColor == true){
			//
			yield return null;
		}
		yield return null;
		btnExitTut.SetActive(true);
		paso8 = true;
	}//fEsperarClickCompColor
	private IEnumerator EsperarClickCompInvisible(){
		//boton invisibilidad
		while(menuComp.invisible == false){
			//
			yield return null;
		}
		Mensaje("","Pulsa de nuevo\nel boton para\nvolver al\nmodo normal");
		while(menuComp.invisible == true){
			//
			yield return null;
		}
		yield return null;
		paso9 = true;
	}//fEsperarClickCompInvisible
	private IEnumerator EsperarClickCompRotar(){
		//boton rotar
		while(menuComp.cont == 1){
			//
			yield return null;
		}
		yield return null;
		paso10 = true;
	}//fEsperarClickCompRotar
	private IEnumerator OtrasFunciones(){
		//mostrar las otras funciones disponibles
		btnExitTut.SetActive(false);
		yield return new WaitForSeconds(4);
		//flechas
		flecha.SetActive(false);
		Mensaje("","Otras opciones\n son");
		textDescr.transform.localPosition = new Vector3(329f,15f,0);

		yield return new WaitForSeconds(3);
		Mensaje("","");
		GameObject.Find("TextDescripcion2").GetComponent<Text>().text = "Visualizar\n\n\nDestruir\nComponente     ";
		GameObject.Find("TextDescripcion2").GetComponent<Text>().fontSize = 30;
		GameObject.Find("TextDescripcion2").transform.localPosition = new Vector3(347f,96,0);
		
		//otra flecha
		flecha2.SetActive(true);
		flecha2.transform.localPosition = new Vector3(261f,0f,0);
		flecha2.transform.localEulerAngles = new Vector3(0,0,180f);
		clon3 = Instantiate(flechaClon2) as Image;
		clon3.transform.SetParent(transform, false);
		clon3.transform.localPosition = new Vector3(240f,97f,0);
		clon3.transform.localEulerAngles = new Vector3(0,0,180f);

		yield return new WaitForSeconds(4);
		GameObject.Find("TextDescripcion2").GetComponent<Text>().text = "Ahora haz click\naqui para cerrar\no toca\nla granja";
		GameObject.Find("TextDescripcion2").transform.localPosition = new Vector3(347f,201,0);
		clon2 = Instantiate(flechaClon2) as Image;
		clon2.transform.SetParent(transform, false);
		clon2.transform.localPosition = new Vector3(214f,204f,0);
		clon2.transform.localEulerAngles = new Vector3(0,0,180f);
		flecha2.SetActive(false);
		Destroy(clon3);
		//habilitar cerrar menu
		menuComp.ready = false;
		//habilitar todos los botones de nuevo
		for(int i=1;i<GameObject.Find("ContainerPanel").transform.childCount;i++){
			GameObject.Find("ContainerPanel").transform.GetChild(i).GetComponent<Button>().enabled = true;
		}
		btnExitTut.SetActive(true);
		StartCoroutine(EsperarClickCompExit());
	}//fOtrasFunciones

	private IEnumerator TutSimulacion(){
		//tutorial simulacion
		Debug.Log("Simulacion");		
		btnCam.GetComponent<Button>().enabled = false;
		flecha.SetActive(false);
		flecha2.SetActive(false);
		Mensaje("","En el modo simulación \nse observan los caminos \nentre los componentes");
		textDescr.transform.localPosition = new Vector3(278f,55f,0);
		yield return new WaitForSeconds(3.0f);
		flecha.SetActive(true);
		flecha.transform.localPosition = new Vector3(365f,158f,0);
		flecha.transform.localEulerAngles = new Vector3(0,0,-94f);
		Mensaje("","haz click aqui para \nseleccionar y filtrar cuales  \ncaminos seran visibles");
		bool abrirPanel = false;
		
		GameObject.Find("Code").GetComponent<GranjeroControllerAux>().FiltrosCaminos();
		GameObject.Find("Code").GetComponent<GranjeroControllerAux>().CerrarPanelFiltrosCaminos();
		controlFiltros = true;

		GameObject.Find("BtnFiltroCaminos").GetComponent<Button>().onClick.AddListener(()=>{
			abrirPanel = true;
		});
		while(!abrirPanel){
			yield return null;
		}

		flecha.SetActive(false);
		Mensaje("","selecciona los que quieras y haz click \nen aceptar para guardar los cambios \no en la X para cancelarlos");
		textDescr.transform.localPosition = new Vector3(-181f,190f,0);
		bool cerrarPanel = false;
		bool aceptarFiltros = false;

		GameObject.Find("Code").GetComponent<GranjeroControllerAux>().ButtonCerrarCaminos.GetComponent<Button>().onClick.AddListener(()=>{
			cerrarPanel = true;
		});
		GameObject.Find("Code").GetComponent<GranjeroControllerAux>().BtnAceptarFiltros.GetComponent<Button>().onClick.AddListener(()=>{
			aceptarFiltros = true;
		});
		//esperar que el usuario haga click en aceptar o en cerrar
		while(!cerrarPanel && !aceptarFiltros){
			yield return null;
		}
		
		Mensaje("","haz click de nuevo en \nel boton Simulación para \nvolver al modo Edición");
		btnCam.GetComponent<Button>().enabled = true;

		yield break;
	}//fTutSimulacion

	private IEnumerator FuncionesMenu(){
		//mostrar las funciones que tiene el menu
		btnExitTut.SetActive(false);
		Mensaje("","Funciones del menu (¡Pruebalas!):\n1)Rotar tablero - 2)Abrir Inventario -3)Guardar Mapa\n4)Mapa Nuevo - 5)Cargar Mapa - 6)Rayos X Componentes\n7)Componentes Visibles - 8)Estadísticas\n9)Volver al Menú inicial");
		textDescr.transform.localPosition = new Vector3(0f,-117f,0);
		numeros.SetActive(true);
		yield return new WaitForSeconds(10);
		Mensaje("¡Ya estas list@!","¡Haz terminado el tutorial, Empieza a XPLORAr!");
		textTitulo.transform.localPosition = new Vector3(0f,-50f,0);
		imagenFondo.SetActive(false);
		yield return new WaitForSeconds(3);
		Mensaje("","");
		numeros.SetActive(false);
		btnCam.SetActive(true);
		btnVideo.SetActive(true);
	}//fFuncionesMenu
	private void ModoNormal(){
		//Coloca todo en el modo normal, es decir, en modo no tutorial
		controlNormal = true;
		Destroy(clon2);
		tutorial = false;
		
		btnFiltros.SetActive(true);
		if(!controlFiltros){
			GameObject.Find("Code").GetComponent<GranjeroControllerAux>().FiltrosCaminos();
			GameObject.Find("Code").GetComponent<GranjeroControllerAux>().CerrarPanelFiltrosCaminos();
		}
		panelBtnTut.SetActive(false);
		baseBtnTut.SetActive(false);
		panelStats.SetActive(true);
		panelBtn.SetActive(true);
		baseBtn.SetActive(true);
		baseBtn.GetComponent<Animator>().SetTrigger("click");
		imagenFondo.SetActive(false);
		flecha.SetActive(false);
		textDescr.SetActive(false);
		GameObject.Find("TextDescripcion2").SetActive(false);
		textTitulo.SetActive(false);
		numeros.SetActive(false);
		btnCam.SetActive(true);
		btnVideo.SetActive(true);
		GameObject.Find("Code").GetComponent<Menu>().ModificarXY = true;
		//menucomp
		if(GameObject.Find("ContainerPanel")){				
			for(int i=1;i<GameObject.Find("ContainerPanel").transform.childCount;i++){
				GameObject.Find("ContainerPanel").transform.GetChild(i).GetComponent<Button>().enabled = true;
			}
			GameObject.Find("ContainerPanel").SetActive(false);
			GameObject.Find("Code").GetComponent<Menu>().ready = true;
			GameObject.Find("Code").GetComponent<Menu>().x = -1;
			GameObject.Find("Code").GetComponent<Menu>().y = -1;
			GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
		}
		btnExitTut.SetActive(false);
	}//fModoNormal
}
