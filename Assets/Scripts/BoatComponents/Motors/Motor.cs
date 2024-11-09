using System;
using UnityEngine;

public class Motor : BoatComponent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float acceleration;
    protected float velocity;
    public float maxAbsVelocity;
    public float drag;
    protected bool isActive;

    protected string motorName;

    float t;
    float anchorVelocity;
    public override void Start()
    {
        base.Start();
        componentType = "Motor";
        BoatScript.EquippedMotor = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMotor();
        }

        if (isActive)
        {
            Accelerate();
            AffectBoat();
        }
        if (velocity != 0.0f && !isActive)
        {
            Deaccelerate();
            AffectBoat();
        }
        else if (velocity == 0.0f && !isActive)
        {
            t = 0.0f;
            anchorVelocity = 0.0f;
        }

    }

    public virtual void Accelerate()
    {
        if (velocity < maxAbsVelocity && velocity > -maxAbsVelocity)
        {
            velocity += acceleration * Time.fixedDeltaTime;
        }
        else
        {
            Debug.Log("Boat has reached max velocity of: " + maxAbsVelocity);
        }
    }
    public virtual void Deaccelerate()
    {
        if (anchorVelocity == 0.0f)
        {
            anchorVelocity = velocity;
        }

        t += (1 / drag) * Time.fixedDeltaTime;
    
     
        velocity = Mathf.Lerp(anchorVelocity, 0.0f, t);
        Debug.Log("Attempting to deaccelerate");
    }
    // Update is called once per frame
    public override void AffectBoat()
    {
        BoatScript.Drive(velocity);
    }

    public virtual void ToggleMotor()
    {
        isActive = !isActive;

        t = 0.0f;
        anchorVelocity = 0.0f;
    }
}
