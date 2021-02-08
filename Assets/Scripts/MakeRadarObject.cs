﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRadarObject : MonoBehaviour
{
    public Image Image; 
    // Start is called before the first frame update
    void Start()
    {
        Radar.RegisterRadarObject(gameObject,Image);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Radar.RemoveRadarObject(gameObject);
    }
}
