using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickRotate : Move {

    private int porcentaje;
    private int rotacion;
    private const int VALOR_MAXIMO_ANALOGICO = 1;

    public JoystickRotate()
    {

    }

    Vector3 aux;
    public override Vector3 GetVector(Transform t = null)
    {
        aux = Vector3.zero;

        porcentaje = (int)((Input.GetAxis("EscudoVertical") * 100) / VALOR_MAXIMO_ANALOGICO) * -1;
        rotacion = (porcentaje * 90) / 100;

        if(   !((Input.GetAxis("EscudoVertical")    < 0.3f) && (Input.GetAxis("EscudoVertical") >   -0.3f))  ||
              !((Input.GetAxis("EscudoHorizontal")  < 0.3f) && (Input.GetAxis("EscudoHorizontal") > -0.3f)) )
        aux = new Vector3(0, 0, rotacion);

        if (Input.GetAxis("EscudoHorizontal") != 0)
        {
            t.localScale = new Vector3(Input.GetAxis("EscudoHorizontal") > 0 ? 1 : -1, 1, 1);
        }

        //if (Input.GetAxis("EscudoHorizontal") == 0 && Input.GetAxis("EscudoVertical") == 0)
        //{
        //    aux = new Vector3(0, 0, 0);
        //}
        return aux;
    }

    public override Quaternion GetQuaternion(Transform t = null)
    {
        throw new System.NotImplementedException();
    }

    public override void CanMove(bool canMove)
    {
        throw new System.NotImplementedException();
    }
}
