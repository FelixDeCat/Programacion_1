using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Entity player;
    public Entity[] enemies;

	void Start ()
    {
        ConstruirPlayer();
        player.Init();

        foreach (var e in enemies) e.Init();

        //enemies.Foreach(x => x.Init());
	}

    void ConstruirPlayer()
    {
        player
            .MoveLink(new HorizontalMove(2))
            .MoveLink(new VerticalMove(2))
            .MoveLink(new Move_Gravity());


        player.rotation = new JoystickRotate();
    }

    void Update()
    {
        player.Refresh();
        foreach (var e in enemies) e.Refresh();
    }

    private void FixedUpdate()
    {
        player.FixedRefresh();
        foreach (var e in enemies) e.FixedRefresh();
    }
}
