using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour {

	private Animator buttonAnim;
	private Menu menu;
    private bool b_showMenu;
    private bool e_showMenu;

    // Use this for initialization
    void Start () {

		buttonAnim = GetComponent<Animator>();
		menu = GameObject.Find("Code").GetComponent<Menu>();
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0)) {
        if (menu.showMenu)
            buttonAnim.SetBool("b_showMenu", true);
        else if (!menu.showMenu)
            buttonAnim.SetBool("b_showMenu", false);

        if (menu.showMenuA)
            buttonAnim.SetBool("e_showMenu", true);
        else if (!menu.showMenuA)
            buttonAnim.SetBool("e_showMenu", false);
        //}
    }
}
