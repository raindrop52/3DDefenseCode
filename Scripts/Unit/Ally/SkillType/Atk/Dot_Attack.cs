using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot_Attack : Skill_Atk
{
    protected float _maxTime = 1f;
    protected float _time = 0f;
    [SerializeField] float _range = 2f;

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, _range);
    }

    protected override void Start()
    {
        base.Start();

        _time = 0f;
        _maxTime = 1f;
    }

    protected override void Update()
    {
        if(_time >= _maxTime)
        {
            _time = 0f;

            Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z);
            List<Enemy> enemies = new List<Enemy>();

            enemies = RoundManager.I.CheckDistanceAllEnemy(pos, _range);

            foreach (Enemy enemy in enemies)
            {
                OnDamage(enemy.GetComponent<Collider>());
            }
        }

        _time += Time.deltaTime;
    }
}
