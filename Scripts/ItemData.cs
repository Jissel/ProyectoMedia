using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Item item;
    public int amount;
    public int slot;

    private Inventory inv;
    
    private Vector2 offset;

    private Piece selectedPiece;

    public ControllerBoard ControllerBoard;

    private Piece piece;

    private Vector3 boardOffset = new Vector3(-12f, 0, -12f);

    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;
    private int x, y, id;

    public int idItem;

    GameObject inventoryPanel;

    private Casilla selectedCell, cellAux;
    bool aux, Drag;
    public int ancho = 24, alto = 24;

    public Casilla[,] casillasAux = new Casilla[24, 24];

    float scaleSprite;
    PointerEventData eventData;
    bool repetir = true;
    public bool selectedItem = false;//FDDM
    public Menu Menu;

    public void Start() {
        ControllerBoard = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        inventoryPanel = GameObject.Find("Inventory Panel");
        Menu = GameObject.Find("Code").GetComponent<Menu>();
        //tamaño inicial del sprite
        scaleSprite = 1;
        selectedItem = false; //FDDM
    }

    public void Update() {
        ControllerBoard.UpdateMouseOver();
        x = (int)mouseOver.x;
        y = (int)mouseOver.y;

        if (selectedPiece != null) {
            ControllerBoard.UpdatePieceDrag(selectedPiece, x, y, (int)startDrag.x, (int)startDrag.y);
        }

        if (Drag) {
            idItem = item.ID;
            ControllerBoard.UpdateItemDrag(x, y, item);
        }
    }

    //se selecciona
    public void OnBeginDrag(PointerEventData eventData) {
        Drag = true;
        if (item != null && !ControllerBoard.TotalComp.activeInHierarchy) {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);

            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            //obtiene la camara y envia el objeto para ajustar el tamaño
            GameObject camara = GameObject.Find("Camera");
            camara.GetComponent<CameraController>().SetItemData(this);
        }

    }

    //Arrastrando
    public void OnDrag(PointerEventData eventData) {
        GameObject.Find("Camera").GetComponent<CameraController>().pan = false;
        if (!ControllerBoard.TotalComp.activeInHierarchy) {
            if (item != null) {
                this.transform.SetParent(inv.transform);
                this.transform.position = eventData.position - offset;
            }

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Board"))) {

                mouseOver.x = (int)(hit.point.x - boardOffset.x);
                mouseOver.y = (int)(hit.point.z - boardOffset.z);
            }
            else {
                mouseOver.x = -1;
                mouseOver.y = -1;
            }

            x = (int)mouseOver.x;
            y = (int)mouseOver.y;

            ControllerBoard.UpdateBoard();// NO QUITAR
            }
        
    }

    // se suelta el item
    public void GeneratePieceA(int x, int y) {

        GameObject.Find("Camera").GetComponent<CameraController>().pan = true;
        //luego de crear el objeto y saber que tipo es, debo saber cuales son los destinos
        //Creacion de objeto en el tablero dependiendo del item seleccionado
        //if(!GameObject.Find("Code").GetComponent<GranjeroControllerAux>().selector){ //controla si estan los caminos visibles o no
        //ModificarXY se coloca en falso para que no se abra el menu de componente automaticamente cuando se suelte el componente en el mapa
        GameObject.Find("Code").GetComponent<Menu>().ModificarXY = false;
        if (item.Title == "Huevo Fertil") {
            ControllerBoard.GeneratePiece(x, y, "Huevo Fertil", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Cria y Levante") {
            ControllerBoard.GeneratePiece(x, y, "Cria y Levante", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Planta ABA") {
            ControllerBoard.GeneratePiece(x, y, "Planta ABA", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Incubadora") {
            ControllerBoard.GeneratePiece(x, y, "Incubadora", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Ponedoras") {
            ControllerBoard.GeneratePiece(x, y, "Ponedoras", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Engorde") {
            ControllerBoard.GeneratePiece(x, y, "Engorde", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Soporte") {
            ControllerBoard.GeneratePiece(x, y, "Soporte", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Mantenimiento") {
            ControllerBoard.GeneratePiece(x, y, "Mantenimiento", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Transformado") {
            ControllerBoard.GeneratePiece(x, y, "Transformado", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Procesamiento") {
            ControllerBoard.GeneratePiece(x, y, "Procesamiento", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        else if (item.Title == "Beneficio") {
            ControllerBoard.GeneratePiece(x, y, "Beneficio", 1);
            mouseOver.x = 0;
            mouseOver.y = 0;
        }
        selectedItem = false;//FDDM
        GameObject.Find("Code").GetComponent<GranjeroControllerAux>().WrapperCrearCaminos();
       
    }

    public void OnEndDrag(PointerEventData eventData) {
        Drag = false;

        if (!ControllerBoard.TotalComp.activeInHierarchy) {
            this.transform.SetParent(inv.slots[slot].transform);
            this.transform.position = inv.slots[slot].transform.position;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Board"))) {

                mouseOver.x = (int)(hit.point.x - boardOffset.x);
                mouseOver.y = (int)(hit.point.z - boardOffset.z);
            }
            else {
                mouseOver.x = -1;
                mouseOver.y = -1;
            }

            x = (int)mouseOver.x;
            y = (int)mouseOver.y;

            GeneratePieceA(x, y);

            ResetTam(); //coloca el tamaño original del sprite
            GameObject camara = GameObject.Find("Camera");
            camara.GetComponent<CameraController>().SetItemData(null); //setea a null despues de soltar el objeto para evitar bugs

            //inv.ExitInventory();

            ControllerBoard.UpdateBoard();

            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }


    public void ResetTam() {
        //resetea el tamaño del gameobject
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }

  

}
