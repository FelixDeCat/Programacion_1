using UnityEngine.UI;
using UnityEngine;

public class Heart
{
    public int indexToDraw = -1;

    public int[] values = new int[4];
    public bool[] confirmers = new bool[4];

    public void ResetConf()
    {
        indexToDraw = -1;
        for (int i = 0; i < confirmers.Length; i++) confirmers[i] = false;
    }

    public void SetValues()
    {

    }
}
