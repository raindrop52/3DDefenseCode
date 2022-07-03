using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // ü��
    public float _hp = 0f;
    public float _maxHp = 10f;
    public float _resHp = 0.3f;        // �ڿ� ġ�� hp

    // Rigidbody, Collider
    protected Collider _collider;

    public virtual void Init()
    {
        // ü�� �ʱ�ȭ
        _hp = _maxHp;

        // �ݶ��̴� ������
        _collider = GetComponent<Collider>();
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
