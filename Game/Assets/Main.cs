using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Main : MonoBehaviour {

    public static Main instancia;

    public Player player;
    public List<Enemy> enemies;
    public List<Enemy> toremove = new List<Enemy>();

    private void Awake()
    {
        instancia = this;
    }

    void Start ()
    {
        ConstruirPlayer();
        player.Init();

        foreach (var e in enemies) e.Init();
        foreach (var e in enemies) e.ConfigureToRemove(RemoveEnemy);
    }

    public void RemoveEnemy(Enemy ent)
    {
        toremove.Add(ent);
    }

    void ConstruirPlayer()
    {
        player
            .MoveLink(new HorizontalMove(2))
            .MoveLink(new VerticalMove(2))
            .MoveLink(new Move_Gravity());


        player.rotation = new JoystickRotate();
    }

    void checkListToremove()
    {
        //tengo que hacer esto porque por alguna razon remuevo un enemigo
        //y luego intenta actualizar algo que removi....
        //por eso lo meto al final del update... cuando termine todo... recien ahi lo remuevo
        if (toremove.Count == 0) return;

        foreach (var r in toremove)
        {
            if (enemies.Contains(r))
            {
                enemies.Remove(r);
            }
        }
        toremove.Clear();
    }

    void Update()
    {
        player.Refresh();
        foreach (var e in enemies) e.Refresh();
        
    }

    private void LateUpdate()
    {
        checkListToremove();
    }

    private void FixedUpdate()
    {
        player.FixedRefresh();
        foreach (var e in enemies) e.FixedRefresh();
    }
}
