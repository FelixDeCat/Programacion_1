using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public static Menu instancia;

    private void Awake()
    {
        instancia = this;
        original = img.color;
        Mostrar(false);
    }

    public Text txt_msg;
    public Image img;
    Color original;
    Color transp = new Color(0, 0, 0, 0);

    public GameObject paraOcultar;

    public void Mostrar(bool b = true, string s = "Menu")
    {
        paraOcultar.SetActive(b);
        img.color = b ? original : transp; 

        if (!b) return;
        txt_msg.text = s;
        
    }
}
