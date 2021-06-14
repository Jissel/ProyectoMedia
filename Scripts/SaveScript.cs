using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using LitJson;


public class SaveScript : MonoBehaviour {
    public GameObject tablero;
    Piece[,] cuadricula;
    GameObject fieldS;
    string textInput;
    public GameObject btnSave;
    Text textStatus;
    CanvasGroup showPanel;
    CanvasGroup showPanelLoad;
    GameObject panelSave;
    GameObject panelSaveMap;
    GameObject btnSaveExit;
    public GameObject panelLoad;
    bool proteccionLectura;
    bool sobreescribir;
    public GameObject btnPrefab;
    GameObject btnLoadExit;
    //   GameObject btnLoadOtro;
    bool loadCreados;
    int archivosTam;
    Inventory inventario;
    private Menu menu;
    public EventSystem eventS;
    bool loadVentana;
    public ControllerBoard ControllerBoard;
    bool tempCreado;
    public bool loading;


    public bool flagLoaded = false; //Controla si se cargó un mapa
    public bool flagOpened = false; //Controla si un mapa está abierto

    string mapName;


    public bool online;
    public const string onlinePath = "/MapasGuardados/GestionBD.single.nombreUsuario/";    
    public const string offlinePath = "/MapasGuardados/";

    public bool loadFlag;
    

 
    // Use this for initialization
    void Start() {
        tablero = GameObject.Find("Tablero");
        cuadricula = tablero.GetComponent<ControllerBoard>().pieces;
        fieldS = GameObject.Find("InputSave");
        btnSave = GameObject.Find("ButtonSaveGuardar");
        textStatus = GameObject.Find("TextStatus").GetComponent<Text>();
        showPanel = GameObject.Find("PanelGuardar").GetComponent<CanvasGroup>();
        showPanelLoad = GameObject.Find("PanelCargar").GetComponent<CanvasGroup>();
        btnLoadExit = GameObject.Find("ButtonCerrarCargar");
        panelSave = GameObject.Find("PanelGuardar");
        panelSaveMap = GameObject.Find("PanelSaveMap");
        btnSaveExit = GameObject.Find("ButtonCerrar");
        panelLoad = GameObject.Find("PanelCargar");
        //      btnLoadOtro = GameObject.Find("BtnCargarOtra");
        inventario = GameObject.Find("Inventory").GetComponent<Inventory>();
        menu = GameObject.Find("Code").GetComponent<Menu>();
        ControllerBoard = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
        proteccionLectura = false;
        sobreescribir = false;
        loadCreados = false;
        loadVentana = false;
        tempCreado = false;
        loading = false;
        archivosTam = 0;
        loadFlag = false;

        if(GestionBD.offline){

           online = false;
        }else{

            online = true;
        }

       
        //listeners de botones
        PanelWrapper();

        //ocultar panel de guardar y cargar
        panelSave.SetActive(false);
        panelLoad.SetActive(false);
        panelSaveMap.SetActive(false);

        //listener boton cierre
        CerrarCargar();
    }//fStart

    public void ChangeFlag()
    {
        flagLoaded = false;
        flagOpened = false;
        
    }//fChangeFlag

    public void ShowSaveMap()
    {     
        

        if (flagLoaded) //Si se cargó un mapa
        {
            //Debug.Log("SE CARGÓ UN MAPA");
            //flagLoaded = false;

            /*File.Exists("MapasGuardados\\admin\\" + mapName + ".xplora2")*/
            if (EsProtegido(mapName + ".xplora2"))
            {
                ShowSave();               

            }else if(!EsProtegido(mapName + ".xplora2"))
            {
                panelSaveMap.SetActive(true);
            }

        }
        else
        {
            if (flagOpened) {
                panelSaveMap.SetActive(true);

            }else{
                ShowSave();
            }                         

        }
      
        
    }//fShowSaveMap


