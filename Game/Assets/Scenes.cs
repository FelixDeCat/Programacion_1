using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scenes {

    public static void Reload() { Load("Game"); }
    public static void Creditos() { Load("Creditos"); }
    public static void Salir() { Application.Quit(); }
    static void Load(string s) { SceneManager.LoadScene(s); }
}
