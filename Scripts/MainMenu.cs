﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.LoadScene("tablero", LoadSceneMode.Single);
    }

    public void ExitMenu()
    {
        //Debug.Log("QUIT!");
        Application.Quit();

    }


}
