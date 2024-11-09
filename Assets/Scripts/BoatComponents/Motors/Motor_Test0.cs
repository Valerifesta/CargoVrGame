using UnityEngine;

public class Motor_Test0 : Motor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        maxAbsVelocity = 1.0f;
        acceleration = 0.1f;
        drag = 20.0f;
        motorName = "Test Motor : Series 0";
    }

   

    // Update is called once per frame
    
}
