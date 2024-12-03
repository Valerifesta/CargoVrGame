using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Grabbable_Wheel : Interactable_Grabbable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void Start()
    {
        base.Start();
        GetColliders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetColliders()
    {
        
        GameObject wheelObj = boatScript.EquippedSteering.CorrespondingGameObject;
        GameObject[] children = new GameObject[wheelObj.transform.childCount];
        
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = wheelObj.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < wheelObj.transform.childCount; i++)
        {
            if (children[i].tag == "ColliderHolder")
            {
                interactionColliders = children[i].GetComponentsInChildren<Collider>();
            }
        }
    }
}
