using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInventoryA : MonoBehaviour {

	private Animator Anim;
	public bool d_showCategory;
	public Inventory inventory;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0)) {
            if (inventory.showIA){
				inventory.DeployInventory();
                Anim.SetBool("d_showCategory", true);
            }	
            else if (!inventory.showIA){
				Anim.SetBool("d_showCategory", false);
			}
                
        //}
	}
}