    public void ShowSave()
    {
        panelSaveMap.SetActive(false);
        panelSave.SetActive(true);
        showPanel.alpha = 1;
        //vacia el texto status
        StatusText("", Color.white);
        //colocar input del save directo al teclado
        eventS.SetSelectedGameObject(fieldS);
        //desabilitar movimiento camara
        GameObject.Find("Camera").GetComponent<CameraController>().mover = false;
    }//fShowSave
    
    private void SaveField() {
        //toma el input del save
        btnSave.GetComponent<Button>().onClick.AddListener(() => {

            flagOpened = true;
                      
            if (sobreescribir == false) {
                textInput = fieldS.GetComponent<InputField>().text; //toma el string al pulsar el boton
                if (String.IsNullOrEmpty(textInput)) { //valida si el campo está vacio
                    Debug.Log("Campo vacio");
                    //notificacion de error
                    StatusText("El campo esta vacio", Color.red);
                } else { //esta lleno
                    ValidarNombre(textInput); //valida si el nombre de archivo es valido
                }
            } else {
                //sobreescribir

                if(online){
                    StartCoroutine(DeleteFile(textInput)); // Inicia corutina para borrar archivo en la BD 
                }
                          
                Save(textInput);
                sobreescribir = false;
            }
        });
    }//fSaveField
    
    public void CerrarSaveMaps()
    {
        GameObject.Find("ButtonCerrarSave").GetComponent<Button>().onClick.AddListener(() => {
            //flagLoaded = false;
            //reactivar movimiento camara
            GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
            //oculta el panel de guardar
            panelSaveMap.SetActive(false);
        });
    }//fCerrarSaveMaps
    
    protected void CerrarVentana() {
        btnSaveExit.GetComponent<Button>().onClick.AddListener(() => {
            //reactivar movimiento camara
            GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
            //oculta el panel de guardar
            panelSave.SetActive(false);
            //coloca de nuevo el campo en vacio
            fieldS.GetComponent<InputField>().text = "";
        });
    }//fCerrarVentana

    protected void PanelWrapper() {
        //listeners
        //Input
        SaveField();
        //btn cerrar
        CerrarVentana();

        //btn cerrar Guardar Mapas
        CerrarSaveMaps();

    }//fPanelWrapper
    
    public void ReplaceMap(){

        Debug.Log("Ruta del Mapa: " + mapName);

        panelSaveMap.SetActive(false);

        string ruta;

 
        if(!online){

            ruta = Application.persistentDataPath+offlinePath;             
            Debug.Log(ruta);       
            mapName = mapName.Replace(Application.persistentDataPath+offlinePath, "");

		}else{

            ruta = Application.persistentDataPath+onlinePath;         
            mapName = mapName.Replace(Application.persistentDataPath+onlinePath, "");

        }

      
        Debug.Log("Nombre del mapa a reemplazar = " + mapName);
               
           if (File.Exists(ruta + mapName + ".xplora2"))
           {  //si el archivo ya existe
              //sobreescribir       

               StartCoroutine(DeleteFile(mapName)); // Inicia corutina para borrar archivo en la BD                         
               sobreescribir = true;
               Save(mapName);

           }else{  

               ShowSave();
           }
       
    }//fReplaceMap
    
    
    private void ValidarNombre(string name)
    {
        string ruta;

        if(!online){

             ruta = Application.persistentDataPath+offlinePath;

        }else{

            ruta = Application.persistentDataPath+onlinePath;

        }


        if (File.Exists(ruta + name + ".xplora2"))
        { //si el archivo ya existe
          //sobreescribir

            
            if (EsProtegido(ruta + name + ".xplora2"))
            {
                StatusText("El archivo esta protegido, Introduzca otro nombre", Color.yellow);
            }
            else
            {
                //si se puede
                StatusText("Pulsa de nuevo guardar para sobreescribir", Color.yellow);
                sobreescribir = true;
            }

            StartCoroutine(DeleteFile(name)); // Inicia corutina para borrar archivo en la BD  

        }
        else
        { //no hay problema
            Save(name);
        }
    }//fvalidarNombre
    

