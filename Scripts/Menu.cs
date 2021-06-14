using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {

    public bool showMenu, showMenuA, destroy, rotate, rnd, invisible, isWhite, buttonColor, buttonEdition, buttonInfo, ButtonVisAct;
    private Vector3 boardOffset = new Vector3(-12f, 0, -12f);
    public bool move, BandEnter, BandOnClick;
    public bool ModificarXY;
    public bool ready;
    private GameObject panel, tablero, panelSave, panelLoad, btnSave, fieldS, panelEdit, btnYes, btnNo, btnYes1, btnNo1;
    GameObject panelTitle, panelColor, panelFiltroCam, fieldBSTotalC, MenuStickers, MenuFicha;
    public GameObject panelInventory, PanelConfirmacion, PanelConfirmacion1, MenuPanel, MenuPanelColor, panelStats, panelProc, PanelSupport,panelInfo, panelSociedad, btnShow, BSTotalC;
    public int x, y, cont, xAux, yAux;
    public ControllerBoard ControllerBoard;
    public EventSystem eventS;
    private string textInput, info, data;
    CanvasGroup showPanel;
    ItemDatabase database;
    Text textStatus, textTC;
    public Renderer rend;
    Material m_Material;
    public bool guardadoTut;
    public int RotX, RotY, ItemX, ItemY;
    public Inventory inventory;
    public Image image;
    public CameraController Camera;
    

    void Start() {
        cont = 1;
        x = -1;
        y = -1;
        RotX = 0;
        RotY = 0;
        move = true;
        ready = true;
        destroy = false;
        rotate = false;
        rnd = true;
        isWhite = false;
        buttonColor = false;
        buttonEdition = false;
        buttonInfo = false;
        ModificarXY = true;
        guardadoTut = false;
        ButtonVisAct = false;
        BandEnter = false;
        BandOnClick = false;

        //FDDM
        fieldBSTotalC = GameObject.Find("InputField");//Input del menu editar para la cantidad de componentes
        textTC = GameObject.Find("TextTC").GetComponent<Text>();//Texto que esta en la ventana que solicita la cantidad de elementos del componente.
        //FDDM

        btnSave = GameObject.Find("ButtonSaveEdition");
        showPanel = GameObject.Find("PanelEdition").GetComponent<CanvasGroup>();
        fieldS = GameObject.Find("InputSaveEdition");
        panelEdit = GameObject.Find("PanelEdition");
        ControllerBoard = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
        tablero = GameObject.Find("Tablero");
        panelSave = GameObject.Find("PanelGuardar");
        panel = GameObject.Find("PanelInfo");
        panelLoad = GameObject.Find("PanelCargar");
        panelProc = GameObject.Find("PanelProcesos");
        panelInfo = GameObject.Find("PanelInfo");
        panelColor = GameObject.Find("PanelChangeColor");
        textStatus = GameObject.Find("TextStatusEdition").GetComponent<Text>(); 
        panelSociedad = GameObject.Find("sociedad");
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        Camera = GameObject.Find("Camera").GetComponent<CameraController>();
        btnShow = GameObject.Find("ShowCategory");
        panelEdit.SetActive(false);
        panelColor.SetActive(false);
        BSTotalC = GameObject.Find("ButtonSaveTotalC");
        MenuStickers = GameObject.Find("MenuStickers");
        MenuFicha = GameObject.Find("PanelFicha");
        PanelSupport= GameObject.Find("PanelSoporte");
        PanelConfirmacion = GameObject.Find("PanelConfirmacion");
        PanelConfirmacion1 = GameObject.Find("PanelConfirmacion1");
        btnYes1 = GameObject.Find("Yes1");
        btnNo1 = GameObject.Find("No1");
        btnYes = GameObject.Find("Yes");
        btnNo = GameObject.Find("No");
        MenuStickers.SetActive(false);
        MenuFicha.SetActive(false);
        PanelSupport.SetActive(false);
        PanelConfirmacion.SetActive(false);
        PanelConfirmacion1.SetActive(false);
    }

    void Update() {

        if (ControllerBoard.TotalComp.activeInHierarchy && (ItemX > -1 && ItemX > -1)) {

            if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("enter") || Input.GetKey(KeyCode.Return)) {
                BandEnter = true;
            }

            if (Input.GetMouseButtonDown(0)) {
                BandOnClick = true;
            }
            MoreComponents();
        }
    }


    public void ButtonShowMenu() {
        #if UNITY_STANDALONE
            if (!showMenu) {
                showMenu = true;
                }
            else if (showMenu) {
                showMenu = false;
                }
        #elif UNITY_ANDROID
                if (!showMenuA) {
                    showMenuA = true;
                }

                else if (showMenuA) {
                    showMenuA = false;
                }
        #endif
    }

    public void Buttonmove() {
        ControllerBoard.StartCoroutine("RotTablero");
    }

    public void OpenPanelAux() {
        OpenPanel(x, y);
    }

    public void OpenPanel(int Startx, int Starty) {
        Piece p = ControllerBoard.GetPiece(Startx, Starty);
        if (p.getIDPiece() == "Soporte") {
            PanelSupport.SetActive(true);
            GameObject.Find("Code").GetComponent<ProcessList>().SoporteProcess();
        }
        else {
            MenuFicha.SetActive(true);
        }
        
        panelSociedad.SetActive(false);
        btnShow.SetActive(false);
        x = Startx;
        y = Starty;
        ViewTitle(x, y);

        Sprite imageS = Resources.Load<Sprite>("Sprites/Items/" + p.id);
        image.sprite = imageS;

        ready = false;
    }

    public void DestroyGameObjectButton() {//FDDM
        
        PanelConfirmacion1.SetActive(true);
        PanelConfirmacion1.transform.GetChild(0).GetComponent<Text>().text = "Desea Eliminar el componente actual?";
        btnYes1.GetComponent<Button>().onClick.AddListener(() => {
            ControllerBoard.DestroyFarm(x, y);
            x = -1;
            y = -1;
            ready = true;
            MenuStickers.SetActive(false);
            PanelConfirmacion1.SetActive(false);
            ReturnZoom();
            GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        });

        btnNo1.GetComponent<Button>().onClick.AddListener(() => {
            PanelConfirmacion1.SetActive(false);
        });

    }//FDDM

    public void ReturnZoom() {//FDDM

        Camera.StartCoroutine("Reset");
        MenuStickers.SetActive(false);
        btnShow.SetActive(true);
        panelSociedad.SetActive(true);
        if (inventory.activeCat) {
            inventory.showC = true;
        }
        if (inventory.activeInv) {
            inventory.showI = true;
        }
        
        x = -1;
        y = -1;
        ready = true;
        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
    }//FDDM

    public void SaveFarmCreate(int x, int y) {
        ItemX = x;
        ItemY = y;
        ControllerBoard.TotalComp.SetActive(true);
        fieldBSTotalC.GetComponent<InputField>().text = "1";
    }

    public void MoreComponents() {//FDDM
      
        GameObject.Find("Camera").GetComponent<CameraController>().mover = false;
        fieldBSTotalC.GetComponent<InputField>().ActivateInputField();
        fieldBSTotalC.GetComponent<InputField>().Select();

        Piece p;
        p = ControllerBoard.GetPiece(ItemX, ItemY);

        if (BandEnter) {
            SaveMoreComp(p);

        }
        else if (BandOnClick) {

            BSTotalC.GetComponent<Button>().onClick.AddListener(() => {
                SaveMoreComp(p);
            });
        }
        

    }//FDDM

    void SaveMoreComp(Piece p) {

        GameObject Aux;
        GameObject.Find("Code").GetComponent<Menu>().ModificarXY = true;
        int i, aux;
        bool band = false;
        GameObject AuxGO;

        //fieldBSTotalC.GetComponent<InputField>().text = "1";
        //GameObject.Find("InputField").GetComponent<InputField>().text = "1";
        textInput = (int.Parse(fieldBSTotalC.GetComponent<InputField>().text)).ToString(); //toma el string al pulsar el boton

        if (string.IsNullOrEmpty(textInput)) {
            textTC.color = Color.red;
            textTC.text = "El campo esta vacio";
        }
        else {
            aux = int.Parse(textInput);
            i = 0;
            if (aux > 0) {

                AuxGO = p.transform.GetChild(1).gameObject;
                AuxGO.transform.GetComponent<TextMesh>().text = textInput;
                p.setTotalComp(aux);
                band = true;

                textTC.color = Color.green;
                textTC.text = " ";
                ControllerBoard.TotalComp.SetActive(false);
                BSTotalC.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
            }
            else {
                textTC.color = Color.red;
                textTC.text = "El valor debe ser mayor a 0";
            }
        }

        
        BandEnter = false;
        BandOnClick = false;
        ItemX = -1;
        ItemY = -1;
    }

    public void Close() {
        if (PanelSupport.activeInHierarchy && panelProc.activeInHierarchy) {
            panelProc.SetActive(false);
        }
        else {
            PanelSupport.SetActive(false);
            MenuFicha.SetActive(false);
        }
        
        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        ready = true;

        if (!panelSave.activeInHierarchy)
            panelProc.SetActive(false);
    }

    public void Estructura() {
        panelProc.SetActive(false);
    }

    public void DeployMenuPanel(int Startx, int Starty) {
        cont = 1;
        RotX = -1;
        RotY = -1;
        panelStats = GameObject.Find("PanelStatistics");
        bool AuxZoom = GameObject.Find("Camera").GetComponent<CameraController>().ZoomLim;
        if (panelSave.activeInHierarchy || panelLoad.activeInHierarchy || panelColor.activeInHierarchy || panel.activeInHierarchy || panelEdit.activeInHierarchy || panelStats != null) {
            ModificarXY = false;
        }
        else {
            ModificarXY = true;
            Piece p = ControllerBoard.GetPiece(Startx, Starty);
            Piece pOpen = ControllerBoard.GetPiece(ControllerBoard.openX, ControllerBoard.openY);
            //activa opciones
            if (x == -1 && y == -1 && ready) {
                btnShow.SetActive(false);
                MenuStickers.SetActive(true);
                if (inventory.activeCat) {
                    inventory.showC = false;
                }
                if (inventory.activeInv) {
                    inventory.showI = false;
                }
                GameObject.Find("Camera").GetComponent<CameraController>().mover = false;
                panelSociedad.SetActive(false);
                x = Startx;
                y = Starty;

                ready = false;
            }
            else if (x == Startx && y == Starty && !ready || !AuxZoom) {//desactiva opciones
                    MenuPanel.SetActive(false);
                    x = -1;
                    y = -1;
                    ready = true;
                    GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
                }
        }
    }

    //funcion que muestra el nombre de un componente en el menu de componente
    public void ViewTitle(int x, int y) {
        Piece p;
        p = ControllerBoard.GetPiece(x, y);
        if (p.nombreUI=="Huevo Fertil") {
            p.nombreUI = "Huevo Fértil";
        }
        if (p.nombreUI == "Cria y Levante") {
            p.nombreUI = "Cría y Levante";
        }
        data = "<b>" + p.nombreUI + "\n</b>\n";
        panelTitle = GameObject.Find("Title");
        panelTitle.transform.GetChild(0).GetComponent<Text>().text = data;
    }

    //funcion que invoca a las funciones que modifican la visibilidad de la superficie de un componente
    //es usada en el menu de componente
    public void ButtonRender() {
        Piece Componente = ControllerBoard.GetPiece(x, y);

        //si el componente esta visible invoca a la funcion Invisible
        if (!Componente.getIsInvisible()) {
            StartCoroutine("Visible");
            invisible = true;
        }
        //si el componente esta invisible invoca a la funcion Visible
        else if (Componente.getIsInvisible()) {
            StartCoroutine("Invisible");
            invisible = false;
        }
    }
    
    public void ModXY() {
        x = -1;
        y = -1;
        ready = true;
    }

    public void DestroyGameObject() {
        print("DestroyGameObject x "+x+ " y "+ y);
        ControllerBoard.DestroyFarm(x, y);
    }

    //funcion que regresa a la escena del menu principal
    public void ButtonReturn() {
        PanelConfirmacion.SetActive(true);
        btnYes.GetComponent<Button>().onClick.AddListener(() => {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        });

        btnNo.GetComponent<Button>().onClick.AddListener(() => {
            PanelConfirmacion.SetActive(false);
        });
        
    }

    public void ButtonZoom() {
        SceneManager.LoadScene("Zoom", LoadSceneMode.Single);
    }

    public void ButtonXplora() {
        SceneManager.LoadScene("tablero", LoadSceneMode.Single);
    }

    //funcion que muestra la descripcion de un componente (es usada en el menu de componente)
    public void DeployInfo() {
        if (panelColor.activeInHierarchy){
            panelColor.SetActive(false);
        }else if(panelEdit.activeInHierarchy){
            panelEdit.SetActive(false);
        }else if(panelProc.activeInHierarchy){
            panelProc.SetActive(false);
        }
        
        GameObject panel = GameObject.Find("PanelEdition");
        if (panel == null) {
            buttonInfo = true;
            ControllerBoard.ViewInfo(x, y);
            GameObject.Find("Camera").GetComponent<CameraController>().mover = false;
        }

    }

    // funcion que modifica el nombre de un componente (es usada por el menu de componente)
    public void ButtonEdition() {
        if (panelInfo.activeInHierarchy) {
            panelInfo.SetActive(false);
        }
        else if (panelProc.activeInHierarchy) {
            panelProc.SetActive(false);
        }
        if (panelColor.activeInHierarchy) {
            panelColor.SetActive(false);
        }

        GameObject panel = GameObject.Find("PanelInfo");
        if (panel == null) {
            panelEdit.SetActive(true);

            showPanel.alpha = 1;
            StatusText("", Color.white);
            eventS.SetSelectedGameObject(fieldS);
            buttonEdition = true;
            GameObject.Find("Camera").GetComponent<CameraController>().mover = false;

            
            btnSave.GetComponent<Button>().onClick.AddListener(() => {
                Piece p;
                p = ControllerBoard.GetPiece(x, y);

                textInput = fieldS.GetComponent<InputField>().text; //toma el string al pulsar el boton

                if (string.IsNullOrEmpty(textInput)) {
                    StatusText("El campo esta vacio", Color.red);
                }
                else {
                    guardadoTut = true;
                    
                        p.setNameUI(textInput);
                  
                    StatusText("Modificación Exitosa", Color.green);
                }
                data = "<color=#DF0174><b>" + p.nombreUI + "\n</b></color>\n";
                panelTitle = GameObject.Find("Title");
                panelTitle.transform.GetChild(0).GetComponent<Text>().text = data;
            });
            fieldS.GetComponent<InputField>().text = "";
        }

    }

    // modifica el color del texto enviado
    private void StatusText(string texto, Color color) {
        textStatus.color = color;
        textStatus.text = texto;
    }
  
    
    public void CloseMenuPanel() {
        MenuPanel.SetActive(false);
        panel.SetActive(false);
        panelEdit.SetActive(false); 
        panelColor.SetActive(false);
        panelProc.SetActive(false);

        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        x = -1;
        y = -1;
        ready = true;
        buttonInfo = false;
        buttonEdition = false;
        buttonColor = false;
    }

    public void ResetClose() {
        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        ready = true;
    }

    //se cierra el panel que muestra la descripcion de un componente
    public void ExitInfo() {
        print("exitInfo");
        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        panel = GameObject.Find("PanelInfo");
        //ViewTitle(x, y);
        panel.SetActive(false);
        buttonInfo = false;
    }

    //se cierra el panel de edicion del componente
    public void ExitEdition() {
        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        buttonEdition = false;
        panelEdit.SetActive(false);
    }

    // se cierra el panel de cambio de color del componente
    public void ExitChangeColor() {
        GameObject.Find("Camera").GetComponent<CameraController>().mover = true;
        fieldS.GetComponent<InputField>().text = "";
        buttonColor = false;
        panelColor.SetActive(false);
        panelEdit.SetActive(true);
    }

   public void Video() {
        print("video");
        GameObject video = Instantiate(Resources.Load<GameObject>("Sprites/Prefabs/IntroVideo")) as GameObject;
        video.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

}
