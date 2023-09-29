using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{

    public Image hpBar;

    public void SetHP(float hp)
    {
        hpBar.fillAmount = hp;
    }
}
