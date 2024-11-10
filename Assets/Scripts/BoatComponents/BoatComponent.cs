using UnityEngine;

public class BoatComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected BoatBehaviour BoatScript;
    protected string componentType;

    protected float functionality; //0-100. Represents %. If its 0, its badly damaged and doesent function. If its 100, its brand new.
    public virtual void Start()
    {
        BoatScript = FindFirstObjectByType<BoatBehaviour>();
    }

    public virtual void AffectBoat()
    {

    }
}
