using Unity.Mathematics;
using UnityEngine;

public class BoatBehaviour : MonoBehaviour
{
    public SteeringWheel EquippedSteering;
    public Motor EquippedMotor;

    private MapManager mapMan;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
        mapMan = FindFirstObjectByType<MapManager>();
        
    }

    // Update is called once per frame



    public void Drive(float velocity)
    {
        transform.position += transform.forward.normalized * velocity * Time.deltaTime;
        float rotToAdd = (EquippedSteering.SteerValue / 100) * velocity / 10;
        GameObject arrow = mapMan.Arrow;
        arrow.transform.Rotate(Vector3.up, rotToAdd, Space.Self);
        //mapScreen.transform.rotation = Quaternion.Euler(mapScreen.transform.eulerAngles.x + rotToAdd , mapScreen.transform.eulerAngles.y, mapScreen.transform.eulerAngles.z);

        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotToAdd, transform.eulerAngles.z);
        transform.Rotate(Vector3.up, rotToAdd, Space.World);
    }
}
