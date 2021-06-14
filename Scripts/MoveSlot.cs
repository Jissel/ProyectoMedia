using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveSlot : MonoBehaviour, IDropHandler {
	private Inventory inv;
    GameObject slotPanel, slotPanel1;
    public Vector2 PosicionInicial;
    public Camera Camera;


    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    void Start () {
		 inv = GameObject.Find("Inventory").GetComponent<Inventory>();

        //Fetch the Raycaster from the GameObject (the Canvas)
        raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        eventSystem = GetComponent<EventSystem>();
        Camera = GameObject.Find("Camera").GetComponent<Camera>();
    }

	void Awake(){
    }


    void Update() {
        /*if (Input.GetKey(KeyCode.Mouse0)) {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results) {
                Debug.Log("Hit " + result.gameObject.name);
            }
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0) {
            //WorldUI is my layer name
            if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI")) {
                string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                //Debug.Log("GrandChild Element: "+results[0].gameObject.name);
                results.Clear();
            }
        }


            /* if (Input.GetMouseButtonDown(0))
                //Debug.Log("Pressed primary button.");

            if (Input.GetMouseButtonDown(1))
                //Debug.Log("Pressed secondary button.");

            if (Input.GetMouseButtonDown(2))
                //Debug.Log("Pressed middle click.");



            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0)) {
                 if (Physics.Raycast(ray, out hit)) {
                     print(hit.transform.name);
                     if (hit.transform.name == "Slot Panel") {
                         Debug.Log("This is a Player");
                     }
                     else {
                         Debug.Log("This isn't a Player");
                     }
                 }
             }

            if (Input.touchCount > 0) {
                if (Input.touchCount == 1) {
                    Touch t = Input.touches[0];
                    if (t.phase == TouchPhase.Began) {
                        PosicionInicial = t.position;
                    }
                    else if (t.phase == TouchPhase.Ended) {
                        float swipeVertical = (new Vector3(0, t.position.y, 0) - new Vector3(0, PosicionInicial.y, 0)).magnitude;

                        if (swipeVertical > SwipeMinY) {
                            float u = Mathf.Sign(t.position.y - PosicionInicial.y);
                            if ((int)u > 0) {
                                print("horizontal");

                                // [ left - bottom ]
                                //slotPanel.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(-462.4f, 7.324997f);
                                // [ right - top ]
                                //slotPanel.gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(485.59f, 7.324997f);
                            }
                        }

                        float swipeHorizontal = (new Vector3(t.position.x, 0, 0) - new Vector3(PosicionInicial.x, 0, 0)).magnitude;

                        if (swipeHorizontal > SwipeMinX) {
                            float u = Mathf.Sign(t.position.x - PosicionInicial.x);
                            if ((int)u < 0) {
                                print("vertical");

                            }
                        }
                    }
                }
            }*/
        }

    public void OnDrop(PointerEventData eventData) {
        print("Script MoveSlot OnDrop");
        slotPanel = GameObject.Find("Slot Panel");
        if (Input.GetMouseButtonDown(0)) {
            print("down");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray)) {
                print("ray");
                //slotPanel = hit.point;
            }
        }
    }

    public void Next(){
        slotPanel = GameObject.Find("Slot Panel");
        slotPanel1 = GameObject.Find("Slot Panel1");

        #if UNITY_STANDALONE
                if (slotPanel.gameObject.GetComponent<RectTransform>().offsetMin.x>=-1062.18){
                    slotPanel.gameObject.GetComponent<RectTransform>().offsetMin += new Vector2(-84f, 0f);
                }
        #elif UNITY_ANDROID
                if (slotPanel1.gameObject.GetComponent<RectTransform>().offsetMin.x >= -1545.18) {
                    slotPanel1.gameObject.GetComponent<RectTransform>().offsetMin += new Vector2(-84f, 0f);
                }
        #endif
    }

    public void Back(){
        slotPanel = GameObject.Find("Slot Panel");
        slotPanel1 = GameObject.Find("Slot Panel1");
        
        #if UNITY_STANDALONE
                if(slotPanel.gameObject.GetComponent<RectTransform>().offsetMin.x<=-0.1800537){
                    slotPanel.gameObject.GetComponent<RectTransform>().offsetMin -= new Vector2(-84f, 0f);
                }
        #elif UNITY_ANDROID
                if (slotPanel1.gameObject.GetComponent<RectTransform>().offsetMin.x <= -0.1800537) {
                    slotPanel1.gameObject.GetComponent<RectTransform>().offsetMin -= new Vector2(-84f, 0f);
                }
        #endif
    }
}
