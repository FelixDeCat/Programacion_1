using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Entity player;

	void Start ()
    {
        ConstruirPlayer();
        player.Init();
	}

    void ConstruirPlayer()
    {
        player
            .MoveLink(new HorizontalMove())
            .MoveLink(new VerticalMove())
            .MoveLink(new Move_Gravity());

        player.rotation = new Rotate(2);
    }

    void Update()
    {
        player.Refresh();
    }

    private void FixedUpdate()
    {
        player.FixedRefresh();
    }
}
