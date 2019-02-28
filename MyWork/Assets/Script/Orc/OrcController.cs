using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrcController : MonoBehaviour
{
    public GameObject _person;//персонаж в озере
    public GameObject _movePoint;//точка на окружности к которой стремится гоблин

    private NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
        transform.position = _movePoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        nav.enabled = true;
        nav.SetDestination(_movePoint.transform.position);

       

    }
}
