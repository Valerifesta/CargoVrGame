using Oculus.Interaction.DebugTree;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    int numberOfCustomInputs = 4;
    [Header("XZ")]
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode Left;
    public KeyCode Right;

    private Vector2 movementVector;

    public int[] correspondingInputValues;
    private KeyCode[] customInputs;
    private string[] customInputStrings;

    public int walkspeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int[] newCorInputs = new int[numberOfCustomInputs];
        correspondingInputValues = newCorInputs;
        KeyCode[] custInputs = new KeyCode[]{Forward, Backward, Left, Right}; //Has to contain the same amount as number of custom inputs.
        customInputs = custInputs;

        customInputStrings = new string[numberOfCustomInputs];
        for (int i = 0; i < numberOfCustomInputs; i++)
        {
            customInputStrings[i] = customInputs[i].ToString();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            string input = Input.inputString.ToUpper();
            
            for (int i = 0; i < numberOfCustomInputs; i++)
            {
                if (customInputStrings[i] == input)
                {
                    correspondingInputValues[i] = 1;
                    Debug.Log("TRIES TO MATCH ("+input+ ") AND ("+customInputs[i]+"). RESULT: " + correspondingInputValues[i]);
                }

                
            }

            movementVector = new Vector2(correspondingInputValues[0] - correspondingInputValues[1], correspondingInputValues[3] - correspondingInputValues[2]);
            Vector3 newPosVector = transform.position + transform.forward * movementVector.x + transform.right * movementVector.y;
            transform.position += (newPosVector - transform.position) *Time.deltaTime * walkspeed;
            //Movement progress


            for (int i = 0; i < numberOfCustomInputs; i++)
            {
                correspondingInputValues[i] = 0;
                Debug.Log("TRIES TO MATCH ("+input+ ") AND ("+customInputs[i]+"). RESULT: " + correspondingInputValues[i]);

                
            }

            
        }
    }
}
