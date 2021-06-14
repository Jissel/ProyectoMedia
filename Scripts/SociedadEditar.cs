using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SociedadEditar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string Mode; // normal | edit

    public GameObject InputGO, EditButton, ConfirmButton;
    public Text InputValue;
    InputField InputComp;
   
    public CanvasGroup EditCanGroup, ConfirmCanGroup;
    Image InputImage;

    void OnGUI()
    {

        if (Event.current.Equals(Event.KeyboardEvent("[enter]")) || Event.current.Equals(Event.KeyboardEvent("return")))
        {
            if (Mode == "edit")
            {
                EditMode(false);
                //aarb
                print(InputValue.text);
                int pos = int.Parse(gameObject.name.Substring(gameObject.name.Length -1));
                print(pos);
                //agregar nombre en el diccionario
                GameObject.Find("Code").GetComponent<SociedadesController>().SociedadDict[pos].SetName(InputValue.text);
                //aarb
            }
                
        }
    }

    private void Awake()
    {
        InputGO = transform.Find("Input").gameObject;
        InputComp = InputGO.GetComponent<InputField>();
        InputImage = InputGO.GetComponent<Image>();
        InputValue = InputGO.transform.Find("Value").GetComponent<Text>();
        
        EditButton = transform.Find("Edit").gameObject;
        ConfirmButton = transform.Find("Confirm").gameObject;
        EditCanGroup = EditButton.GetComponent<CanvasGroup>();
        ConfirmCanGroup = ConfirmButton.GetComponent<CanvasGroup>();

        SetButtonEvents();
        DefaultMode();
        
    }

    private void SetButtonEvents()
    {
        EditButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            EditMode(true);
        });

        ConfirmButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            EditMode(false);
            //aarb
            int pos = int.Parse(gameObject.name.Substring(gameObject.name.Length -1));
            print(pos);
            //agregar nombre en el diccionario
            GameObject.Find("Code").GetComponent<SociedadesController>().SociedadDict[pos].SetName(InputValue.text);
            //aarb
        });
    }

    public void DefaultMode()
    {
        EditCanGroup.alpha = 0;
        EditCanGroup.blocksRaycasts = false;
        EditCanGroup.interactable = true;

        ConfirmCanGroup.alpha = 0;
        ConfirmCanGroup.blocksRaycasts = false;
        ConfirmCanGroup.interactable = false;

        InputComp.interactable = false;
        InputImage.color = new Color(InputImage.color.r, InputImage.color.g, InputImage.color.b, 0);

        Mode = "normal";
    }

    public void EditMode(bool state)
    {
        ConfirmCanGroup.blocksRaycasts = state;
        ConfirmCanGroup.interactable = state;
        InputComp.interactable = state;
        EditCanGroup.blocksRaycasts = !state;
        EditCanGroup.interactable = !state;

        if (state)
        {
            Mode = "edit";
            EditCanGroup.alpha = 0;
            ConfirmCanGroup.alpha = 1;
            InputImage.color = new Color(InputImage.color.r, InputImage.color.g, InputImage.color.b, 1);
            EventSystem.current.SetSelectedGameObject(InputGO);
        }
        else
        {
            Mode = "normal";
            EditCanGroup.alpha = 1;
            ConfirmCanGroup.alpha = 0;
            InputImage.color = new Color(InputImage.color.r, InputImage.color.g, InputImage.color.b, 0);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Mode == "normal")
        {
            EditCanGroup.alpha = 1;
            EditCanGroup.blocksRaycasts = true;
            EditCanGroup.interactable = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Mode == "normal")
        {
            EditCanGroup.alpha = 0;
            EditCanGroup.blocksRaycasts = false;
            EditCanGroup.interactable = false;
        }
    }


}
