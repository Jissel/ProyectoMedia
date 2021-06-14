using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlVideoMouse : MonoBehaviour, IPointerEnterHandler {

	Animator controlAnim;

	// Use this for initialization
	void Start () {
		controlAnim = GameObject.Find("Controles").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerEnter(PointerEventData eventData) {
		controlAnim.SetTrigger("show");
    }
}
