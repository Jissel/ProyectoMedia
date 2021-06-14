using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInventory : MonoBehaviour {

	private Animator Anim;
	public bool c_showCategory;
	public Inventory inventory;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		Anim = GetComponent<Animator>();
	}

    
    // Update is called once per frame
    void Update () {
        if (inventory.showI){
			inventory.DeployInventory();
                Anim.SetBool("c_showCategory", true);
        }
				
        else if (!inventory.showI){
			Anim.SetBool("c_showCategory", false);
		}
	}
}