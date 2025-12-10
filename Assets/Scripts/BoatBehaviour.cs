using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Temporary observable data")]
    [SerializeField] private Vector2 FlooredPos;
    [SerializeField] private string CurrentChunkName;
    [SerializeField] private int CurrentChunkTopIndex;
    [SerializeField] private int CurrentChunkVerticalIndex;

    [SerializeField] private Vector2[] NeighboringChunks;

    [Header("Start paramteters")]
    public int startTopIndex;
    public int startVerticalIndex;


    private WorldManager worldMan;





    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Start()
    {
        mapMan = FindFirstObjectByType<MapManager>();
        //worldMan = FindFirstObjectByType<WorldManager>();

        Invoke("ApplyStartStateSettings", 0.1f);
        //SetStartChunk(topVerticalIndex, startVerticalIndex); 

    }

    public void SetStartChunk(int chunkTopIndex, int chunkVertindex) //Places the player inside of the chunk. Only called once between sessions. Needs to happen after chunks are instantiated.
    {
        worldMan = FindFirstObjectByType<WorldManager>();
        GameObject startChunk = worldMan.GetChunk(chunkTopIndex, chunkVertindex);
        transform.position = new Vector3(startChunk.transform.position.x, transform.position.y, startChunk.transform.position.z);
        GetStartChunk();
    }
    void GetStartChunk() //Based on world position. Checks *all* chunks*.
    {
        updateFlooredPos();
        for (int i = 0; i < worldMan.Chunks.Length; i++)
        {

            Vector2[] chunkCorners = worldMan.ChunkCorners[i];
            Vector2 localCornerA = chunkCorners[0];
            Vector2 localCornerD = chunkCorners[3];
            if (localCornerA.x < FlooredPos.x && FlooredPos.x < localCornerD.x && localCornerA.y < FlooredPos.y && FlooredPos.y < localCornerD.y)
            {

                print("Boat has started on chunk: " + worldMan.Chunks[i].name);
                CurrentChunkName = worldMan.Chunks[i].name;
                Vector2 currentChunkIndexes = worldMan.GetChunkIndexes(i);
                CurrentChunkTopIndex = (int)currentChunkIndexes.x;
                CurrentChunkVerticalIndex = (int)currentChunkIndexes.y;

                NeighboringChunks = worldMan.GetNeighborChunks(CurrentChunkTopIndex, CurrentChunkVerticalIndex);

            }
        }
    }

    void TryUpdateCurrentChunk()//Checks neighbor chunk corners and compares it with current world position
    {
        for (int i = 0; i < NeighboringChunks.Length; i++) //Checks if player is within one of the regristered neighbor chunks.
        {
            Vector2 neighbor = NeighboringChunks[i];
            int neighborIndexInAllChunks = worldMan.listIndexesToAllChunkIndex((int)neighbor.x, (int)neighbor.y);
            Vector2[] neighborChunkCorners = worldMan.ChunkCorners[neighborIndexInAllChunks];
            Vector2 neighborLocalCornerA = neighborChunkCorners[0];
            Vector2 neighborLocalCornerD = neighborChunkCorners[3];
            if (neighborLocalCornerA.x < FlooredPos.x && FlooredPos.x < neighborLocalCornerD.x && neighborLocalCornerA.y < FlooredPos.y && FlooredPos.y < neighborLocalCornerD.y)
            {
                print("BOAT HAS ENTERED A NEIGHBORING CHUNK. INDEX: " + neighbor);

                CurrentChunkName = worldMan.Chunks[neighborIndexInAllChunks].name;
                CurrentChunkTopIndex = (int)neighbor.x;
                CurrentChunkVerticalIndex = (int)neighbor.y;

                NeighboringChunks = worldMan.GetNeighborChunks(CurrentChunkTopIndex, CurrentChunkVerticalIndex);
                
                //SetCurrentChunk((int)neighbor.x, (int)neighbor.y);

            }

        }
        //get new neighboring chunks if current chunk chances
    }
    /*
    void SetCurrentChunk(int topIndex, int vertIndex)
    {
        CurrentChunkName = 
    }*/

    void updateFlooredPos()
    {
        FlooredPos = new Vector2(MathF.Round(transform.position.x, 1, MidpointRounding.ToEven), MathF.Round(transform.position.z, 1, MidpointRounding.ToEven));
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
    public void ChangeBoatCompState(BoatComponent[] compsToSync) //Syncs the specified  components.
    {
        for (int i = 0; i < compsToSync.Length; i++)
        {
            if (compsToSync[i] != null)
            {
                compsToSync[i].SyncSettings();
            }
        }
        
    }
    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E)) //Syncs states for toggling control of the ship. temporary meassure, to be replaced by automatic syncing in the future..
        //TODO: Make syncing to be correlated with changing values in the corresponding components.
        {
            BoatComponent[] equippedCompsToSync = new BoatComponent[] { EquippedSeat, EquippedMotor, EquippedSteering };
            ChangeBoatCompState(equippedCompsToSync);
            updateFlooredPos();
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            TryUpdateCurrentChunk();
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
