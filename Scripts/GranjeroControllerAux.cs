using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;

public class GranjeroControllerAux : MonoBehaviour {
	private Vector3 boardOffset;
    private Vector3 pieceOffset;
	private List<GameObject> granjeros;
	public List<List<Posicion>> caminosGranjeros;
	public bool mostrarCam;
	public bool mostrarEsquinas;
	public bool controlMover;
	public GameObject panelFiltroCamino;
	List<DestinoComponentes>[] destinosCompAux;
	public bool selector;
	public bool permitirBoton;
	GameObject mensaje;
	public GameObject BtnAceptarFiltros,ButtonCerrarCaminos;
	public bool abrirPanel;
	public GameObject go;
	private Piece[,] cacheCaminos;
	Piece [,] arrayCalculo;
	private Menu menu;
	private ControllerBoard tablero;
	private Grid grid;
	public List<CaminoComponente> caminosList;
	//public List<Node> camino;
	public bool llegoAlFinal;
	//int grados;
	
	public PiezaCamino[,] MatrizCaminos = new PiezaCamino[24,24];
	public int[,] MatrizCaminosResultados;

	// Use this for initialization
	void Start () {
		//Offset para la posicion de los componentes
		boardOffset = new Vector3(-12f, 0, -12f);
		pieceOffset = new Vector3(0.5f, 0, 0.5f);
		//lista de granjeros en el mapa
		granjeros = new List<GameObject>();
		//inicializacion de listas de caminos para cada tipo de componente
		caminosGranjeros = new List<List<Posicion>>();
		caminosGranjeros.Add(new List<Posicion>()); //componente 1
		caminosGranjeros.Add(new List<Posicion>()); //componente 2
		caminosGranjeros.Add(new List<Posicion>()); //componente 3
		//valores booleanos para controlar
		mostrarCam = true;
		controlMover = true;
		selector = false;
		permitirBoton = true;
		llegoAlFinal = false;
		//asignar gameobjects
		grid = GameObject.Find("Code").GetComponent<Grid>();
		mensaje = GameObject.Find("MensajePantalla");
		mensaje.SetActive(false);
		panelFiltroCamino = GameObject.Find("PanelFiltroCaminos");
		BtnAceptarFiltros = GameObject.Find("BtnAceptarFiltros");
		ButtonCerrarCaminos = GameObject.Find("ButtonCerrarCaminos");
		menu = GameObject.Find("Code").GetComponent<Menu>();
		tablero = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
		//msjTxt = GameObject.Find("MensajeText").GetComponent<Text>();
		//variables para el tutorial
		abrirPanel = false;
		cacheCaminos = null;
		arrayCalculo = new Piece [24,24];
		//inicializacion de listas de caminos para cada tipo de componente
		caminosList = new List<CaminoComponente>();
		//matrices
		//MatrizCaminos = new PiezaCamino[24,24];
		InicializarMatrizCaminos();
		MatrizCaminosResultados = new int[11,11];
		InicializarMatrizCaminosResultados();

		/*GameObject.Find("BtnCamino").GetComponent<Button>().onClick.AddListener(()=>{
			//si se puede presionar el boton y si hay componentes en el tablero
			if(permitirBoton && tablero.TableroNoEsVacio()){
				if(!selector){
					selector = true;
					//menu.move = false; //no se puedan editar ni mover los componentes
					Debug.Log("Activar caminos");
					//ActivarCaminos();
					StartCoroutine(ColocarGranjeros());
				}else{
					//menu.move = true; //se puedan editar y mover los componentes
					Debug.Log("Desactivar caminos");
					//DesactivarCaminos();
					StartCoroutine(BorrarGranjeros());
					selector = false;
					//StopAllCoroutines();
				}
			}else{
				StartCoroutine(MostrarMensajePantalla("No hay componentes en el mapa"));
			}
		});*/
		//panelFiltroCamino.SetActive(false);
	}

