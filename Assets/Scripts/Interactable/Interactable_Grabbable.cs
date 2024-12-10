using UnityEngine;

public class Interactable_Grabbable : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Collider[] interactionColliders;

    public override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //public virtual void OnGrab()
}
