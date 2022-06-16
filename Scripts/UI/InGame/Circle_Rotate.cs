using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle_Rotate : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    bool stop = false;
    public bool Stop
    {
        get { return stop; }
        set { stop = value; }
    }

    void Start()
    {

    }
        
    void Update()
    {
        if(!stop)
            transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void DefaultRotate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
