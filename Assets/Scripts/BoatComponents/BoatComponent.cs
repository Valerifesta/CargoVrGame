using UnityEngine;

public class BoatComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected BoatBehaviour BoatScript;
    protected string componentType;
    protected bool currentlyControlled;
    protected bool tempInputControllers;

    protected float functionality; //0-100. Represents %. If its 0, its badly damaged and doesent function. If its 100, its brand new.
    public GameObject CorrespondingGameObject;
    public virtual void Start()
    {
        BoatScript = FindFirstObjectByType<BoatBehaviour>();
        BoatScript.BoatCompSlots.Add(this);

        tempInputControllers = BoatScript.UseTemporaryInputs;
    }

    public virtual void SyncSettings()
    {
        Debug.Log("Synced settings for " + componentType);

    }

   

    public virtual void AffectBoat()
    {

    }

    public virtual void TryGetBoatCompInput(BoatComponent affectedBoatComp)
    {
        
    }
}
