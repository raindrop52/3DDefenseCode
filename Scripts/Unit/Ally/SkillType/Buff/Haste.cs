using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : Skill_Buff
{
    public bool _isParty = false;
    [SerializeField] List<Ally> _enableCreatures = new List<Ally>();

    protected override void Start()
    {
        if(_playTime > 0)
            Invoke("Disappear", _playTime);
    }

    protected override void InBuffTypeEvent(GameObject obj)
    {
        if (_owner == null)
            return;

        float value = _value + (_owner._level * _calcValue);
        float percent = value / 100.0f;

        Ally ally = obj.GetComponent<Ally>();
        if (ally != null)
        {
            if (_isParty == true)
            {
                if (ally._tmpSpeedParty > 0)
                {
                    if(ally._tmpSpeedParty < percent)
                        ally._tmpSpeedParty = percent;
                }
                else
                    ally._tmpSpeedParty = percent;
            }
            else
            {
                if (ally._tmpSpeedOnly > 0)
                {
                    if(ally._tmpSpeedOnly < percent)
                        ally._tmpSpeedOnly = percent;
                }
                else
                    ally._tmpSpeedOnly = percent;
            }

            _enableCreatures.Add(ally);
        }
    }

    public override void Disappear()
    {
        foreach(Ally ally in _enableCreatures)
        {
            if (_isParty == true)
            {
                ally._tmpSpeedParty = 0;
            }
            else
            {
                ally._tmpSpeedOnly = 0;
            }
        }

        base.Disappear();
    }
}
