using Oculus.Interaction;
using Oculus.Interaction.Samples;
using Oculus.VoiceSDK.UX;
using OVR.OpenVR;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
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

    [SerializeField] private GameObject[] _verticalChunks;
    [SerializeField] private GameObject[] _topChunks;

    public int topIndexInsert;
    public int vertIndexInsert;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateChunks();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(GetChunk(topIndexInsert, vertIndexInsert).name);
        }
    }

    private void InstantiateChunks()
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
                //newVerticalChunk.GetComponent<MeshRenderer>().bounds.size.Set(ChunkSize, ChunkSize, ChunkSize);
                newVerticalChunk.transform.position = topChunks[i].transform.position - (Vector3.forward * ChunkSize * (a+1) *2);
               

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

        foreach (GameObject chunk in allChunks)
        {
            chunk.layer = LayerMask.NameToLayer("Chunk");
        }

        Chunks = allChunks.ToArray();

    }
    GameObject GetChunk(int topIndex, int verticalIndex)
    {
        GameObject chunk = new GameObject();
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
        }
        return chunk;
    }
}
