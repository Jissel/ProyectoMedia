using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class GranjeroController : MonoBehaviour {
    private Vector3 origen, destino;
    private Vector3 pieceOffset;
    private Vector3 boardOffset;
    public GameObject tablero;
    private Piece[,] cuadricula;
    private List<Posicion> visitados;
    private bool mover;
    private int contMovmimientos, contVeces, contVector;
    private int tamCamino;
    private static Vector3 suplente;
    public bool llegoAlFinal, control;
    private bool cam;
    public bool esquinas;
    private bool mouseSobreUI;
    private bool mostrarEsquinas;
    private int contFallidos;
    private List<Vector3> caminoList;
    private int[] vectorRotaciones;
    private GranjeroControllerAux grjCtrAux;
    private bool moveCamino;
    List<Node> path;
    List<int> rotaciones;

    // Use this for initialization

    void Start() {
        pieceOffset = new Vector3(0.5f, 0, 0.5f);
        boardOffset = new Vector3(-12f, 0, -12f);
        //lineaCamino = GetComponent<LineRenderer>();
        contVeces = 0;
        control = Inicializar();
        esquinas = false;
        control = false;
    }

    public bool Inicializar() {
        //inicializacion de las variables
        tablero = GameObject.Find("Tablero");
        cuadricula = tablero.GetComponent<ControllerBoard>().pieces;
        //InicializarDirecciones();
        mover = false;
        contMovmimientos = 0;
        contFallidos = 0;
        tamCamino = 1000;
        contVector = 0;
        llegoAlFinal = false;
        //GetComponent<LineRenderer>().positionCount = 0;
        visitados = new List<Posicion>();
        /* camino = null;
		camino = new Vector3[tamCamino];
		camino.Initialize(); */
        vectorRotaciones = new int[tamCamino];
        mouseSobreUI = false;
        origen = transform.position;
        destino = origen;
        suplente = transform.position;
        caminoList = new List<Vector3>();
        cam = GameObject.Find("Code").GetComponent<GranjeroControllerAux>().mostrarCam; //estado original del boton
        grjCtrAux = GameObject.Find("Code").GetComponent<GranjeroControllerAux>();
        mostrarEsquinas = GameObject.Find("Code").GetComponent<GranjeroControllerAux>().mostrarEsquinas;
        moveCamino = false;
        return true;
    }//fInicializar

    public void WrapperMove(CaminoSimple camino) {
        path = camino.camino;
        rotaciones = camino.vectorRotaciones;
        //this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        StartCoroutine(MoverGranjerito());
    }

    private IEnumerator MoverGranjerito() {
        while (true) {
            if (cuadricula[path.Last().gridX, path.Last().gridY].getIDPiece() == "Export") {
                StartCoroutine(InvisibleGranjero());
            }

            int i = 0;
            float tiempo;

            tiempo = 1f;
            Vector3 pos;
            foreach (Node node in path) {
                if (node != path.First() && node != path.Last()) {
                    pos = node.worldPosition;
                    /* this.gameObject.transform.eulerAngles = new Vector3(0, rotaciones[i], 0); */
                    if (pos != Vector3.zero) {
                        float startTime = Time.time;
                        float endTime = startTime + tiempo;
                        //GetComponent<Animator>().SetTrigger("walk");
                        while (Time.time < endTime) {
                            float timeSoFar = Time.time - startTime;
                            float fractionTime = timeSoFar / tiempo;

                            if (i < path.Count - 1 && i > 0) { //para controlar que no se salga del limite
                                transform.position = Vector3.Lerp(path[i].worldPosition + Vector3.up * 0.15f, path[i + 1].worldPosition + Vector3.up * 0.15f, fractionTime);
                            }

                            yield return null;
                        }
                    }
                    i++;
                }
            }
            //GetComponent<Animator>().SetTrigger("idle");

            //Loop de camino
            transform.position = (Vector3.right * path[0].gridX) + (Vector3.forward * path[0].gridY) + boardOffset + pieceOffset;
            if (cuadricula[path.Last().gridX, path.Last().gridY].getIDPiece() == "Export") {
                StartCoroutine(VisibleGranjero());
            }
            yield return null;
        }

        //yield break;

    }//fMoverGranjerito

    public IEnumerator InvisibleGranjero() {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject g = gameObject.transform.GetChild(i).gameObject;
            Renderer rend = g.GetComponent<Renderer>();

            foreach (Material m in rend.materials) {
                yield return new WaitForSeconds(0.7f);
                m.SetFloat("_Mode", 2);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0);
                m.DisableKeyword("_ALPHATEST_ON");
                m.EnableKeyword("_ALPHABLEND_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.renderQueue = 3000;
            }
            iTween.FadeTo(g, 0, 1);//original
        }
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject g = gameObject.transform.GetChild(i).gameObject;
            g.GetComponent<Renderer>().enabled = false;
        }
        yield break;
    }//fInvisibleGranjero

    public IEnumerator VisibleGranjero() {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject g = gameObject.transform.GetChild(i).gameObject;
            Renderer rend = g.GetComponent<Renderer>();

            iTween.FadeTo(g, iTween.Hash("alpha", 1, "time", 1.0f, "Delay", 1));

            foreach (Material m in rend.materials) {
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                m.SetInt("_ZWrite", 1);
                m.DisableKeyword("_ALPHATEST_ON");
                m.DisableKeyword("_ALPHABLEND_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.renderQueue = -1;
            }
        }
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject g = gameObject.transform.GetChild(i).gameObject;
            g.GetComponent<Renderer>().enabled = true;
        }
        yield break;
    }//fVisisbleGranjero

}