using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlVideoMouse2 : MonoBehaviour, IPointerExitHandler {

	Animator controlAnim;

	// Use this for initialization
	void Start () {
		controlAnim = GameObject.Find("Controles").GetComponent<Animator>();
	}

	public void OnPointerExit(PointerEventData eventData) {
		controlAnim.SetTrigger("hide");
    }
}
