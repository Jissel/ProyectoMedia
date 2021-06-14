using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public ControllerBoard ControllerBoard;
    GameObject inventoryPanel, inventoryPanel1;
    GameObject slotPanel, slotPanel1;
    ItemDatabase database;
    public bool activeInv, activeCat;

    private bool ready;
    public GameObject tablero, panelInventory,panelCategory;
    public GameObject buttonMenu;
    public GameObject inventorySlot, inventorySlot1;
    public GameObject inventoryItem, inventoryItem1;

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    public Casilla[,] casillasAux = new Casilla[24, 24];
    bool aux;
    private Casilla cellAux;
    private GameObject scrollbar;

    private Vector3 posicion;
    private float posY;
    private float posZ;
    
    int ancho = 1024;
    private int anchoAux;
    public bool show,showC,showI,showIA;
    public bool cleared;

    public int cont;

    void Start() {
        ControllerBoard = GameObject.Find("Tablero").GetComponent<ControllerBoard>();
        tablero = GameObject.Find("Tablero");
        database = GetComponent<ItemDatabase>();
        slotAmount = 9;
        ready = true;
        activeInv = false;
        activeCat = true;
        panelCategory = GameObject.Find("InventoryCategory");                                                         
        //panelCategory.SetActive(false);
        anchoAux = Screen.width;
        show =false;
        showC =false;
        showI =false;
        showIA = false;
        cleared = false; //controla si se limpio el tablero
        cont = 0;
    }

    public void Destroyer() {
       for (int i = 0; i < tablero.transform.childCount; i++) {
            GameObject g = tablero.transform.GetChild(i).gameObject;
            Destroy(g);
        }

        for (int i = 0; i < 24; i++) {
            for (int j = 0; j < 24; j++) {
                ControllerBoard.BreakFree(i, j);
                ControllerBoard.pieces[i,j] = null;
            }
        }
        //PSB
        GameObject.Find("Code").GetComponent<SociedadesController>().VaciarDiccionarioComponentes();
        //PSB
        GameObject.Find("Code").GetComponent<GranjeroControllerAux>().selector = false;
        GameObject.Find("Code").GetComponent<Menu>().move = true;
        GameObject.Find("Tablero").GetComponent<ControllerBoard>().ComponentesMapa.Clear();
        StartCoroutine(GameObject.Find("Code").GetComponent<GranjeroControllerAux>().BorrarGranjeros());
        Debug.Log(" compMapa "+ControllerBoard.ComponentesMapa.Count);
            
        cleared = true;
    }

    public void ButtonShow() {
        if (!show){
            show = true;
            if(activeCat){
                showC=true;
            }
            if(activeInv){
                #if UNITY_STANDALONE
                    showI = true;
                #elif UNITY_ANDROID
                    showIA = true;
                #endif

            }
        }else if (show){
            show = false;
            if(activeCat){
                showC=false;
            }
            if(activeInv){
                #if UNITY_STANDALONE
                      showI = false;
                #elif UNITY_ANDROID
                      showIA = false;
                #endif
                GameObject.Find("Code").GetComponent<Menu>().ModificarXY = true;
            }
        }
    }

    public void AddItem(int id) {
        Item itemToAdd = database.FetchItemByID(id);
        

        for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == -1) {
                    items[i] = itemToAdd;
                    #if UNITY_STANDALONE
                        GameObject itemObj = Instantiate(inventoryItem);
                    #elif UNITY_ANDROID
                        GameObject itemObj = Instantiate(inventoryItem1);
                    #endif
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;

                    slots[cont].transform.GetChild(0).GetComponent<Text>().text= itemToAdd.Title;

                    itemObj.transform.localPosition = new Vector3(0, 0, 0);
                    itemObj.transform.localScale = new Vector3(1, 1, 1);
                break;
                }
            }
        cont++;
    }

   

    public void DeployInventory() {
        activeInv=true;
        activeCat=false;

        showC =false;
        #if UNITY_STANDALONE
               showI=true;
        #elif UNITY_ANDROID
               showIA = true;
        #endif
        if (ready) {

            #if UNITY_STANDALONE
                inventoryPanel = GameObject.Find("Inventory Panel");
                slotPanel = GameObject.Find("Slot Panel");
                for (int i = 0; i < slotAmount; i++) {
                        items.Add(new Item());
                        slots.Add(Instantiate(inventorySlot));

                        
                        slots[i].GetComponent<Slot>().id = i;
                        slots[i].transform.SetParent(slotPanel.transform);
                }
            #elif UNITY_ANDROID
                inventoryPanel1 = GameObject.Find("Inventory Panel1");
                slotPanel1 = GameObject.Find("Slot Panel1");
                for (int i = 0; i < slotAmount; i++) {
                    items.Add(new Item());
                    slots.Add(Instantiate(inventorySlot1));
                    slots[i].GetComponent<Slot>().id = i;
                    slots[i].transform.SetParent(slotPanel1.transform);
                }        
            #endif
            AddItem(0);
            AddItem(1);
            AddItem(2);
            AddItem(3);
            AddItem(4);
            AddItem(5);
            AddItem(6);
            AddItem(7);
            //AddItem(8);
            //AddItem(9);
            AddItem(10);
           
        }
        
        ready = false;
        ControllerBoard.UpdateBoard();
    }

    public void ReturnCategory(){
        activeCat=true;
        activeInv=false;
        showC=true;
        showI=false;
        showIA = false;
    }

    public void ExitInventory() {
        activeInv=false;
        panelInventory.SetActive(false);
        GameObject.Find("Code").GetComponent<Menu>().ModificarXY = true;
    }

    public void ExitCategory(){
        activeCat=false;
        panelCategory.SetActive(false);
    }

    bool CheckIfItemIsInInventory(Item item) {
        for (int i = 0; i < items.Count; i++){
            if (items[i].ID == item.ID) {
                return true;
            }
        }
        return false;
    }
}
