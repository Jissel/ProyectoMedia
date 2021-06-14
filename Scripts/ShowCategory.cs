using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCategory : MonoBehaviour {

	private Animator Anim;
	public bool b_showCategory;
	public Inventory inventory;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0)) {
            if (inventory.showC)
                Anim.SetBool("b_showCategory", true);
            else if (!inventory.showC)
                Anim.SetBool("b_showCategory", false);
        //}
	}
}