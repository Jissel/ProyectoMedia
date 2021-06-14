using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUp : MonoBehaviour {

	private Animator Anim;
	public bool a_showCategory;
	public Inventory inventory;

	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		Anim = GetComponent<Animator>();
	}
	
	void Update () {

            if (inventory.show) {
                #if UNITY_STANDALONE
                    Anim.SetBool("a_showCategory", true);
                #elif UNITY_ANDROID
                    Anim.SetBool("f_showCategory", true);
                #endif
            } else if (!inventory.show) {
                #if UNITY_STANDALONE
                     Anim.SetBool("a_showCategory", false);
                #elif UNITY_ANDROID
                     Anim.SetBool("f_showCategory", false);
                #endif
            }
    }
}
