using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class BoatBehaviour : MonoBehaviour
{
 
    private MapManager mapMan;
    public List<BoatComponent> BoatCompSlots;
    [Header("Game Settings")]
    public bool UseTemporaryInputs;

    [Header("Start State Settings")]
    public bool SeatedByDefault;
    public bool MotorOnByDefault;

    [Header("Current State")]
    public bool IsSeated;
    public bool MotorActive;
    public bool IsSteering;

    [Header("Equipped Boat Components")]
    public SteeringWheel EquippedSteering;
    public Motor EquippedMotor;
    public Seat EquippedSeat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void Start()
    {
        mapMan = FindFirstObjectByType<MapManager>();
        Invoke("ApplyStartStateSettings", 0.1f);
        
    }

    public void ApplyStartStateSettings()
    {
        IsSeated = SeatedByDefault;
        MotorActive = MotorOnByDefault;
        IsSteering = UseTemporaryInputs; //Steering is on by default if the temporary controllers are active.
        foreach (BoatComponent comp in BoatCompSlots)
        {
            comp.SyncSettings();
        }
        Debug.Log("Finished applying start state settings");

    }
    public void ChangeBoatCompState(BoatComponent[] compsToAffect) //The specifically given components.
    {
        for (int i = 0; i < compsToAffect.Length; i++)
        {
            if (compsToAffect[i] != null)
            {
                compsToAffect[i].SyncSettings();
            }
        }
        
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //Syncs states for toggling control of the ship. temporary input.
        {
            BoatComponent[] comps = new BoatComponent[] { EquippedSeat, EquippedMotor };
            ChangeBoatCompState(comps);
        }
    }

    // public void SetStates(bool IsSeated)

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