    private bool EsProtegido(string fileName)
    {
        if (File.Exists(fileName) )
        {
            StreamReader entrada = new StreamReader(fileName);
            return entrada.Peek() == '*';
        }

        return false;
       
    }//fEsProtegido


    private void StatusText(string texto, Color color) {
        //Modifica el texto de status
        textStatus.color = color;
        textStatus.text = texto;
    }//fStatusText

    public void Save(string fileName) {
        SavedFile archivo; //clase para guardar
        Debug.Log("Save "+fileName);
        //crea una carpeta para guardar archivos
        DirectoryInfo ruta = Directory.CreateDirectory(Application.persistentDataPath+"/MapasGuardados");

        mapName = fileName;

        string subfolder;

              
        //Verifica si está online y asigna ruta
        if(online){
            subfolder = Application.persistentDataPath+onlinePath; //Guarda ruta con nombre del usuario
        
        }else{
            subfolder = Application.persistentDataPath+offlinePath;
        }

        Directory.CreateDirectory(subfolder); //Crea carpeta con el nombre del usuario

     
        String linea;
        String json = "[";
        

        bool forzarGuardado = false;
        while (!forzarGuardado) {
            try {
                forzarGuardado = true;

                if(online){
                     StartCoroutine(AddFile(fileName)); // Inicia corutina para guardar archivo       
                }
                                

                for (int x = 0; x < 24; x++) {
                    for (int y = 0; y < 24; y++) {
                        if (cuadricula[x, y] != null && !(cuadricula[x, y].name == "Slave1(Clone)" || cuadricula[x, y].name == "Slave2(Clone)" || cuadricula[x, y].name == "Salida(Clone)")) {
                            archivo = new SavedFile(cuadricula[x, y].getIDPiece(),x,y,cuadricula[x, y].getName(),cuadricula[x, y].getSociedad(),cuadricula[x, y].getTotalComp(),cuadricula[x, y].totalComp);
                            linea = JsonUtility.ToJson(archivo,true);
                            json = json+linea+",";
                        }//fif
                    }//ffor
                }//ffor

                //remover ultimo char que es una ,
                json = json.Remove(json.Length-1);

                if(json.Length>0){
                    //Agregar ultimo ]
                    json = json + "]";
                }
            
                //visualizar json creado
                Debug.Log(json);

             
            
                File.WriteAllText(subfolder + fileName + ".xplora2",json);   
                
                
                //coloca de nuevo el campo en vacio
                fieldS.GetComponent<InputField>().text = "";
                Debug.Log("Archivo guardado");
                StatusText("Archivo guardado con exito", Color.green);

                if(online){

                     //subir el archivo al servidor
                    GameObject.Find("Code").GetComponent<LevelUploader>().StartUpload(fileName);

                }
               
            }
            catch (Exception) {
                //
            }
        }
     }//fSave

