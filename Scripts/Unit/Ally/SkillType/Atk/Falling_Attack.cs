using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Attack : Skill_Atk
{
    [SerializeField] GameObject _prefabParent;

    ParticleSystem _ps;
    public float _rad = 5f;
    public Transform _damageFloor;

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, _rad);
    }

    protected override void Start()
    {
        Invoke("Disappear", _playTime);

        _ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> psList = new List<ParticleSystem.Particle>();

        ParticlePhysicsExtensions.GetTriggerParticles(_ps, ParticleSystemTriggerEventType.Enter, psList);

        if (psList.Count > 0 && _damageFloor != null)
        {
            Collider[] cols = Physics.OverlapSphere(_damageFloor.position, _rad);

            foreach(Collider col in cols)
            {
                EnemyDamage(col);
            }
        }
    }

    public override void SetTransform(Transform trans)
    {
        _prefabParent.transform.position = trans.position;
    }

    public override void Disappear()
    {
        Destroy(_prefabParent);
    }
}