	public CaminoSimple FindPath(int xo, int yo, int xd, int yd, int GrDest){
		CaminoSimple cam = new CaminoSimple();
		List<Node> path = null;
		llegoAlFinal = false;

		Node startNode = grid.grid[xo,yo]; //origen
		Node targetNode = grid.grid[xd,yd]; //destino

		List<Node> openSet = new List<Node>(); //los nodos que faltan
		HashSet<Node> closedSet = new HashSet<Node>(); //los que ya se evaluaron
		openSet.Add(startNode); //añado la posicion inicial

		while (openSet.Count > 0){
			Node currentNode = openSet[0];
			for(int i=1; i<openSet.Count; i++){
				if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost){
					currentNode = openSet[i];
				}
			}//for

			openSet.Remove(currentNode);//eliminar nodo
			closedSet.Add(currentNode);//agregar el nodo como ya usado

			if(currentNode.Equals(targetNode)){
				path = new List<Node>();
				Node actualN = targetNode;

				while(!actualN.Equals(startNode)){
					path.Add(actualN);
					actualN = actualN.parent;
				}
				path.Add(startNode);
				path.Reverse();
				llegoAlFinal = true;
				cam.camino = path;
				return cam;
			}

			int newMvmCtNeb; //new movement cost to neighbour
			foreach (Node neighbour in grid.GetNeighbours(currentNode)){
				if(neighbour.type==-2 || closedSet.Contains(neighbour)){ //no caminable
					//
				}else{
					if(neighbour.type==-1 || neighbour.type==GrDest ){ //si es piso o es mismo tipo que el destino
						newMvmCtNeb = currentNode.gCost + GetDistance(currentNode, neighbour);
						if(newMvmCtNeb < neighbour.gCost || !openSet.Contains(neighbour)){
							neighbour.gCost = newMvmCtNeb;
							neighbour.hCost = GetDistance(neighbour, targetNode);
							neighbour.parent = currentNode;

							if(!openSet.Contains(neighbour)){
								openSet.Add(neighbour);
							}
						}//if
					}
				}//else
			}//foreach
		}//while
		return null;
	}//findpath

	int GetDistance(Node nodeA, Node nodeB){
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		int result = 0;

		//caminos rectos
		if (nodeA.gridX > nodeB.gridX) { //abajo
			result = 14*dstX + 10 * (dstY-dstX);
		}else if(nodeA.gridX < nodeB.gridX){ //arriba
			result = 14*dstX + 10 * (dstY-dstX);
		}else if (nodeA.gridY > nodeB.gridY) { //derecha
			result = 14*dstY + 10*(dstX-dstY);
		}else if(nodeA.gridY < nodeB.gridY){ //izq
			result = 14*dstY + 10*(dstX-dstY);
		}else{ //sino se cumple alguno de los anteriores
			if(dstX > dstY){
				result = 14*dstY + 10*(dstX-dstY);
			}else{
				result = 14*dstX + 10 * (dstY-dstX);
			}
		}
		return result;
	}

	public IEnumerator GenerarYEnviarGranjero(CaminoSimple camino, string tipoGranjero){
		//Genera un granjero en la posicion que se le indica y en envia al destino que se le indica
		Piece p = tablero.pieces[camino.camino.First().gridX,camino.camino.First().gridY];
		//lista
		//granjeros.Add(Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/Caminos/Cube")) as GameObject);
		granjeros.Add(Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/Comp/"+tipoGranjero)) as GameObject);
		//granjeros[granjeros.Count-1].transform.SetParent(p.transform, false);
		granjeros[granjeros.Count-1].transform.position = (Vector3.right * camino.camino.First().gridX) + (Vector3.forward * camino.camino.First().gridY) + boardOffset + pieceOffset;
		//enviar		
		granjeros[granjeros.Count-1].GetComponent<GranjeroController>().Inicializar();
		granjeros[granjeros.Count-1].GetComponent<GranjeroController>().WrapperMove(camino);
		yield break;
	}//fGenerarYEnviarGranjero

	public IEnumerator BorrarGranjeros(){
        //borrar todos los granjeros de todos lados y cuando se suelte la pieza se coloquen todos de nuevo
        GameObject[] listaGranjerosTablero = GameObject.FindGameObjectsWithTag("Granjero");
		for(int i=0;i<listaGranjerosTablero.Length;i++){
			Destroy(listaGranjerosTablero[i]);
		}

		//borrar calle
		GameObject[] listaCalle = GameObject.FindGameObjectsWithTag("calle");
		foreach(GameObject go in listaCalle){
			Destroy(go);
		}
		yield break;
    }//fBorrarGranjeros

    public IEnumerator ColocarGranjeros(){
		//print("colocar grannjero");
		InicializarMatrizCaminos();
		//print("se inicializo matriz");
		//Este metodo coloca todos los granjeros en el mapa
		bool successGrid;
		successGrid = GameObject.Find("Code").GetComponent<Grid>().CreateGrid(); //grid
		permitirBoton = false;
		bool export = false;

		//animacion de espera (granjero caminando)
		go = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/AnimacionCargaCaminos")) as GameObject;
		go.transform.SetParent(GameObject.Find("Canvas").transform, false);
		//envio
		List<Piece> ListaDestino = new List<Piece>();
		CaminoSimple ruta;
		caminosList.Clear();
		foreach(Piece piezaActual in tablero.ComponentesMapa){			
			if(piezaActual.getIDPiece()!="Export"){ //export no envia a nadie, solo recibe
				export = false;
				//busco en la lista
				foreach(DestinoComponentes destino in tablero.destinosComp[piezaActual.getIDNumber()]){
					if(destino.activo){ //si el filtro del destino esta activo
						if(destino.destino != "Export"){
							ListaDestino = tablero.ComponentesMapa.FindAll(pc => pc.getIDPiece() == destino.destino);
							foreach(Piece pc in ListaDestino){
								ruta = FindPath(piezaActual.GetX(0),piezaActual.GetY(0),pc.GetX(0),pc.GetY(0),pc.getIDNumber());
								while(!llegoAlFinal){ //espera que se cree un camino antes de crear el siguiente
									yield return null;
								}
								CreacionRutasPiso(ref ruta);
								StartCoroutine(GenerarYEnviarGranjero(ruta,destino.tipoEnvio));
								
							}
						}else if(destino.destino == "Export" && !export){
							int sumDif = Math.Abs(piezaActual.GetX(0) - 1) + Math.Abs(piezaActual.GetY(0) - 11); //abajo
							int sumDif2 = Math.Abs(piezaActual.GetX(0) - 11) + Math.Abs(piezaActual.GetY(0) - 22); //izq
							int sumDif3 = Math.Abs(piezaActual.GetX(0) - 22) + Math.Abs(piezaActual.GetY(0) - 11); //arriba
							int sumDif4 = Math.Abs(piezaActual.GetX(0) - 11) + Math.Abs(piezaActual.GetY(0) - 1); //der
							int xExp=0, yExp=0;

							int resultado = Math.Min(sumDif,sumDif3);
							resultado = Math.Min(resultado,sumDif2);
							resultado = Math.Min(resultado,sumDif4);
							
							if(resultado == sumDif2){
								//envio a izq
								xExp = 11; 
								yExp = 22;
							}else if(resultado == sumDif4){
								//envio der
								xExp = 11; 
								yExp = 1;
							}else if(resultado == sumDif){
								//envio a abajo
								xExp = 1; 
								yExp = 11;
							}else if(resultado == sumDif3){
								//envio a arriba
								xExp = 22; 
								yExp = 11;
							}
							export=true;
							Piece dest = tablero.ComponentesMapa.Find(pc => pc.getIDPiece() == "Export");
							ruta = FindPath(piezaActual.GetX(0),piezaActual.GetY(0),xExp,yExp,dest.getIDNumber());
							while(!llegoAlFinal){ //espera que se cree un camino antes de crear el siguiente
								yield return null;
							}
							CreacionRutasPiso(ref ruta);
							StartCoroutine(GenerarYEnviarGranjero(ruta,destino.tipoEnvio));
						}//if
					}//if
				}//foreach
			}//if
		}//foreach
		Destroy(go);
		//Debug.Log("Se creo el mapa ");
		permitirBoton = true;
		yield return null;
    }//colocargranjeros

	public void DesactivarCaminos(){
		//Desabilita los caminos
		GameObject[] granjeros = GameObject.FindGameObjectsWithTag("Granjero");
		foreach(GameObject go in granjeros){
			go.GetComponent<LineRenderer>().enabled = false;
			go.transform.localScale = new Vector3(0,0,0);
		}
		//borrar calle
		GameObject[] listaCalle = GameObject.FindGameObjectsWithTag("calle");
		foreach(GameObject go in listaCalle){
			Destroy(go);
		}
		
		System.GC.Collect();
	}//fDesactivarCaminos

	public void ActivarCaminos(){
		//Activa los caminos
		mostrarEsquinas = true;
		GameObject[] granjeros = GameObject.FindGameObjectsWithTag("Granjero");
		foreach(GameObject go in granjeros){
			go.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			go.GetComponent<LineRenderer>().enabled = true;
		}
	}//fActivarCaminos

	public void WrapperCrearCaminos(){
		//Debug.Log("selector "+selector+" compMapa "+tablero.ComponentesMapa.Count);
		if(selector){
			ResetSimulation();
		}
	}

	public void ResetSimulation(){
		//reseteo la simulacion (movimiento, eliminacion componentes)
		StopItAll();
		StartCoroutine(BorrarGranjeros());
		//cacheCaminos = null;
		StartCoroutine(ColocarGranjeros());
	}

	public void FiltrosCaminos(){
		//Filtrar los caminos que se muestran en la simuacion

		//guardo el estado original de la lista
		destinosCompAux = tablero.destinosComp;
		//Muestra el panel
		GameObject.Find("BtnFiltroCaminos").GetComponent<Button>().onClick.AddListener(()=>{
			if(selector){
				StartCoroutine(BorrarGranjeros());
				//DesactivarCaminos();
				panelFiltroCamino.SetActive(true);
				GenerarToggleFiltros();
			}else{
				StartCoroutine(MostrarMensajePantalla("Debes activar la Simulación primero"));
			}
		});
		//aceptar y crear granjeros
		BtnAceptarFiltros.GetComponent<Button>().onClick.AddListener(()=>{
			//selector = true;
			//menu.move = false; //no se puedan editar ni mover los componentes
			BorrarToggleFiltrosCaminos();
			UpdateDestinos();
			//ActivarCaminos();
			StartCoroutine(ColocarGranjeros());
			panelFiltroCamino.SetActive(false);
		});
	}//fFiltrosCaminos
	public void CerrarPanelFiltrosCaminos(){
		//cierra el panel de filtrar
		ButtonCerrarCaminos.GetComponent<Button>().onClick.AddListener(()=>{
			//si se cierra el panel no se aceptan los cambios
			tablero.destinosComp = destinosCompAux;
			if(selector){
				//ActivarCaminos();
				StartCoroutine(ColocarGranjeros());
			}
			BorrarToggleFiltrosCaminos();
			panelFiltroCamino.SetActive(false);
		});
	}//fCerrarPanelFiltrosCaminos

	void GenerarToggleFiltros(){
		//Lee la lista de destinos de cada componente y para cada uno de los destinos que tiene cada lista,
		//instancia toggles para seleccionar (o no), los destinos de cada componente
		int i=0;
		foreach(List<DestinoComponentes> listaDest in tablero.destinosComp){
			foreach(DestinoComponentes destinoC in listaDest){
				GameObject toggle = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/FiltrosG")) as GameObject;
				if(i==0){//granja 1
					toggle.transform.SetParent(GameObject.Find("Granja1Filtro").transform, false);
				}else if(i==1){//granja 2
					toggle.transform.SetParent(GameObject.Find("Granja2Filtro").transform, false);
				}else if(i==2){//granja 3
					toggle.transform.SetParent(GameObject.Find("Granja3Filtro").transform, false);
				}
				toggle.GetComponentInChildren<Toggle>().isOn = destinoC.activo;
				toggle.GetComponentInChildren<Text>().text = destinoC.destino;
			}
			i++;
		}
	}//fGenerarToggleFiltros

	void UpdateDestinos(){
		List<DestinoComponentes>[] destinosCompNuevos = tablero.destinosComp;

		//buscar para cada GranjaFiltro los hijos tag=togglecamino ver cuales estan activos, luego modificar el elemento en la lista
		for(int j=1; j<=3;j++){  //3 es la cantidad de componentes
			for(int i=0;i<GameObject.Find("Granja"+j+"Filtro").transform.childCount;i++){
				if(GameObject.Find("Granja"+j+"Filtro").transform.GetChild(i).tag == "ToggleCamino"){
					//Debug.Log(i+" "+destinosCompNuevos[j-1].Count+" "+(j-1)+"Destino "+GameObject.Find("Granja"+j+"Filtro").transform.GetChild(i).GetComponentInChildren<Text>().text+" activo: "+ GameObject.Find("Granja"+j+"Filtro").transform.GetChild(i).GetComponentInChildren<Toggle>().isOn);
					destinosCompNuevos[j-1].ElementAt(i-1).activo = GameObject.Find("Granja"+j+"Filtro").transform.GetChild(i).GetComponentInChildren<Toggle>().isOn;
					tablero.destinosComp[j-1].ElementAt(i-1).activo = GameObject.Find("Granja"+j+"Filtro").transform.GetChild(i).GetComponentInChildren<Toggle>().isOn;
					Debug.Log("update filtros "+destinosCompNuevos[j-1].ElementAt(i-1).destino+" "+destinosCompNuevos[j-1].ElementAt(i-1).activo);
				}//fif
			}//ffor
		}//ffor
	}//fUpdateDestinos

	void BorrarToggleFiltrosCaminos(){
		GameObject[] listaToggles = GameObject.FindGameObjectsWithTag("ToggleCamino");
		foreach(GameObject go in listaToggles){
			Destroy(go);
		}
	}//fBorrarToggleFiltrosCaminos

	public void StopItAll(){
		StopAllCoroutines();
	}

	
	public void ToastMensaje (string msj){
		//Muestra un mensaje por pantalla, como un Toast de Android (publico)
		StartCoroutine(MostrarMensajePantalla(msj));
	}//fToastMessage

	private IEnumerator MostrarMensajePantalla(string msj){
		//Muestra un mensaje por pantalla, como un Toast de Android (privado)
		mensaje.SetActive(true);
		mensaje.GetComponent<CanvasGroup>().alpha = 1;
		GameObject.Find("MensajeText").GetComponent<Text>().text = msj;
		Debug.Log(msj);

		while(mensaje.GetComponent<CanvasGroup>().alpha > 0){
			yield return new WaitForSeconds (0.1f);
			mensaje.GetComponent<CanvasGroup>().alpha -= 0.05f;
		}

		mensaje.SetActive(false);
		yield break;
	}//fMostrarMensajePantalla

	private void CreacionRutasPiso(ref CaminoSimple ruta){
		Vector3 dif;
		Vector3 auxEsquina = ruta.camino[1].worldPosition - ruta.camino[0].worldPosition;
		//string mensaje ="";
		int grados = 0;
		string dir="";
		int numSpriteCam=0;
		int xCam, yCam;
		for(int i=0;i<ruta.camino.Count;i++){
			/* if(ruta.camino[i].worldPosition != Vector3.zero){
				mensaje += " "+ruta.camino[i].worldPosition.ToString();
			} */
			if(i<ruta.camino.Count-1){
				dif = ruta.camino[i+1].worldPosition - ruta.camino[i].worldPosition;
			}else{
				dif = ruta.camino[i].worldPosition - ruta.camino[i].worldPosition;
			}
			xCam = ruta.camino[i].gridX;
			yCam = ruta.camino[i].gridY;
			
			if(dif.x > 0 && (dif.z == auxEsquina.z)){
				//Debug.Log("de abajo hacia arriba");
				//0
				dir="arr";
				numSpriteCam = 0;
				grados = 90;
			}
			if(dif.x < 0 && (dif.z == auxEsquina.z)){
				//Debug.Log("de arriba hacia abajo");
				//0
				dir="aba";
				numSpriteCam = 0;
				grados = -90;
			}
			if(dif.z > 0 && (dif.x == auxEsquina.x)){
				//Debug.Log("de der a izq");
				//1
				dir="izq";
				numSpriteCam = 1;
				grados = 0;
			}
			if(dif.z < 0 && (dif.x == auxEsquina.x)){
				//Debug.Log("de izq a der");								
				//1
				dir="der";
				numSpriteCam = 1;
				grados = 180;
			}
			//mensaje += dir+" "+numSpriteCam;
			if(dif != auxEsquina){
				//Debug.Log("esquina "+dif.ToString());				
				//Debug.Log("dir: "+dir);
				//dir es la direccion previa
				if((dir=="der" && dif.x>0)||(dir=="aba" && dif.z>0)){
					//3
					//Debug.Log("3");
					numSpriteCam = 3;	
					if(dir=="der"){//si antes iba hacia der ahora va hacia arr
						dir="arr";
						grados = 90;
					}else if(dir=="aba"){//si antes iba hacia aba ahora va hacia izq
						dir="izq";
						grados = 0;
					}
				}
				if((dir=="der" && dif.x<0)||(dir=="arr" && dif.z>0)){
					//5
					//Debug.Log("5");
					numSpriteCam = 5;
					if(dir=="der"){//si antes iba hacia der ahora va hacia aba
						dir="aba";
						grados = -90;
					}else if(dir=="arr"){//si antes iba hacia arr ahora va hacia izq
						dir="izq";
						grados = 0;
					}
				}
				if((dir=="aba" && dif.z<0)||(dir=="izq" && dif.x>0)){
					//4
					//Debug.Log("4");
					numSpriteCam = 4;
					if(dir=="aba"){//si antes iba hacia aba ahora va hacia la derecha
						dir="der";
						grados = 180;
					}else if(dir=="izq"){//si antes iba hacia izq ahora va hacia arr
						dir="arr";
						grados = 90;
					}
				}
				if((dir=="arr" && dif.z<0)||(dir=="izq" && dif.x<0)){
					//6
					//Debug.Log("6");
					numSpriteCam = 6;
					if(dir=="arr"){//si antes iba hacia arriba ahora va hacia la derecha
						dir="der";
						grados = 180;
					}else if(dir=="izq"){//si antes iba hacia izq ahora va hacia aba
						dir="aba";
						grados = -90;
					}
					
				}
				//mensaje += " esquina "+numSpriteCam;
			}
			auxEsquina = dif;
			
			if(tablero.pieces[ruta.camino.Last().gridX,ruta.camino.Last().gridY].getIDPiece() != "Export"){
				//colocar el numero de sprite correcto para cada parte del camino
				if(MatrizCaminos[xCam,yCam].valor == -1){ //si la posicion esta vacia, se coloca una pieza simple
					MatrizCaminos[xCam,yCam].valor = numSpriteCam;
				}else{ //si la posicion esta ocupada se coloca un overlap
					MatrizCaminos[xCam,yCam].valor = MatrizCaminosResultados[MatrizCaminos[xCam,yCam].valor,numSpriteCam];
					//borrar pieza anterior
					Destroy(MatrizCaminos[xCam,yCam].calle);
				}
				if(i!=0 && i<ruta.camino.Count-1){ //no coloca la pieza de inicio del camino
					MatrizCaminos[xCam,yCam].calle = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/Caminos/"+MatrizCaminos[xCam,yCam].valor)) as GameObject;
					MatrizCaminos[xCam,yCam].calle.transform.SetParent(tablero.transform, true);
					MatrizCaminos[xCam,yCam].calle.transform.eulerAngles = Vector3.up * -90;
					MatrizCaminos[xCam,yCam].calle.transform.position = ruta.camino[i].worldPosition + (Vector3.up*0.001f);
				}
			}
			ruta.vectorRotaciones.Add(grados);
			
		}
		ruta.vectorRotaciones.Add(grados); //ultima pos, para evitar error
		//Debug.Log("camino: "+mensaje );
	}

	public void InicializarMatrizCaminos(){
		for(int i = 0; i<24;i++){
			for(int j = 0; j<24;j++){
				MatrizCaminos[i,j]= new PiezaCamino();
			}
		}
	}

	public void InicializarMatrizCaminosResultados(){
		//NOTA CAMBIE LOS NUMEROS DE LAS IMAGENES PARA QUE EMPEZARAN EN 0
		MatrizCaminosResultados.Initialize();
		for(int i=0;i<11;i++){
			for(int j=0;j<11;j++){
				if(i==j){
					MatrizCaminosResultados[i,j] = i;
				}else if(i == 2 || j == 2 || (i==0 && j==1) || (i==0 && j==8) || (i==0 && j==7) || (i==1 && j==9) || (i==1 && j==10) || (i==3 && j==6) || (i==3 && j==8) || (i==3 && j==10) || (i==4 && j==5) || (i==4 && j==8) || (i==4 && j==9) || (i==5 && j==7) || (i==5 && j==10) || (i==6 && j==7) || (i==6 && j==9) || (i==7 && j==8) || (i==7 && j==9) || (i==7 && j==10) || (i==8 && j==9) || (i==8 && j==10) || (i==9 && j==10)){
				//if(i == 2 || j == 2 || ((i==0)&&(j==1||j==8||j==7)) || ((i==1)&&(i==9||i==10)) || ((i==3) && (j==6 || j==8 || j==10)) || ((i==4) && (j==5 || j==8 || j==9)) || ((i==5) && (j==7 || j==10)) || ((i==6) && (j==7 || j==9)) || ((i==7) && (j==8 || j==9 || j==10)) || ((i==8) && (j==9 || j==10)) || (i==9 && j==10)){
					MatrizCaminosResultados[i,j] = 2;
					MatrizCaminosResultados[j,i] = 2;
				}else if((i==1 && j==3) || (i==1 && j==4) || (i==1 && j==7) || (i==3 && j==4) || (i==3 && j==7) || (i==4 && j==7)){
					MatrizCaminosResultados[i,j] = 7;
					MatrizCaminosResultados[j,i] = 7;
				}else if((i==1 && j==5) || (i==1 && j==6) || (i==1 && j==8) || (i==5 && j==6) || (i==5 && j==8) || (i==6 && j==8)){
					MatrizCaminosResultados[i,j] = 8;
					MatrizCaminosResultados[j,i] = 8;
				}else if((i==0 && j==3) || (i==0 && j==5) || (i==3 && j==5) || (i==3 && j==9) || (i==0 && j==9) || (i==5 && j==9)){
					MatrizCaminosResultados[i,j] = 9;
					MatrizCaminosResultados[j,i] = 9;
				}else if((i==4 && j==0) || (i==0 && j==6) || (i==0 && j==10) || (i==4 && j==6) || (i==4 && j==10) || (i==6 && j==10)){
					MatrizCaminosResultados[i,j] = 10;
					MatrizCaminosResultados[j,i] = 10;
				}
			}
		}
	}
}

