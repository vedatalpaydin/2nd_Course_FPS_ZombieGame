﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{

    public Slider musicSlider;
    private GameObject gController;
    private void Awake()
    {
        gController = GameObject.Find("GameController");
        if(gController == null) return;
        AudioSource music = gController.GetComponent<AudioSource>();
        musicSlider.value = music.volume;
    }

    public void ExitScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void ChangeVolume(float volume)
    {
        if(gController == null) return;
        AudioSource music = gController.GetComponent<AudioSource>();
        music.volume = volume;
    }
}
