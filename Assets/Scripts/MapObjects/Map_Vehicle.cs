using UnityEngine;

public class Map_Vehicle : MapInterest
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        colorRepresent = new Color32(255, 232, 0, 255);
        colorMarkerSize = 10;
    }

    // Update is called once per frame
    void Update()
    {
        mapMan.AddDrawPending(colorRepresent, colorMarkerSize, transform.position);
    }


}
