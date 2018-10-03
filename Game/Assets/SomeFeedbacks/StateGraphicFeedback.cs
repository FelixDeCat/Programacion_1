using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGraphicFeedback : MonoBehaviour {

    SpriteRenderer sprite;
    private void Awake() {
        gameObject.FindAndLink<SpriteRenderer>(SpriteFound);
        SetStateGraphic();
    }
    private void SpriteFound(SpriteRenderer obj) { sprite = obj; }

    public void SetStateGraphic(Sprite sp = null)
    {
        sprite.sprite = sp;
    }
}
