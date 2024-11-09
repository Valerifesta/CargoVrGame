using Unity.Mathematics;
using UnityEngine;

public class BoatBehaviour : MonoBehaviour
{
    public SteeringWheel EquippedSteering;
    public Motor EquippedMotor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
   
   

    public void Drive(float velocity)
    {
        transform.position += transform.forward.normalized * velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + ((EquippedSteering.SteerValue/100) * velocity /10), transform.eulerAngles.z);
    }
}
