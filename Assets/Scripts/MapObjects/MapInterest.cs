using UnityEngine;

public class MapInterest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected Color32 colorRepresent; //The color of which the interest object is marked in on the sonar map.
    protected int colorMarkerSize;

    protected MapManager mapMan;
    public virtual void Start()
    {
        mapMan = FindFirstObjectByType<MapManager>();
    }

    
}
