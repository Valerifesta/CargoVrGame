using UnityEngine;

public class Seat : BoatComponent
{
    protected string seatName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        componentType = "Seat";
        BoatScript.EquippedSeat = this;
    }

    public override void SyncSettings()
    {
        base.SyncSettings();
        currentlyControlled = BoatScript.IsSeated; //Makes player be able to control all components on start.
                                                          //Will need to be changed to instead depened on speciifc conditions, depending on the component in question.
    }
}
