using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SociedadesController : MonoBehaviour {
    public GameObject panel;
    public GameObject btnCerrar;
    public GameObject btnFilSoc;
    public GameObject BtnSocList;
    private ControllerBoard tablero;
    List<Piece> listaOcultados;
    public int cantSociedades;
    private GameObject ContainerFiltros, ContainerList;
    public Dictionary<int, List<string>> Sociedades; //PSB Lista de Sociedades y los componentes que tienen cada uno
    public Dictionary<int, Sociedad> SociedadDict;
    private Text TextPanelSociedad;
    public Dictionary<int, bool> SociedadActiva;
    public string mensajePanelError;


    // Use this for initialization
    void Start () {
        Sociedades = new Dictionary<int, List<string>>(); //PSB inicializo el diccionario de las sociedades
        SociedadDict = new Dictionary<int, Sociedad>();

        SociedadDict.Add(0, new Sociedad(new Color(1f, 1f, 1f, 1.00f), "Sociedad 0"));
        SociedadDict.Add(1, new Sociedad(new Color(0.06666667f, 0.6392157f, 0.8470588f, 1f), "Sociedad 1"));
        SociedadDict.Add(2, new Sociedad(new Color(0.6351443f, 0.2834639f, 0.9245283f, 1f), "Sociedad 2"));
        SociedadDict.Add(3, new Sociedad(new Color(0.9960784f, 0.9490196f, 0.3058824f, 1.00f), "Sociedad 3"));
        SociedadDict.Add(4, new Sociedad(new Color(0, 1f, 0.5382376f, 1f), "Sociedad 4"));
        SociedadDict.Add(5, new Sociedad(new Color(1f, 0.4537815f, 0f, 1f), "Sociedad 5"));

        tablero = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
		panel = GameObject.Find("PanelFiltrosSociedades");
		btnCerrar = GameObject.Find("BtnCerrarPanelFilSoc");
		btnFilSoc = GameObject.Find("BtnFilSoc");
        BtnSocList = GameObject.Find("BtnSocList");
        ContainerList = GameObject.Find("ContainerList");
        TextPanelSociedad = GameObject.Find("TextPanelSociedad").GetComponent<Text>();
		
        //cantidad de sociedades
		cantSociedades = 6;

        IniDicAct();
		
		//crear una lista para saber cuales elementos se han quitado del mapa
		listaOcultados = new List<Piece>();

		//listeners de botones
		Listeners();
		
		//ocultar panel
        ContainerList.GetComponent<CanvasGroup>().alpha = 0;
		ContainerList.SetActive(false);
        panel.GetComponent<CanvasGroup>().alpha = 0;
		panel.SetActive(false);
	}
	private void Listeners(){ //cambiado
        //Boton para activar panel de filtros de sociedades

        //boton para activar panel de lista de sociedades
        BtnSocList.GetComponent<Button>().onClick.AddListener(()=>{            
			panel.SetActive(true);
			panel.GetComponent<CanvasGroup>().alpha = 1;
            ContainerList.SetActive(true);
            ContainerList.GetComponent<CanvasGroup>().alpha = 1;
            //cuando se abre el panel ver si se limpio el tablero y restaurar los togles
            if(GameObject.Find("Inventory").GetComponent<Inventory>().cleared){
                RestoreFilters();
                GameObject.Find("Inventory").GetComponent<Inventory>().cleared = false;
            }
            //titulo
            TextPanelSociedad.text = "Selecciona una sociedad";			
		});
        //boton para cerrar panel
		btnCerrar.GetComponent<Button>().onClick.AddListener(()=>{
            ContainerList.GetComponent<CanvasGroup>().alpha = 0;
            ContainerList.SetActive(false);
			panel.GetComponent<CanvasGroup>().alpha = 0;
            //PSB
            foreach (Transform item in GameObject.Find("ContainerText").transform)
            {
                item.GetComponent<SociedadEditar>().DefaultMode();
            }
            //PSB
            panel.SetActive(false);
            
		});        
		Filtros();
		Sociedad();
	}


    //PSB ===================================================================================================================
    //Operaciones con el diccionario de sociedades
    public bool ValidarElementoSociedad(int sociedad, string idElem) //Valida si un elemento puede ser insertado en el tablero segun la sociedad
    {
        bool addok = false;
        mensajePanelError = "¡Ya existe el componente en esta sociedad!";
        if(SociedadActiva[sociedad] == true){ //si la sociedad seleccionada esta activa //aarb

            if (!Sociedades.ContainsKey(sociedad))
            {
                Sociedades.Add(sociedad, new List<string>());
                Sociedades[sociedad].Add(idElem);
                addok = true;
            }
            else
            {
                //Existe la sociedad
                if (!Sociedades[sociedad].Exists(v => v == idElem)) //No se encuentra el componente en la sociedad
                {
                    Sociedades[sociedad].Add(idElem);
                    addok = true;
                }
            }
        
        }else{
            print("Debes seleccionar una sociedad activa para colocar el componente");
            mensajePanelError = "Debes seleccionar una sociedad activa para colocar el componente";
        }
        return addok;
    }

    public void EliminarComponenteDiccionario(int sociedad, string idElem) //Elimina un componente de la sociedad si es el unico elimina la sociedad
    {//Si sociedad no existe colocar -1
        if (Sociedades.ContainsKey(sociedad))
        {
            if (Sociedades[sociedad].Exists(p => p == idElem))
            {
                Sociedades[sociedad].Remove(idElem);
            }
            if (Sociedades[sociedad].Count == 0)
                Sociedades.Remove(sociedad);
        }
        else
        {
            print("borrando por componente individual");
        }
    }

    public void VaciarDiccionarioComponentes()
    {
        Sociedades.Clear();
    }
    //PSB ===================================================================================================================

    public void Sociedad()
    {
        //aarb
        //inicializar botones

        //indicador de sociedad activa
        GameObject ImgSocActiva = GameObject.Find("ImgSocActiva");

        for (int i = 0; i < cantSociedades; i++)
        { //para cada uno de los botones de las sociedades
            int iterador = i;
            //colocar color al boton
            GameObject.Find("BtnSoc" + iterador).GetComponent<Image>().color = SociedadDict[iterador].GetColor();

            //listener boton seleccion de sociedad
            GameObject.Find("BtnSoc" + iterador).GetComponent<Button>().onClick.AddListener(() => {
                tablero.socGeneral = iterador;
                print(tablero.socGeneral);
                //asignar color de sociedad activa
                BtnSocList.GetComponent<Image>().color = SociedadDict[iterador].GetColor();
                //cerrar panel al seleccionar una sociedad
                ContainerList.GetComponent<CanvasGroup>().alpha = 0;
                ContainerList.SetActive(false);
                panel.GetComponent<CanvasGroup>().alpha = 0;
                panel.SetActive(false);
            });
        }

    }//fSociedad

    public void Filtros(){ 
        //aarb
        //se ejecuta al presionar los "ojos", consigue los elementos de la sociedades y segun el toggle
        //crear una lista con los componentes que se han eliminado y despues que se recoloquen borrarlos de esa lista
        List<Piece> listaP;
        List<Piece> listaAdd;
        for(int i=0; i< cantSociedades; i++){ 
            int iterador = i;
            //inicializar toggles
            GameObject.Find("FiltroSoc"+iterador).GetComponent<Toggle>().isOn = true;

            //listener para detectar si se ha presionado un toggle
            GameObject.Find("FiltroSoc"+iterador).GetComponent<Toggle>().onValueChanged.AddListener(delegate {
                if (!GameObject.Find("FiltroSoc" + iterador).GetComponent<Toggle>().isOn)
                { //si esta desactivado
                  //busco todos los componentes donde la sociedad sea la actual para cada toggle
                    listaP = tablero.ComponentesMapa.FindAll(pc => pc.getSociedad() == iterador);
                    Debug.Log(listaP.Count);
                    //añadir elementos a lista de ocultados
                    listaOcultados.AddRange(listaP);
                    //ocultar elementos
                    foreach (Piece p in listaP)
                    {
                        p.gameObject.SetActive(false);
                    }//fforeach
                    //desactivar boton
                    GameObject.Find("BtnSoc"+iterador).GetComponent<Button>().interactable = false;

                    //colocar sociedad como desactivada
                    SociedadActiva[iterador] = false;
                }
                else
                { //si esta activado
                    int AuxSoc = tablero.socGeneral;
                    listaAdd = listaOcultados.FindAll(pc => pc.getSociedad() == iterador);
                    listaOcultados.RemoveAll(pc => pc.getSociedad() == iterador); //eliminar de la lista
                    tablero.socGeneral = iterador;
                    foreach (Piece p in listaAdd)
                    {
                        p.gameObject.SetActive(true);
                    }//fforeach
                    tablero.socGeneral = AuxSoc;

                    //activar boton
                    GameObject.Find("BtnSoc"+iterador).GetComponent<Button>().interactable = true;

                    //colocar sociedad como activada
                    SociedadActiva[iterador] = true;
                }//felse
            });
            
		}   
        
    }//fFiltros

    public void RestoreFilters(){
        //reinicializa los botones y toggles
		for(int i=0; i< cantSociedades; i++){ //para cada uno de los toggles de las sociedades
            int iterador = i;
			GameObject.Find("FiltroSoc"+iterador).GetComponent<Toggle>().isOn = true;
            GameObject.Find("BtnSoc"+iterador).GetComponent<Button>().interactable = true;
		}

		listaOcultados.Clear();
	}//fRestoreFilters
	
    void IniDicAct(){
        SociedadActiva = new Dictionary<int, bool>();
        for(int i=0;i<cantSociedades;i++){
            SociedadActiva.Add(i,true);
        }
    }
	
}

public class Sociedad
{
    Color color;
    string name;

    public Sociedad(Color col, string nomb)
    {
        color = col;
        name = nomb;
    }
    public void SetColor(Color col)
    {
        color = col;
    }
    public Color GetColor()
    {
        return color;
    }
    public void SetName(string nomb)
    {
        name = nomb;
    }
    public string GetName()
    {
        return name;
    }
}