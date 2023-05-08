using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Bean _currentCellBeanObject;

    public Bean GetCurrentCellBean()
    {
        if (_currentCellBeanObject != null)
            return _currentCellBeanObject;
        return null;
    }

    public void SetForThisCellBean(Bean newBean) => _currentCellBeanObject = newBean;
}
