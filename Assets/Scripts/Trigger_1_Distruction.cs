using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_1_Distruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovment.enemySpawn += TrigerDistruction;
    }

    private void TrigerDistruction()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
