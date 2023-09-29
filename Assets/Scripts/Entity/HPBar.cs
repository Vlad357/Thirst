using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : HPBarUI
{
    #region Fields
    public GameObject target;
    public Vector3 offset;
    #endregion
    #region Methods
    #region Unity methods
    private void Update()
    {
        transform.position = target.transform.position + offset;
    }
    #endregion
    #endregion
}
