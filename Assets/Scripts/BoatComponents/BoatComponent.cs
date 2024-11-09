using UnityEngine;

public class BoatComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected BoatBehaviour BoatScript;
    protected string componentType;
    public virtual void Start()
    {
        BoatScript = FindFirstObjectByType<BoatBehaviour>();
    }

    public virtual void AffectBoat()
    {

    }
}
