using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomUIBase : MonoBehaviour
{
    [SerializeField] GameObject _tabButtonPanel;

    protected virtual void OnEnable()
    {
        _tabButtonPanel.SetActive(true);
    }

    protected virtual void OnDisable()
    {
        _tabButtonPanel.SetActive(false);
    }
}
