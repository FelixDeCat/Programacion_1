using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Generic_LifeBar : MonoBehaviour
{
    public void UpdateBar(float scaleX)
    {
        this.transform.localScale = this.transform.localScale.Only_X(scaleX);
    }

    public void Off()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
