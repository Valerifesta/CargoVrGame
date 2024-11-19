using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected BoatBehaviour boatScript;

    public virtual void Start()
    {
        boatScript = GetBoatBehaviour();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual BoatBehaviour GetBoatBehaviour()
    {
        return FindFirstObjectByType<BoatBehaviour>();
    }
}
