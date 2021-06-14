using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public Node[,] grid;
	public List<Node> path;
	private Vector3 boardOffset = new Vector3(-12f, 0, -12f);

    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);
	private Piece[,] tablero;


	// Use this for initialization
	void Start () {
		tablero = GameObject.Find("Tablero").GetComponent<ControllerBoard>().pieces;
		CreateGrid();
	}
	public bool CreateGrid(){
		grid = new Node[24,24];
		int type = -2;
		Piece p;
		for(int x=0; x<24; x++){
			for(int y=0; y<24; y++){
				p = tablero[x,y];
				if(p!=null){
					if(!(p.name == "Slave1(Clone)" || p.name == "Slave2(Clone)" )){
						type = p.getIDNumber();
					}else{
						type = -2; //no caminable
					}
				}else{ //caminable
					type = -1;
				}
				//construyendo el grid
				grid[x,y] = new Node(type,(Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset,x,y);
			}//for
		}//for
		return true;
	}//creategrid

	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node>();

		int checkX;
		int checkY;
		for(int x=-1; x<=1; x++){
			for(int y=-1; y<=1; y++){
				//if(x!=0 && y!=0 && (x==-1 || y==-1) && (x==-1 || y==1) && (x==1 || y==1) && (x==1 || y==-1)){ //-1 -1 / -1 1 / 1 1 / 1 -1
				if((x==0 && y==1)||(x==-1 && y==0)||(x==1 && y==0)||(x==0 && y==-1)){
					checkX = node.gridX + x;
					checkY = node.gridY + y;
					if((checkX >0 && checkX<23) && (checkY > 0 && checkY<23)){
						neighbours.Add(grid[checkX,checkY]);
					}//fif
				}//fif
			}//ffor
		}//ffor
		return neighbours;
	}//fGetNeighbours

	/* void OnDrawGizmos(){
		if(grid != null){
			foreach(Node n in grid){
				//Gizmos.color = (n.walkable)?Color.blue:Color.red;
				switch(n.type){
					case -1:
						Gizmos.color = Color.blue;
					break;
					case -2:
						Gizmos.color = Color.red;
					break;
					case 0:
						Gizmos.color = Color.yellow;
					break;
					case 1:
						Gizmos.color = Color.green;
					break;
					case 2:
						Gizmos.color = Color.white;
					break;
				}
				if(path != null){
					if(path.Contains(n)){
						Gizmos.color = Color.black;
					}
				}
				Gizmos.DrawCube(n.worldPosition, Vector3.one);
			}
		}
	} */
}
