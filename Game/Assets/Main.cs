using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Main : MonoBehaviour {

    public static Main instancia;

    public bool iscredits;

    public Player player;
    public Boss boss;
    public List<Enemy> enemies;
    public List<EnemyShooter> enemiesShooters;
    public List<Enemy> toremove = new List<Enemy>();
    public List<EnemyShooter> toremoveshooter = new List<EnemyShooter>();

    public Msg_Dead msj;

    

    private void Awake()
    {

        instancia = this;
    }

    void Start ()
    {
        if (iscredits) return;
        ConstruirPlayer();
        player.Init();
        boss.Init();

        foreach (var e in enemies) e.Init();
        foreach (var e in enemies) e.ConfigureToRemove(RemoveEnemy);
        foreach (var e in enemiesShooters) e.Init();
        foreach (var e in enemiesShooters) e.ConfigureToRemove(RemoveEnemy);
    }

    public void RemoveEnemy(Enemy ent)
    {
        toremove.Add(ent);
    }
    public void RemoveEnemy(EnemyShooter ent)
    {
        toremoveshooter.Add(ent);
    }

    void ConstruirPlayer()
    {
        player
            .MoveLink(new HorizontalMove(3))
            .MoveLink(new VerticalMove(3));

        player.rotation = new JoystickRotate();
    }

    void checkListToremove()
    {
        //tengo que hacer esto porque por alguna razon remuevo un enemigo
        //y luego intenta actualizar algo que removi....
        //por eso lo meto al final del update... cuando termine todo... recien ahi lo remuevo
        CheckToRemove(enemies, toremove);
        CheckToRemove(enemiesShooters, toremoveshooter);
    }

    void CheckToRemove<T>(List<T> original, List<T> toremove) {
        if (toremove.Count == 0) return;
        foreach (var r in toremove)
            if (original.Contains(r))
                original.Remove(r);
        toremove.Clear();
    }

    bool m;
    void Update()
    {
        if (iscredits) return;
        player.Refresh();
        boss.Refresh();
        foreach (var e in enemies) e.Refresh();
        foreach (var e in enemiesShooters) e.Refresh();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.instancia.Mostrar(m);
            m = !m;
        }

    }
    

    private void LateUpdate()
    {
        if (iscredits) return;
        checkListToremove();
    }

    private void FixedUpdate()
    {
        if (iscredits) return;
        player.FixedRefresh();
        boss.FixedRefresh();
        foreach (var e in enemies) e.FixedRefresh();
        foreach (var e in enemiesShooters) e.FixedRefresh();
    }

    public void BTN_CargarCreditos() { Scenes.Creditos(); }
    public void BTN_RecargarEscena() { Scenes.Reload(); }
    public void BTN_Salir() { Scenes.Salir(); }
}
