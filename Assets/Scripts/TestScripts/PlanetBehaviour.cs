using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]

public class PlanetBehaviour : MonoBehaviour
{
    [SerializeField] private float Mass;
    [SerializeField] private float BaseSize = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = Mass;
        transform.localScale = Vector3.one * Mass / 10;
        if (BaseSize == 0)
        {
            BaseSize = 1;
        }

    }
    public void UpdateSize()
    {
        transform.localScale = Vector3.one * BaseSize * Mass / 10;
    }

    // Update is called once per frame
    
}
