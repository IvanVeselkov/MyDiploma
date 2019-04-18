using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcSpeedControl : MonoBehaviour
{

    NavMeshAgent _nav;
    // Start is called before the first frame update
    void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "water")
        {
            _nav.speed = 3;
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "water")
        {
            _nav.speed = 3;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "water")
        {
            _nav.speed = 8;
        }
    }
}
