using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Msg_Dead : MonoBehaviour {

    Text text;
    public Text countdown;
    Color original;
    Color transparent = new Color(0,0,0,0);

    bool anim;
    const float MAX_TIMER = 6;
    float timer;

    public void Awake()
    {
        text = gameObject.GetComponent<Text>();
        original = text.color;
        countdown.color = transparent;
        text.color = transparent;
        timer = MAX_TIMER;
    }

    Action callback;
    public void SetEndMessage(Action act)
    {
        callback = act;
        anim = true;
    }

    private void Update()
    {
        if (anim)
        {
            if (timer > 0)
            {
                timer = timer - 1 * Time.deltaTime;
                countdown.text = ((int)timer).ToString();
            }
            else
            {
                anim = false;
                timer = MAX_TIMER;
                callback();
            }
        }
    }

    public void ShowMessage(bool b)
    {
        text.color = b ? original : transparent;
        countdown.color = b ? original : transparent;
    }
}
