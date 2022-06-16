using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round_Attack : Dot_Attack
{
    protected override void Start()
    {
        _time = 0f;
        _maxTime = 1f;
    }
}
