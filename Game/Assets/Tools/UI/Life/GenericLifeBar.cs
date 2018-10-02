using UnityEngine;
using System.Collections;

public class GenericLifeBar {

    [SerializeField] UI_Generic_LifeBar UI_Bar;

    [SerializeField] int max = 100;

    public GenericLifeBar(UI_Generic_LifeBar ui, int max = 100, int current = 100)
    {
        this.max = max;
        UI_Bar = ui;
    }

    public void Off()
    {
        UI_Bar.Off();
    }

    public void UpdateLife(int life)
    {
        UI_Bar.UpdateBar((life * 100) / max);
    }
}
