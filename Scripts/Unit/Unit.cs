using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // 체력
    public int _hp = 0;
    public int _maxHp = 10;
    public float _resHp = 0.3f;        // 자연 치유 hp

    // Rigidbody, Collider
    protected Collider _collider;

    public virtual void Init()
    {
        // 체력 초기화
        _hp = _maxHp;

        // 콜라이더 가져옴
        _collider = GetComponent<Collider>();
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
