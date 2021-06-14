using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsButton : MonoBehaviour {

	public GameObject panelStats;
	GameObject listaSociedades, listaComponentes;
	public CanvasGroup cv;
	public bool actStats;
	public bool eliminar;
	public bool eliminado;
	int cantSociedades;
	ControllerBoard tablero;
	int sociedadElegida;

	// Use this for initialization
	void Start () {
		actStats = false;
		eliminar = false;
		eliminado = false;
		panelStats = GameObject.Find("PanelStatistics");
		cv = GameObject.Find("PanelStatistics").GetComponent<CanvasGroup>();
		panelStats.SetActive(false);
		tablero = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
		sociedadElegida = 0;
		
		BtnStats();
	}

	void Update(){
		if(eliminado){
			panelStats.SetActive(false);
			eliminado = false;
		}
	}

	public void BtnStats(){
		//muestra el panel de stats
		GameObject.Find("ButtonStatistics").GetComponent<Button>().onClick.AddListener(()=>{
			panelStats.SetActive(true);
			cv.alpha = 1;
			actStats = true;
			GraficarSociedades();
			listenerBtnComp();
			CerrarStats();
			listaSociedades = GameObject.Find("ListaSociedades");
			listaComponentes = GameObject.Find("ListaComponentes");
			listaComponentes.SetActive(false);
		});
	}//fBtnStats

	public void CerrarStats(){
		//cierra el panel y manda a eliminar las estadisticas
		GameObject.Find("ButtonCerrarStats").GetComponent<Button>().onClick.AddListener(()=>{
			listaComponentes.SetActive(true);
			listaSociedades.SetActive(true);
			actStats = false;
			eliminar = true;
			GameObject.Find("Grafico").GetComponent<Statistics>().DestroyGraph();
			StartCoroutine(Clean());
		});
	}//fCerrarStats

	public void GraficarSociedades(){
		GameObject.Find("TituloStats").GetComponent<Text>().text = "Estadísticas por sociedad";
		for (int i = 0; i < GameObject.Find("Code").GetComponent<SociedadesController>().cantSociedades; i++)
        { //para cada uno de los botones de las sociedades
            int iterador = i;
            //colocar color al boton
            GameObject.Find("leyenda" + iterador).GetComponent<Image>().color = GameObject.Find("Code").GetComponent<SociedadesController>().SociedadDict[iterador].GetColor();
			//colocar nombre
			GameObject.Find("cSoc" + iterador).GetComponent<Text>().text = GameObject.Find("Code").GetComponent<SociedadesController>().SociedadDict[iterador].GetName();
			//colocar cantidad
			GameObject.Find("cantGranja" + iterador).GetComponent<Text>().text = tablero.ComponentesMapa.FindAll(pc => pc.getSociedad() == iterador).Count.ToString();
		}
		GameObject.Find("Grafico").GetComponent<Statistics>().Graficar();
	}

	void listenerBtnComp(){
		cantSociedades = GameObject.Find("Code").GetComponent<SociedadesController>().cantSociedades;
		for (int i = 0; i < cantSociedades; i++)
        { //para cada uno de los botones de las sociedades
            int iterador = i;

            //listener boton seleccion de sociedad
            GameObject.Find("btnSociedad" + iterador).GetComponent<Button>().onClick.AddListener(() => {
                sociedadElegida = iterador;
				statsComponentes(sociedadElegida);
            });
        }
	}

	void statsComponentes(int sociedadElegida){
		print("Sociedad elegida "+sociedadElegida);
		if( tablero.ComponentesMapa.FindAll(pc => pc.getSociedad() == sociedadElegida).Count > 0){
			//limpio la grafica
			GameObject.Find("Grafico").GetComponent<Statistics>().DestroyGraph();
			StartCoroutine(Clean());
			//oculto y muestro la otra pantalla
			listaComponentes.SetActive(true);
			listaSociedades.SetActive(false);
			//manda a graficar
			GameObject.Find("Grafico").GetComponent<Statistics>().GraficarSociedad(sociedadElegida);
			//listener de regresar
			GameObject.Find("btnBackSociedades").GetComponent<Button>().onClick.AddListener(()=>{
				//limpio la grafica
				GameObject.Find("Grafico").GetComponent<Statistics>().DestroyGraph();
				StartCoroutine(Clean());
				listaComponentes.SetActive(false);
				listaSociedades.SetActive(true);
				GraficarSociedades();
			});
		}
	}

	IEnumerator Clean(){
		//espera hasta que se terminen de borrar
		yield return new WaitUntil(()=> GameObject.Find("Grafico").transform.childCount == 0);
		eliminado = true;
	}//fClean
}
