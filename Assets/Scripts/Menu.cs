using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void StartMultiPlayer()
    {
        // TODO: implement
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
