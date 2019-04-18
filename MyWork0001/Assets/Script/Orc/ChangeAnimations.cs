using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimations : MonoBehaviour
{
    public GameObject _movePoint;
    private Animator anim;
    public bool _walk;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _walk = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_walk)
        {
            anim.SetBool("IsRunnig", true);
        }
        else
        {
            anim.SetBool("IsRunnig", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == _movePoint.tag)
        {
            _walk = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == _movePoint.tag)
        {
            _walk = true;
        }
    }
}
