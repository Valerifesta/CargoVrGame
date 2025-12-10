using JetBrains.Annotations;
using Oculus.Interaction;
using Oculus.Interaction.Samples;
using Oculus.VoiceSDK.UX;
using OVR.OpenVR;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Unity.Collections;
using UnityEditor.Search;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    //Rules for chunks:
    //1. Chunks are always supposed to be cubic areas.
    //2. Only objects and events within a certian amount of chunk distance can happen. Exactly how many chunks is needed depends on the object/event itself.
    //   Objects outside of this distance will not be loaded. 
    //3. Dont make the map too big maen.
    public GameObject WorldPivotObj; //Set in inspector
    public float ChunkSize; //
    public int WorldSize; // Amount of chunk rows.
    public GameObject[] Chunks;
    public Vector2[][] ChunkCorners = new Vector2[4][]; //A list in the same order as each chunk. 

    [SerializeField] private GameObject[] _verticalChunks;
    [SerializeField] private GameObject[] _topChunks;

    public int topIndexInsert;
    public int vertIndexInsert;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateChunks();
        /*
        //temp
        BoatBehaviour boat = FindFirstObjectByType<BoatBehaviour>();
        boat.SetStartChunk(boat.startTopIndex, boat.startVerticalIndex);*/ //Moved down at the end of "GenerateChunks()"
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject insertCorrespondingChunk = GetChunk(topIndexInsert, vertIndexInsert);
            if (insertCorrespondingChunk != null)
            {
                Debug.Log("Test DEBUG: " + insertCorrespondingChunk.name);
            }
            
        }
    }

    private void GenerateChunks() //Refers to the primtiive visuals, lists the chunks in order. Visualizes corners. 
    {
        GameObject[] topChunks = new GameObject[WorldSize];
        List<GameObject> verticalChunks = new List<GameObject>();
        for (int i = 0; i < WorldSize; i++)
        {
            GameObject newTopChunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
            

            newTopChunk.transform.localScale *= (ChunkSize / 5);
            if (i == 0)
            {
                newTopChunk.transform.position = Vector3.zero; //offset here in order 
            }
            else
            {
                newTopChunk.transform.position = topChunks[i - 1].transform.position + Vector3.right * ChunkSize * 2;
            }
            topChunks[i] = newTopChunk;
            newTopChunk.transform.parent = WorldPivotObj.transform;
            newTopChunk.name = i + "_" + 0;


            for (int a = 0; a < WorldSize - 1; a++)
            {
                GameObject newVerticalChunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
                newVerticalChunk.transform.localScale *= (ChunkSize / 5);
                newVerticalChunk.transform.position = topChunks[i].transform.position - (Vector3.forward * ChunkSize * (a + 1) * 2);
               
                ///VisualizeCorners(newVerticalChunk.transform.position.x, newVerticalChunk.transform.position.z);
               

                newVerticalChunk.transform.parent = WorldPivotObj.transform;
                newVerticalChunk.name = i + "_" + (a + 1);

                verticalChunks.Add(newVerticalChunk);

            }

            
        }
        //reset all child objects y pos
        Vector3 offset = verticalChunks[verticalChunks.Count - 1].transform.position - WorldPivotObj.transform.position;
        WorldPivotObj.transform.position -= offset / 2;
        
        //puts all in array
        List<GameObject> allChunks = new List<GameObject>(verticalChunks.Count + topChunks.Length);
        _topChunks = topChunks;
        _verticalChunks = verticalChunks.ToArray();

        allChunks.AddRange(topChunks);
        allChunks.AddRange(verticalChunks);

        ChunkCorners = new Vector2[allChunks.Count][]; //Makes space in list. Each chunk will have 4 corners connected to it.

        for (int i = 0; i < allChunks.Count; i++)
        {
            GameObject chunk = allChunks[i];
            chunk.layer = LayerMask.NameToLayer("Chunk");
            GenerateChunkCorners(chunk.transform.position.x, chunk.transform.position.z, i);
        }

        Chunks = allChunks.ToArray();

        //
        BoatBehaviour boat = FindFirstObjectByType<BoatBehaviour>();
        boat.SetStartChunk(boat.startTopIndex, boat.startVerticalIndex);
        //

    }
    public GameObject GetChunk(int topIndex, int verticalIndex) //2025-12-08; When given a vertical index above what each row has, it shifts up to the next row-???? why??? why do this??
    {
        GameObject chunk = new GameObject();
        int allChunkIndex = new int();

        if (verticalIndex >= WorldSize || topIndex >= WorldSize) //If the inputted index is outside of the range of the corresponding listed chunk type. As the map is expanded uniformally and is dependant on the "WorldSize" to decide the maximum number of rows, comparing the index to WorldSize practically checks if its outside of the established size limit.
        {
            print("ERROR! COULD NOT GET VALID CHUNK. INDEX IS OUTSIDE OF SIZE LIMIT. TOP INDEX: " + topIndex + ", VERTICAL INDEX: " + verticalIndex);
        }
        else
        {
            print("CHUNK INDEX IS INSIDE OF WORLD SIZE. ATTEMPTING TO FIND CORRESPONDING CHUNK. TOP INDEX: " + topIndex + ", VERTICAL INDEX: " + verticalIndex);
            allChunkIndex = listIndexesToAllChunkIndex(topIndex, verticalIndex);
            /*
            if (verticalIndex == 0)
            {
                allChunkIndex = topIndex;
            }
            else
            {
                allChunkIndex = _topChunks.Length - 1 + ((_topChunks.Length - 1) * topIndex) + verticalIndex;
            }
            */
            if (allChunkIndex < Chunks.Length && Chunks[allChunkIndex] != null)
            {
                chunk = Chunks[allChunkIndex];
                print("RETRIEVED CHUNK: " + chunk + " ; CORRESPONDING TO INPUTS: TOP INDEX: " + topIndex + ", VERTICAL INDEX: " + verticalIndex);
                return chunk;
            }
            else
            {
                print("ERROR! COULD NOT GET VALID CHUNK. INDEX: " + allChunkIndex);
                return null;
            }
        }



        /*
     if (verticalIndex != 0)
     {
         int allChunkIndex = new int();
         if (topIndex == 0)
         {
             allChunkIndex = ((4 * topIndex) - 1) + verticalIndex;
         }
         else
         {
             allChunkIndex = ((4 * topIndex) - topIndex) + verticalIndex; //for some reason it offsets back at 2-0. It goes to 1-4 instead of 2-1.
         }
         chunk = _verticalChunks[allChunkIndex];
     }
     else
     {
         chunk = _topChunks[topIndex];
     }*/
        return null;
    }
    public Vector2[] GetNeighborChunks(int currentTopIndex, int currentVertIndex) //Based on current chunk, rooted on start chunk
    {
        List<Vector2> verifiedNeighborChunksIndexes = new List<Vector2>();
        Vector2 indexes_U = new Vector2(currentTopIndex + 1, currentVertIndex); //Up
        Vector2 indexes_D = new Vector2(currentTopIndex - 1, currentVertIndex); //Down
        Vector2 indexes_L = new Vector2(currentTopIndex, currentVertIndex - 1); //Left
        Vector2 indexes_R = new Vector2(currentTopIndex, currentVertIndex + 1); //Right

        Vector2[] chunkIndexesToCheck = new Vector2[] { indexes_U, indexes_D, indexes_L, indexes_R };
        for (int i = 0; i < chunkIndexesToCheck.Length; i++)
        {
            Vector2 chunk = chunkIndexesToCheck[i];
            if (chunk.x >= WorldSize || chunk.x < 0 || chunk.y >= WorldSize || chunk.y < 0)
            {
                print("ERROR! UNABLE TO RETRIEVE NEIGHBOR CHUNK. OUTSIDE OF RANGE. INPUTTED INDEX: " + chunk);
            }
            else
            {
                print("RETRIEVED NEIGHBOR CHUNK. INPUTTED INDEX: " + chunk);
                GameObject retrievedNeighborChunk = GetChunk((int)chunk.x, (int)chunk.y);
                verifiedNeighborChunksIndexes.Add(chunk);
            }
        }

        return verifiedNeighborChunksIndexes.ToArray();


        //Check if each is inside of limit. If true, add to neighbor chunks.




    }
    public int listIndexesToAllChunkIndex(int TopIndex, int VertIndex) //Indexes HAS to be verified already.
    {
        int allChunkIndex = new int();

        if (VertIndex == 0)
        {
            allChunkIndex = TopIndex;
        }
        else
        {
            allChunkIndex = _topChunks.Length - 1 + ((_topChunks.Length - 1) * TopIndex) + VertIndex;
        }

        return allChunkIndex;
    }
    public Vector2 GetChunkIndexes(int indexInAllChunks)
    {
        int retrievedTopIndex = new int();
        int retrievedVerticalIndex = new int();

        if (indexInAllChunks > WorldSize)
        {
            for (int i = 0; (indexInAllChunks - retrievedTopIndex * (WorldSize - 1)) >= WorldSize; i++)
            {
                retrievedTopIndex = i;
            }
            retrievedVerticalIndex = indexInAllChunks - retrievedTopIndex * (WorldSize - 1);
            if (retrievedTopIndex != 0)
            {
                retrievedTopIndex -= 1; //Makes it into an index instead of a number of steps.

            }

        }
        else
        {
            retrievedTopIndex = indexInAllChunks;
            retrievedVerticalIndex = 0;
        }

        return new Vector2(retrievedTopIndex, retrievedVerticalIndex);
    }
    
    

    void GenerateChunkCorners(float theoryZ, float theoryX, int indexInAllChunks)
    {
        float dist = ChunkSize;
        Vector2 A = new Vector3(theoryZ - dist, theoryX - dist);
        Vector2 B = new Vector3(theoryZ + dist, theoryX - dist);
        Vector2 C = new Vector3(theoryZ - dist, theoryX + dist);
        Vector2 D = new Vector3(theoryZ + dist, theoryX + dist);

        Vector2[] corners = new Vector2[] { A, B, C, D };
        print(A + "," + B + "," + C + "," + D);
        ChunkCorners[indexInAllChunks] = corners;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        foreach (Vector2 corner in corners) //Does the visual part)
        {
            GameObject newCorner = Instantiate(sphere);
            newCorner.transform.parent = WorldPivotObj.transform;
            newCorner.transform.position = new Vector3(corner.y, newCorner.transform.position.y, corner.x);
            Destroy(sphere);
        }

    }
}
