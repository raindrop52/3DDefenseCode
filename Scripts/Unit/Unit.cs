using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // ü��
    public int _hp = 0;
    public int _maxHp = 10;
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
