using Unity.VRTemplate;
using UnityEngine;

public class SteeringWheel : BoatComponent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float SteerValue;
    protected string steeringWheelName;

    public float steerStrength; //is only public bc of testing. Change to protected later.
    public float maxSteerAbsolute; //^
    public override void Start()
    {
        base.Start();
        componentType = "Steering";
        BoatScript.EquippedSteering = this;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0.0f)
        {
            Steer(x * steerStrength * Time.deltaTime); //the Time.deltaTime is only needed for debugging with keyboard.
                                       //Later on, the "x" value inputted into the steering wheel is gonna depend on how much the player rotates the physical wheel in a direction.
        }
    }

    public void Steer(float xMagn)
    {
        TryChangeSteerValue(xMagn);
    }

    public void TryChangeSteerValue(float valueToAdd)
    {
        float nextSteerValue = SteerValue + valueToAdd;

        if (nextSteerValue > maxSteerAbsolute)
        {
            SteerValue = maxSteerAbsolute;
            Debug.Log("Steering max to the right");
        }
        if (nextSteerValue < -maxSteerAbsolute)
        {
            SteerValue = -maxSteerAbsolute;
            Debug.Log("Steering max to the left");

        }

        if (nextSteerValue < maxSteerAbsolute && nextSteerValue > -maxSteerAbsolute)
        {
            SteerValue = nextSteerValue;
        }

        TryRotateWheelVisual();

    }
    public void TryRotateWheelVisual()
    {
        float t = Mathf.InverseLerp(-maxSteerAbsolute, maxSteerAbsolute, SteerValue);
        CorrespondingGameObject.GetComponent<XRKnob>().value = t;
    }

    public void OnWheelGrabBegin(Vector3 grabPos)
    {

    }
    public void OnWheelRelease(Vector3 releasePos)
    {

    }

}
