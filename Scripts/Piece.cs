using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    public string id;
    public string nombre;
    public string nombreUI;//nombre configurable por el usuario
    public int Rotation;
    public string color;
    public List<Posicion> posiciones = new List<Posicion>();
    private bool isInvisible;
    private GameObject granjero;
    public Sprite sprite;
    public int type;
    public int sociedad;
    public Inventory inventory;
    public int totalComp;
    public int idnumerico;

    void Awake() {
        id = "";
        Rotation = 0;
    }

    
    void Update() {
       
    }

    private void Start() {
        color = "Red";
    }

    public int getIDNumber(){
        //modificado
        int index = -1;
        switch(id){
            case "Huevo Fertil":
                index= 0;
                this.idnumerico = 0;
            break;
            case "Cria y Levante":
                index= 1;
                this.idnumerico = 1;
            break;
            case "Planta ABA":
                index= 2;
                this.idnumerico = 2;
            break;
            case "Incubadora":
                index = 3;
                this.idnumerico = 3;
            break;
            case "Ponedoras":
                index = 4;
                this.idnumerico = 4;
            break;
            case "Engorde":
                index = 5;
                this.idnumerico = 5;
            break;
            case "Soporte":
                index = 6;
                this.idnumerico = 6;
            break;
            case "Mantenimiento":
                index = 7;
                this.idnumerico = 7;
            break;
            case "Beneficio":
                index = 8;
                this.idnumerico = 8;
            break;
        }
        /* switch(id){
            case "XPCRI1":
                index= 0;
            break;
            case "XPCRI2":
                index= 1;
            break;
            case "XPCRI3":
                index= 2;
            break;
            case "Export":
                index = 3;
            break;
        } */
        return index;
    }

    public int getSociedad() {
        return sociedad;
    }

    public string getIDPiece() {
        return id;
    }

    public void setIDPiece(string i) {
        id = i;
    }

    public string getName() {
        return nombre;
    }

    public void setName(string n) {
        nombre = n;
    }

    public string getNameUI() {
        return nombreUI;
    }

    public void setNameUI(string n) {
        nombreUI = n;
    }

    public string getColor() {
        return color;
    }

    public void setColor(string c) {
        color = c;
    }

    public bool getIsInvisible() {
        return isInvisible;
    }

    public void setIsInvisible(bool i) {
        isInvisible = i;
    }

    public int getTypePiece() {
        return type;
    }

    public void setTypePiece(int i) {
        type = i;
    }

    public bool ValidMove(Piece[,] board, int x2, int y2)
    {
        return ((board[x2, y2] != null) ? false : true);
    }

    public int GetX(int pos){
        return(posiciones[pos].xP);
    }

    public int GetY(int pos){
        return(posiciones[pos].yP);
    }

    public void SetX(int x){
        posiciones[0].xP=x;
    }

    public void SetY(int y){
        posiciones[0].yP=y;
    }

    public int getTotalComp() {
        return totalComp;
    }

    public void setTotalComp(int i) {
        totalComp = i;
    }
}

public class Posicion {
    public int xP { get; set; }
    public int yP { get; set; }
}
