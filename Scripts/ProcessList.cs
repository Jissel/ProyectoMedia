using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ProcessList : MonoBehaviour {
	private GameObject panelProc;
	private ProcessDatabase procDB;
	private bool varFiltro, todasFiltro, entradasFiltro, internosFiltro, salidasFiltro;
	private Button btnV, btnTodas, btnEntradas, btnInternos, btnSalidas, btnDescargarPDF, btnFinanzas,btnVentas,btnCostos,btnAba;
	private string tipoF;
	private string tipoSoporte;
	// Use this for initialization

	void Start () {
		//asignaciones
		panelProc = GameObject.Find("PanelProcesos");
		panelProc.SetActive(false);
		procDB = GetComponent<ProcessDatabase>();
		//inicializaciones
		varFiltro = false; 
		todasFiltro = true; 
		entradasFiltro = false; 
		internosFiltro = false;
		salidasFiltro = false;
		tipoF = "todas";
		tipoSoporte = "";
	}

	public void ButtonProcess(){
        panelProc.SetActive(true);
        panelProc.GetComponent<CanvasGroup>().alpha = 1;
		ColocarProcesos(tipoF);
		FiltrosBotones();
	}
	public void ButtonExitProcess(){
		BorrarProcesos();
		panelProc.GetComponent<CanvasGroup>().alpha = 0;
		panelProc.SetActive(false);
	}

	public void SoporteProcess(){
		btnFinanzas = GameObject.Find("Finanzas").GetComponent<Button>();
		btnVentas = GameObject.Find("Ventas").GetComponent<Button>();
		btnCostos = GameObject.Find("Costos").GetComponent<Button>();
		btnAba = GameObject.Find("Abastecimiento").GetComponent<Button>();

		btnFinanzas.onClick.AddListener(()=>{
			tipoSoporte = "finanzas";
			ButtonProcess();
		});
		btnVentas.onClick.AddListener(()=>{
			tipoSoporte = "ventas";
			ButtonProcess();
		});
		btnCostos.onClick.AddListener(()=>{
			tipoSoporte = "costos";
			ButtonProcess();
		});
		btnAba.onClick.AddListener(()=>{
			tipoSoporte = "abastecimiento";
			ButtonProcess();
		});
	}

	private void ColocarProcesos(string tipo){
		//borrar la lista antes de inicializarla
		BorrarProcesos();
		Menu shortcut = this.GetComponentInParent<Menu>();
		int x = shortcut.x;
		int y = shortcut.y;
		
		Piece p = GameObject.Find("Tablero").GetComponent<ControllerBoard>().pieces[x,y];

		//conseguir la lista de procesos segun el tipo
		List<Process> listaFiltrada = new List<Process>();
		listaFiltrada = procDB.ProcDatabase.FindAll(ft =>ft.Id == p.getIDPiece());
		print(x+" "+y+ " "+listaFiltrada[5].Id+" "+p.getIDPiece());

		//si es un componente de soporte
		if(p.getIDPiece() == "Soporte"){
            print("hey");
			listaFiltrada = listaFiltrada.FindAll(ft => ft.Soporte == tipoSoporte);
		}
		
		if(tipo != "todas"){
			listaFiltrada = listaFiltrada.FindAll(ft => ft.Tipo == tipo);
		}
		
		if(!varFiltro){ //si el filtro variable esta desactivado toma solo los que no tienen variable
			listaFiltrada = listaFiltrada.FindAll(ft => ft.Variable == false);
		}

		GameObject procesoListado; //gameobject para instanciar
		int i=1;
		foreach(Process proc in listaFiltrada){
			procesoListado = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/ProcesoPrefab")) as GameObject;
			procesoListado.transform.SetParent(panelProc.transform.GetChild(0), false);
			procesoListado.GetComponentInChildren<Text>().text = " "+i+". "+ proc.Name;
            procesoListado.GetComponent<ProcessPDF>().path = proc.File;
            i++;
		}
	}

	private void BorrarProcesos(){
		GameObject[] listaProc = GameObject.FindGameObjectsWithTag("proceso");
		foreach(GameObject go in listaProc){
			Destroy(go);
		}
	}

	private void FiltrosBotones(){
		//para cambiar los colores de los botones
		Color colorNormal = new Color(100f,174f,99f,255f);
		Color colorClaro = new Color(154f,204f,153f,255f);
		Color colorOscuro = new Color(2f,113f,0f,255f);

		//inicializar botones
		btnV = GameObject.Find("ButtonV").GetComponent<Button>();
		btnTodas = GameObject.Find("ButtonTodas").GetComponent<Button>();
		btnEntradas = GameObject.Find("ButtonEntradas").GetComponent<Button>();
		btnInternos = GameObject.Find("ButtonInternos").GetComponent<Button>();
		btnSalidas = GameObject.Find("ButtonSalidas").GetComponent<Button>();

		//listeners
		DescargarPDF();
		btnV.onClick.AddListener(()=>{
			if(!varFiltro){
				varFiltro=true;
				//btnV.colors.normalColor = colorOscuro;
			}else{
				varFiltro = false;
			}
			ColocarProcesos(tipoF);
		});
		btnTodas.onClick.AddListener(()=>{
			if(!todasFiltro){
				tipoF = "todas";
				todasFiltro=true;
				entradasFiltro = false; 
				internosFiltro = false;
				salidasFiltro = false;
				ColocarProcesos(tipoF);
			}
		});
		btnEntradas.onClick.AddListener(()=>{
			if(!entradasFiltro){
				tipoF = "entrada";
				todasFiltro= false;
				entradasFiltro = true; 
				internosFiltro = false;
				salidasFiltro = false;
				ColocarProcesos(tipoF);
			}
		});
		btnInternos.onClick.AddListener(()=>{
			if(!internosFiltro){
				tipoF = "interno";
				todasFiltro = false;
				entradasFiltro = false; 
				internosFiltro = true;
				salidasFiltro = false;
				ColocarProcesos(tipoF);
			}
		});
		btnSalidas.onClick.AddListener(()=>{
			if(!salidasFiltro){
				tipoF = "salida";
				todasFiltro = false;
				entradasFiltro = false;
				internosFiltro = false;
				salidasFiltro = true;
				ColocarProcesos(tipoF);
			}
		});
	}//ffiltrosbotones
	
	public void DescargarPDF(){
		Menu shortcut = this.GetComponentInParent<Menu>();
		int x = shortcut.x;
		int y = shortcut.y;
		string path ="";
		string folderPath;
		
		Piece p = GameObject.Find("Tablero").GetComponent<ControllerBoard>().pieces[x,y];
		btnDescargarPDF = GameObject.Find("ButtonDPDF").GetComponent<Button>();

		if (p.getIDPiece() == "Huevo Fertil") {
			path = "txt/AVI150-REP-HUE.pdf";
        }
        else if (p.getIDPiece() == "Cria y Levante") {
			path = "txt/AVI150-REP-CYL.pdf";
        }
        else if (p.getIDPiece() == "Planta ABA") {
			path = "txt/ABA150-VAL-ABA.pdf";
        }
        else if (p.getIDPiece() == "Incubadora") {
			path = "txt/AVI150-REP-INC.pdf";
        }
        else if (p.getIDPiece() == "Ponedoras") {
			path = "txt/AVI150-PRO-PON.pdf";
        }
        else if (p.getIDPiece() == "Engorde") {
			path = "txt/AVI150-PRO-ENG.pdf";
        }
        else if (p.getIDPiece() == "Soporte") {
			//aqui falta ventas, finanzas, costos y abastecimiento
			path = "txt/";
        }
        else if (p.getIDPiece() == "Mantenimiento") {
			path = "txt/BAS150-SOP-MAN.pdf";
        }
        else if (p.getIDPiece() == "Beneficio") {
			path = "txt/AVI150-BEN-BEN.pdf";
        }

		if(p.getIDPiece() == "Soporte"){
			if(tipoSoporte == "finanzas"){
				path = "txt/BAS150-SOP-FIN.pdf";
			}else if(tipoSoporte == "ventas"){
				path = "txt/BAS150-SOP-VEN.pdf";
			}else if(tipoSoporte == "costos"){
				path = "txt/BAS150-SOP-COS.pdf";
			}else if(tipoSoporte == "abastecimiento"){
				path = "txt/BAS150-SOP-ABA.pdf";
			}
		}


		folderPath = Application.dataPath+ "/StreamingAssets/" + path;
        
        
        btnDescargarPDF.onClick.AddListener(() => {

            Debug.Log("file:///" + Path.GetFullPath(folderPath));

            Application.OpenURL("file:///"+Path.GetFullPath(folderPath));
            Debug.Log("--> "+ folderPath);

        });
	}
}
