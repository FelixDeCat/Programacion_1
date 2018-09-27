using UnityEngine;

public class CheckIsGrounded : MonoBehaviour
{
    public bool test;
    private bool isGrounded;
    protected string nametag;
    protected bool calculate;

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }

        set
        {
            test = value;
            isGrounded = value;
        }
    }

    public void Initialize(string nametag = "Floor")
    {
        this.nametag = nametag;
        calculate = true;
    }
    public void StopCalculate()
    {
        calculate = false;
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (!calculate) return;

        if (other.gameObject.tag == nametag)
        {
            IsGrounded = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!calculate) return;
        IsGrounded = false;
    }
}