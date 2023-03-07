using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoreLoop : MonoBehaviour
{ // this is all local to the current player
    public float playerNumber;
    public float timeLimit = 120; // seconds
    private float _turnTime;
    private float _timeElapsed; // turns since start

    [SerializeField] private PlayerCharacter playerCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
