using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HeartManager : MonoBehaviour
{
    public List<UI_HeartGraphic> hearts_graphic;
    public List<Heart> hearts;

    int counter = 0;
    private const int QUANTITY = 5;

    public Sprite[] sprites;

    private void Start()
    {
        hearts = new List<Heart>();
        for (int i = 0; i < hearts_graphic.Count; i++) { hearts.Add(new Heart()); }
        Configure();
    }

    public void SetLife(int life)
    {
        foreach (var h in hearts) h.ResetConf();

        foreach (var h in hearts)
            for (int i = 0; i < h.values.Length; i++)
                if (life >= h.values[i]) h.confirmers[i] = true;
                else break;

        foreach (var h in hearts)
        {
            // lo hago al revez asi si el primero que encuentra es el 4° por ejemplo, 
            // este solo calcula el indice y corta, los demas ni hace falta que calcule
            for (int i = h.confirmers.Length; i > 0; i--)
            {
                if (h.confirmers[i-1]) {
                    h.indexToDraw = i-1;
                    break;
                }
            }
        }

        SetFrontEnd();
    }

    public void SetFrontEnd()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts_graphic[i].SetImage(sprites[hearts[i].indexToDraw + 1]);// +1 xq tengo un -1, y el -1 esta en el indice 0
        }
    }

    void Configure() // FUNCA BIEN
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            for (int j = 0; j < hearts[i].values.Length; j++)
            {
                counter += QUANTITY;
                hearts[i].values[j] = counter;
            }
        }
        /// al final deveria quedar algo asi como
        /// [5,10,15,20] , [25,30,35,40] , [45,50,55,60] , [65, 70, 75, 80], [85,90,95,100]

    }
}