    //Carga los datos del archivo desde la BD
    //Se llama con el botón ButtonLoad en tablero
    public void CargarDatosArchivo()
    {
        string subfolder;

        //Verifica si está online y asigna ruta
        if(!online){

            subfolder = Application.persistentDataPath+offlinePath; //Guarda ruta con nombre del usuario
            
        }else{

            subfolder = Application.persistentDataPath+onlinePath; //Guarda ruta con nombre del usuario
        }

        Directory.CreateDirectory(subfolder); //Crea carpeta con el nombre del usuario si no existe
        
        //carga directa sin bd
        if(!online){
            print("carga sin bd");
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath+"\\MapasGuardados");
            FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            string[] archivos = files.Select(f => f.Name).ToArray();
            Cargar(archivos);
        }else{ //carga normal
            StartCoroutine(FileData());
        }

    }//fCargarDatosArchivo

    public void Cargar(string[] archivos) {
        //Esta funcion se llama desde el boton
        panelLoad.SetActive(true);
        showPanelLoad.alpha = 1;
        ControllerBoard.cantidades[0] = 0;
        ControllerBoard.cantidades[1] = 0;
        ControllerBoard.cantidades[2] = 0;


        DirectoryInfo infofolder = new DirectoryInfo(Application.persistentDataPath+"\\MapasGuardados");

        //cargar desde otra ubicacion
        DirectoryInfo adm = Directory.CreateDirectory(infofolder + "\\" + "admin"); //aarb: se crea la carpeta directamente (si no existe), para no tener que crearla a mano

        //Consigue los nombres de los archivos guardados
        if (archivos != null) { 
            Debug.Log(archivos);

             if(online){
 
                DirectoryInfo info = new DirectoryInfo(infofolder + "\\" + GestionBD.single.nombreUsuario);            
             }


            string aux;
            GameObject test;

            if(!online){
                archivosTam = archivos.Length;
            }else{
                archivosTam = archivos.Length-1;
            }
            
            for (int i = 0; i < archivosTam; i++) {
                aux = archivos[i].Substring(archivos[i].LastIndexOf('\\') + 1);
        
                test = Instantiate(btnPrefab) as GameObject;
                test.transform.SetParent(panelLoad.transform.GetChild(0), false);
                test.GetComponentInChildren<Text>().text = aux;
            }
            
            
        }

    }//fCargar


    public void Load(string fileName)
    {
        Debug.Log("Load "+fileName);
        Piece p;
        //carga el archivo donde estan guardados los datos
        List<SavedFile> archivoCargado = new List<SavedFile>();
        JsonData mapaArchivoJson;

        //leer datos de archivo en formato json y convertir a lista de objetos
        #if UNITY_STANDALONE
            
            mapaArchivoJson = JsonMapper.ToObject(File.ReadAllText(fileName));
            for (int i = 0; i < mapaArchivoJson.Count; i++)
            {
                archivoCargado.Add(new SavedFile( mapaArchivoJson[i]["id"].ToString(),  (int)mapaArchivoJson[i]["x"], (int)mapaArchivoJson[i]["y"], mapaArchivoJson[i]["name"].ToString(), (int)mapaArchivoJson[i]["sociedad"], (int)mapaArchivoJson[i]["comp"],(int)mapaArchivoJson[i]["totalComp"] 
                ));
            }
           
        #elif UNITY_ANDROID

            TextAsset file = Resources.Load("Procesos") as TextAsset;
            string jsonString = file.ToString ();
            mapaArchivoJson = JsonMapper.ToObject(jsonString);
           
            for (int i = 0; i < mapaArchivoJson.Count; i++)
            {
                archivoCargado.Add(new SavedFile( mapaArchivoJson[i]["ID"].ToString(),  (int)mapaArchivoJson[i]["X"], (int)mapaArchivoJson[i]["Y"], mapaArchivoJson[i]["Name"].ToString(), (int)mapaArchivoJson[i]["Sociedad"], (int)mapaArchivoJson[i]["comp"] 
                ));
            }
           
        #endif   

        Debug.Log("cantidad de componentes en el archivo "+archivoCargado.Count);
        
        //Cargar componentes en el mapa
        if (tablero.transform.childCount == 4 || GameObject.Find("Inventory").GetComponent<Inventory>().cleared){ //controlar si se limpio el tablero
            for (int i = 0; i < archivoCargado.Count; i++){
                //guardar la sociedad activa en el momento
                int AuxSoc = tablero.GetComponent<ControllerBoard>().socGeneral;
                tablero.GetComponent<ControllerBoard>().socGeneral = archivoCargado[i].sociedad;
                //generar la pieza en el tablero
                loadFlag = true;
                tablero.GetComponent<ControllerBoard>().GeneratePiece(archivoCargado[i].x, archivoCargado[i].y, archivoCargado[i].id,archivoCargado[i].comp);
                loadFlag = false;
                //buscar la pieza agregada y asignar nombre y color
                p = cuadricula[archivoCargado[i].x, archivoCargado[i].y];
                p.setName(archivoCargado[i].name);
                p.totalComp = archivoCargado[i].totalComp;
                p.transform.GetChild(1).gameObject.transform.GetComponent<TextMesh>().text = p.totalComp.ToString();
                //colocar la sociedad al tablero
                tablero.GetComponent<ControllerBoard>().socGeneral = AuxSoc;
            }
        }

        GameObject.Find("Code").GetComponent<GranjeroControllerAux>().WrapperCrearCaminos();

        StartCoroutine(BorrarTemp());
        if (loadVentana)
        {
            loadVentana = false;
        }
    }//fin load
    
    public void DownloadWrapper(string fileName, bool fadmin){
        StartCoroutine(DownloadData(fileName,fadmin));
    }//fDownloadWrapper

    private IEnumerator DownloadData(string fileName, bool fadmin){
        string rutaRemota, rutaLocal;
        if(fadmin){ //si es un archivo protegido

        
            rutaLocal = Application.persistentDataPath+"\\MapasGuardados\\admin";  
            rutaRemota = "http://xplora2-0.000webhostapp.com/MapasGuardados/admin";

        
        }else{
              if(!online){

                  rutaLocal = Application.persistentDataPath+"\\MapasGuardados\\";
                  rutaRemota = "";

              }else{

                  rutaRemota = "http://xplora2-0.000webhostapp.com/MapasGuardados/" + GestionBD.single.nombreUsuario;
                  rutaLocal = Application.persistentDataPath+"\\MapasGuardados\\" + GestionBD.single.nombreUsuario;
              }
        }

        if(online){
            
            WWW www = new WWW(rutaRemota + "/" + fileName);
            Debug.Log(rutaRemota + "/" + fileName);
            yield return www;
            string filecontent = www.text;
            Debug.Log(filecontent);
            File.WriteAllText(rutaLocal + "\\" + fileName, filecontent);
            Debug.Log("Descargado");
            yield return new WaitForSeconds(1f);
        }

        LoadBefore(rutaLocal + "\\" + fileName); //Carga mapas del usuario
    }//fDownloadData

    private void CerrarCargar()
    {
        btnLoadExit.GetComponent<Button>().onClick.AddListener(() => {
            //borrar los botones del panel de carga
            CerrarLoadYBorrarBotones();
        });
    }//fCerrarCargar

    public void CerrarLoadYBorrarBotones(){
        GameObject[] botonesC = GameObject.FindGameObjectsWithTag("botoncarga");
        foreach(GameObject go in botonesC){
            Destroy(go);
        }
        panelLoad.SetActive(false);
    }//fCerrarLoadYBorrarBotones
             
    //Carga mapas, se llama desde el script LoadButton
    public void LoadBefore(string fileName)
    {
        print("lb");
        flagOpened = false;
        flagLoaded = true;
        mapName = fileName; // nombre del mapa cargado


        if (!(fileName.Contains(".xplora2")))
       {          
           fileName = fileName + ".xplora2"; //aarb
       }
       
       /*
        Debug.Log("fileName = " + fileName);
        Debug.Log("mapName = " + mapName);                   
       */
          
        //verificar si existe el archivo, limpiar tablero, llamar a Load()
        if (File.Exists(fileName))
        {
            print("existe");
            //limpiar tablero antes de colocar las piezas
            inventario.Destroyer();         

            StartCoroutine(Clean(fileName));
         
        }
    }//fLoadBefore

    public void LoadFromOutside(string nombre)
    { //aarb
        //para cargar mapas desde otra clase

         if(!online){

            LoadBefore(Application.persistentDataPath + offlinePath + nombre);

         }else{

            LoadBefore(Application.persistentDataPath + onlinePath + nombre);

         }

    }//fLoadFromOutside


    IEnumerator BorrarTemp() {
        bool borrado = false;

        if(!online){

            while (File.Exists(Application.persistentDataPath+offlinePath+"tempGSceBk.xplora2") && !borrado) {
                try {
                    File.Delete(Application.persistentDataPath+offlinePath+"tempGSceBk.xplora2");                    
                    flagLoaded = false;
                    flagOpened = true;
                    borrado = true;
                }
                catch (Exception) {
                    //
                }

            }
        }else{

            while (File.Exists(Application.persistentDataPath+onlinePath+"\\tempGSceBk.xplora2") && !borrado) {
                try {
                    File.Delete(Application.persistentDataPath+onlinePath+"\\tempGSceBk.xplora2");
                    StartCoroutine(DeleteFile("tempGSceBK"));
                    flagLoaded = false;
                    flagOpened = true;
                    borrado = true;
                }
                catch (Exception) {
                    //
                }
            }
        }


        yield break;
    }//fBorrarTemp

    IEnumerator Clean(string fileName) {

       //espera hasta que se terminen de borrar los componentes del tablero
        //yield return new WaitUntil(() => tablero.transform.childCount == 4);
        //al terminarse carga el archivo
        
        Load(fileName);
        yield break;
    }//fClean
       
    IEnumerator AddFile(string fileName)
    {
        //Agrega  nombre de archivo a la BD

        WWW connect = new WWW("http://xplora2-0.000webhostapp.com/registroArchivo.php?uss=" + GestionBD.single.nombreUsuario + "&filename=" + fileName);
        //WWW connect = new WWW("http://localhost/test/registroArchivo.php?uss=" + GestionBD.single.nombreUsuario + "&filename=" + fileName);

        yield return (connect);
                       
        if (connect.text == "error")
        {         
            Debug.Log("Error al guardar archivo");

        }       
        else if (connect.text == "File Saved"){

            Debug.Log("Archivo Guardado en la BD");

        }
        else{
            Debug.Log("Error conexión de la BD Archivos "+connect.text);

        }

        yield break; 

    }//fAddFile

    IEnumerator DeleteFile(string fileName)
    {
        //Borra archivo en la BD 
        //Para evitar duplicados al sobreescribir un mapa

        WWW connect = new WWW("http://xplora2-0.000webhostapp.com/borrarArchivo.php?uss=" + GestionBD.single.nombreUsuario + "&filename=" + fileName);
        // WWW connect = new WWW("http://localhost/test/borrarArchivo.php?uss=" + GestionBD.single.nombreUsuario + "&filename=" + fileName);
        yield return (connect);

        if (connect.text == "error")
        {
            Debug.Log("Error al borrar el archivo");

        }
        else if (connect.text == "File Deleted")
        {

            Debug.Log("Archivo Borrado de la BD");

        }
        else
        {

            Debug.Log("Error conexión de la BD Archivos");

        }

        yield break;

    }//fDeleteFile

    IEnumerator FileData()
    {
        //Lista los nombres de archivos que posee un usuario

        WWW connect = new WWW("http://xplora2-0.000webhostapp.com/datosArchivo.php?uss=" + GestionBD.single.nombreUsuario);
       // WWW connect = new WWW("http://localhost/test/datosArchivo.php?uss=" + GestionBD.single.nombreUsuario);
        yield return (connect);

        if (connect.text == "error")
        {
            Debug.Log("Error en la conexión");

        }
        else if (connect.text == "none")
        {
            Debug.Log("El usuario no posee mapas guardados");
            Cargar(null);
            loadCreados = true;

        }
        else
        {
            string[] mapas = connect.text.Split('|');
            mapas = mapas.Distinct().ToArray(); //Quita nombres duplicados en la BD
            Debug.Log("Carga de nombres de mapas desde la bd");
            Cargar(mapas);          
                    
        }

    }//fFileData
           

 
}//Class

[Serializable]
public class SavedFile
{
    public string id;
    public int x;
    public int y;
    public string name;
    public int sociedad;
    public int comp;
    public int totalComp;

    public SavedFile(string ID, int X, int Y, string Name, int Sociedad, int Component, int Cantidad){
        this.id = ID;
        this.x = X;
        this.y = Y;
        this.name = Name;       
        this.sociedad = Sociedad;
        this.comp = Component;
        this.totalComp = Cantidad;
    }
}
