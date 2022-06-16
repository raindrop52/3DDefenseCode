using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Penguin : MonoBehaviour
{
    Animator _anim;
    [SerializeField] float _time = 0f;
    [SerializeField] float _loopTime = 8f;
    [SerializeField] bool[] _animArray = new bool[3] { true, false, false };
    string[] _animName = new string[3] { "Idle", "Walk", "Jump" };

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _time += Time.deltaTime;

        if(_time > _loopTime)
        {
            _time = 0f;

            ChangeAnim();
        }
    }

    void ChangeAnim()
    {
        if (_anim != null)
        {
            int randNo = Random.Range(0, 3);
            bool val = false;

            for (int i = 0; i < 3; i++)
            {
                if (randNo == i)
                {
                    val = true;
                    _animArray[i] = true;
                }
                else
                {
                    val = false;
                    _animArray[i] = false;
                }

                _anim.SetBool(_animName[i], val);
            }
        }
    }
}
