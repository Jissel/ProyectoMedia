using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Casilla : MonoBehaviour{

    //Probando las casillas que indican si la casilla esta ocupada o n
    public Material colorCasilla;
    public int numC = 0;

    void Start() {

    }

    // Update is called once per frame
    //public void OnMouseEnter() {

      //  if (Input.GetMouseButton(0)) {
        //}
    //}

    public void OnMouseExit() {
        GetComponent<Renderer>().enabled = false;
    }

    public void ChangeColor(bool free, Casilla[,] c, int x, int y) {

        if (free) {
            //verde
            c[x, y].GetComponent<Renderer>().material.color = ConvertColorRGBA(51, 255, 51, 150); 
        } else{
            //rojo
            c[x, y].GetComponent<Renderer>().material.color = ConvertColorRGBA(255, 0, 0, 150);
        }
        GetComponent<Renderer>().enabled = false;
    }

    static public Color ConvertColorRGBA(float r, float g, float b, float a) {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }

}