public class CaminoComponente{
	public int originX { get; set; }
	public int originY { get; set; }
	public int destX { get; set; }
	public int destY { get; set; }
	public string tipoGranjero { get; set; }
	public CaminoSimple ruta { get; set; }

	public CaminoComponente(int ox,int oy,int dx, int dy, string tpG){
		originX=ox;
		originY=oy;
		destX=dx;
		destY=dy;
		tipoGranjero=tpG;
	}
	public CaminoComponente(int ox,int oy,int dx, int dy, string tpG, CaminoSimple cam){
		originX=ox;
		originY=oy;
		destX=dx;
		destY=dy;
		tipoGranjero=tpG;
		ruta = cam;
	}
}

public class PiezaCamino{
	public int valor { get; set; }
	public GameObject calle;

	public PiezaCamino(int val, GameObject go){
		valor = val;
		calle = go;
	}
	public PiezaCamino(int val){
		valor = val;
	}
	public PiezaCamino(){
		valor = -1;
		calle = null;
	}
}

public class CaminoSimple{
	public List<Node> camino;
	public List<int> vectorRotaciones;
	public int contMovmimientos;

	public CaminoSimple(){
		contMovmimientos = 0;
		vectorRotaciones = new List<int>();
	}
}

public class DestinoComponentes{
	public string destino { get; set; }
	public bool activo { get; set; }
	public string tipoEnvio { get; set; } //prefab con el que se va a recorrer el camino

	public DestinoComponentes(string des, bool val){destino=des; activo=val;}
	public DestinoComponentes(string des, bool val, string tipo){destino=des; activo=val; tipoEnvio=tipo;}
}