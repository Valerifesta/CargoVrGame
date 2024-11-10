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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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


       // InvokeRepeating("resetTexture", 0, 0.3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float t = Mathf.Sin(Time.time) + 1;
        redSize = Mathf.RoundToInt(Mathf.LerpUnclamped(1, 50, t));
        DotTest();
        */



        if (goal != null)
        {
            
        }
    }

    
    public void AddDrawPending(Color32 color, int size, Vector3 markerWorldPos)
    {
        

        Vector2 dir2D = new Vector2(markerWorldPos.x - _boatScript.transform.position.x, markerWorldPos.z - _boatScript.transform.position.z).normalized;
        float distance = Vector3.Distance(markerWorldPos, _boatScript.transform.position);

        int offsetScale = 20;
        Color32[] colors = new Color32[size * size];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }

        offsetX = (int)(dir2D.x * distance * offsetScale);
        offsetY = (int)(dir2D.y * distance * offsetScale);

        int x = (int)textureCenter().x - (size / 2) + offsetX;
        int y = (int)textureCenter().y - (size / 2) + offsetY;

        int xClamped = Mathf.Clamp(x, 0, width - size);
        int yClamped = Mathf.Clamp(y, 0, height - size);
        MapTexture.SetPixels32(xClamped, yClamped, size, size, colors);
        


    }

    public void LateUpdate()
    {
        DrawPending();
        resetTexture();
    }
    void DrawPending()
    {
        MapScreenObject.GetComponent<Renderer>().material.mainTexture = MapTexture;
        MapTexture.Apply();

    }
    
    Vector2 textureCenter()
    {
        return new Vector2(width / 2, height / 2);
    }

    void resetTexture()
    {
        MapTexture.SetPixels32(defaultColor);
    }
    void DotTest()
    {

        resetTexture();
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
