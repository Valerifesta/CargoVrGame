using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject MapScreenObject;
    public GameObject MapBoundsObj;
    public GameObject Arrow;
    Texture2D MapTexture;
    Texture2D ClearedTexture;

    public int redSize;

    private int width;
    private int height;

    public Map_Goal goal;

    Color32[] defaultColor;

    public bool isActive;

    private BoatBehaviour _boatScript;

    public int offsetX;
    public int offsetY;

    public int mapPixelScale; //this scale number is arbitrary but 20 kinda works lol. In a way i guess it kinda becomes the size/bounds of the map
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject ChunkWithPlayer;
   
    void Start()
    {
        width = 256;
        height = 256;

        _boatScript = FindFirstObjectByType<BoatBehaviour>();
        
        defaultColor = new Color32[width * height];
        for (int i = 0; i < defaultColor.Length; i++)
        {
            defaultColor[i] = new Color32(0, 0, 0, 255);
        }
        
        ClearedTexture = new Texture2D(width, height);
        MapTexture = new Texture2D(width, height);


       
        
    }

    // Update is called once per frame
    void Update()
    {
        /* This is from the learning tests.
         * 
        float t = Mathf.Sin(Time.time) + 1;
        redSize = Mathf.RoundToInt(Mathf.LerpUnclamped(1, 50, t));
        DotTest();
        */

        if (goal != null)
        {
            
        }
        if (MapBoundsObj != null)
        {
            RaycastHit[] hit = Physics.RaycastAll(MapBoundsObj.transform.position, Vector3.down, 100);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.layer == LayerMask.NameToLayer("Chunk"))
                {
                    ChunkWithPlayer = hit[i].collider.gameObject;
                    break;
                }
                //allChunksInRay[i] = hit[i].collider.gameObject;
            }

        }
    }

    
    public void AddDrawPending(Color32 color, int size, Vector3 markerWorldPos)
    {
        Vector2 dir2D = new Vector2(markerWorldPos.x - _boatScript.transform.position.x, markerWorldPos.z - _boatScript.transform.position.z).normalized;
        float distance = Vector3.Distance(markerWorldPos, _boatScript.transform.position);

        
        Color32[] colors = new Color32[size * size];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }

        //Defines offset of map interest relative to the boat/map center
        offsetX = (int)(dir2D.x * distance * mapPixelScale);
        offsetY = (int)(dir2D.y * distance * mapPixelScale);

        //Mashes it all together, the map interest position will coorespond to its world position relative to the boat/map center
        int x = (int)textureCenter().x - (size / 2) + offsetX;
        int y = (int)textureCenter().y - (size / 2) + offsetY;

        //Makes map interests unable to reach beyond the pixel bounds.
        int xClamped = Mathf.Clamp(x, 0, width - size);
        int yClamped = Mathf.Clamp(y, 0, height - size);
        MapTexture.SetPixels32(xClamped, yClamped, size, size, colors);
    }

    public void LateUpdate()
    {
        DrawPending();
        pendTextureReset();
    }
    void DrawPending() //Fulfills the pending changes to the map. the pending is real.
    {
        MapScreenObject.GetComponent<Renderer>().material.mainTexture = MapTexture;
        MapTexture.Apply();

    }
    
    Vector2 textureCenter() //Returns center of texture, duh.
    {
        return new Vector2(width / 2, height / 2);
    }

    void pendTextureReset() //Adds a texture reset to the maps "pending" changes. 
    {
        MapTexture.SetPixels32(defaultColor);
    }
    void DotTest() //Stuff i messed around with when learning to mannipulate Texture2D properties.
    {

        pendTextureReset();
        //MapScreenObject.GetComponent<Renderer>().material.mainTexture = MapTexture;

        Color32[] colors = new Color32[redSize * redSize];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color32(255, 0, 0, 255);
        }
        MapTexture.SetPixels32((width /2) - (redSize /2), (height /2) - (redSize/2), redSize, redSize, colors); 
        MapTexture.Apply();
        MapScreenObject.GetComponent<Renderer>().material.mainTexture = MapTexture;
        

    }
}
