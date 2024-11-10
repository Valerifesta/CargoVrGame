using UnityEngine;

public class Map_Goal : MapInterest
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void Start()
    {
        base.Start();
        colorRepresent = new Color32(0, 255, 0, 255);
        colorMarkerSize = 20;
    }
    private void Update()
    {
        mapMan.AddDrawPending(colorRepresent, colorMarkerSize, transform.position);
    }

}
