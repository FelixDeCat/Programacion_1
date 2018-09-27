using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIsGrounded2D : CheckIsGrounded
{
    protected override void OnTriggerStay(Collider other)
    {
        //nothing
    }
    protected override void OnTriggerExit(Collider other)
    {
        //nothing
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!calculate) return;
        if (other.gameObject.tag == "Floor")
        {
            IsGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!calculate) return;
        IsGrounded = false;
    }
}
