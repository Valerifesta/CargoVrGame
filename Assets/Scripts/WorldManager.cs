using Oculus.Interaction;
using Oculus.Interaction.Samples;
using Oculus.VoiceSDK.UX;
using OVR.OpenVR;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    //Rules for chunks:
    //1. Chunks are always supposed to be cubic areas.
    //2. Only objects and events within a certian amount of chunk distance can happen. Exactly how many chunks is needed depends on the object/event itself.
    //   Objects outside of this distance will not be loaded. 
    //3. Dont make the map too big maen.
    public float ChunkSize; //
    public int WorldSize; //How many more chunk "circuferances" exist around the base chunk.
    public GameObject[] Chunks;

    private List<GameObject> previousSegmentChunks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateChunks();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InstantiateChunks()
    {
        GameObject[] topChunks = new GameObject[WorldSize];
        for (int i = 0; i < WorldSize; i++)
        {
            GameObject newTopChunk = GameObject.CreatePrimitive(PrimitiveType.Plane);

            newTopChunk.transform.localScale *= (ChunkSize / 5);
            if (i == 0)
            {
                newTopChunk.transform.position = Vector3.zero;
            }
            else
            {
                newTopChunk.transform.position = topChunks[i - 1].transform.position + Vector3.right * ChunkSize *2;
            }
            topChunks[i] = newTopChunk;

            for (int a = 0; a < WorldSize - 1; a++)
            {
                GameObject newVerticalChunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
                newVerticalChunk.transform.localScale *= (ChunkSize / 5);
                //newVerticalChunk.GetComponent<MeshRenderer>().bounds.size.Set(ChunkSize, ChunkSize, ChunkSize);
                newVerticalChunk.transform.position = topChunks[i].transform.position - (Vector3.forward * ChunkSize * (a+1) *2);
            }
        }
        /*
        GameObject centerChunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
        centerChunk.transform.localScale *= ChunkSize;

        for (int i = 0; i < WorldSize; i++)
        {
            if (i == 0)
            {
                GameObject upperCore = GameObject.CreatePrimitive(PrimitiveType.Plane);
                upperCore.transform.localScale *= ChunkSize;
                upperCore.transform.position = centerChunk.transform.position + Vector3.forward * ChunkSize;

                GameObject lowerCore = GameObject.CreatePrimitive(PrimitiveType.Plane);
                lowerCore.transform.localScale *= ChunkSize;
                lowerCore.transform.position = centerChunk.transform.position -Vector3.forward * ChunkSize;

                GameObject rightCore = GameObject.CreatePrimitive(PrimitiveType.Plane);
                rightCore.transform.localScale *= ChunkSize;
                rightCore.transform.position = centerChunk.transform.position + Vector3.right * ChunkSize;

                GameObject leftCore = GameObject.CreatePrimitive(PrimitiveType.Plane);
                leftCore.transform.localScale *= ChunkSize;
                leftCore.transform.position = centerChunk.transform.position -Vector3.right * ChunkSize;

                GameObject[] coreChunks = new GameObject[4] { upperCore, lowerCore, rightCore, leftCore };
                previousSegmentChunks.AddRange(coreChunks);

            }
            else
            {
                List<GameObject> newSegmentChunks = new List<GameObject>();
                foreach (GameObject chunk in previousSegmentChunks)
                {
                    Vector3 diff = centerChunk.transform.position - chunk.transform.position; //math might be wrong

                    
                    if (diff.x != 0 && diff.z != 0) //corner chunk)
                    {

                    }
                    else
                    {
                        GameObject newChunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        newChunk.transform.localScale *= ChunkSize;
                        newChunk.transform.position = chunk.transform.position + diff.normalized * ChunkSize;//math might be wrong

                    }
                }
                
            }
        }

        

        */

        /*
        GameObject[] chunksX = new GameObject[WorldSize];
        GameObject[] chunksZ = new GameObject[(WorldSize ^ 2) - WorldSize];

        for (int i = 0; i < WorldSize; i++)
        {
            GameObject newChunkX = GameObject.CreatePrimitive(PrimitiveType.Plane);
            newChunkX.transform.localScale *= ChunkSize;

            //set a base position for the first chunk X
            if (i != 0)
            {
                newChunkX.transform.position = chunksX[i - 1].transform.position + (Vector3.right * ChunkSize);
            }
            chunksX[i] = newChunkX;

            for (int a = 0; a < WorldSize; a++)
            {
                GameObject newChunkZ = GameObject.CreatePrimitive(PrimitiveType.Plane);
                newChunkZ.transform.localScale *= ChunkSize;

                if (a != 0)
                {
                    newChunkZ.transform.position = chunksZ[a - 1].transform.position + (Vector3.forward * ChunkSize);
                }
                chunksZ[a] = newChunkZ;
            }


        }*/

    }
}
