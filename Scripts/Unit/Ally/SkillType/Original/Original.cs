using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Original : MonoBehaviour
{
    [SerializeField] protected Skill_Info _info;
    protected Ally _owner;

    void Start()
    {
        _owner = GetComponent<Ally>();
        StartCoroutine(CheckOperationMp());
    }

    public void SetInfo(Skill_Info info)
    {
        _info = info;
    }

    IEnumerator CheckOperationMp()
    {
        while(true)
        {
            if (_owner != null)
            {
                if(_owner._maxMp > 0)
                {
                    if (_owner._mp >= _owner._maxMp)
                    {
                        _owner._mp = 0;
                        Operation();
                    }
                }
                else
                {
                    Operation();

                    yield return new WaitForSeconds(1.0f);
                }
            }

            yield return null;
        }
    }

    protected virtual void Operation()
    {

    }
}
