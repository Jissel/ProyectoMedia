using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{
	public bool walkable;
	public int type;
	public Vector3 worldPosition;
	public int gCost;
	public int hCost;
	public int gridX;
	public int gridY;
	public Node parent;
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY){
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public Node(int _type, Vector3 _worldPos, int _gridX, int _gridY){
		type = _type;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost{
		get{
			return gCost + hCost;
		}
	}

	public override bool Equals(System.Object obj){
		if(obj == null){
			return false;
		}

		// If parameter cannot be cast to Point return false.
        Node p = obj as Node;
        if ((System.Object)p == null){
            return false;
        }

        // Return true if the fields match:
        return (gridX == p.gridX) && (gridY == p.gridY);
    }

	public override string ToString(){
		return "Nodo "+gridX+" "+gridY;
	} 

    public bool Equals(Node p){
        // If parameter is null return false:
        if ((object)p == null){
            return false;
        }

        // Return true if the fields match:
        return (gridX == p.gridX) && (gridY == p.gridY);
    }
}
