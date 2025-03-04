using Meta.WitAi.Utilities;
using Oculus.Interaction.Body.Input;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlanetBehaviour[] Planets;
    private Rigidbody[] bods;
    private ConstantForce[] forces;
    private int planetAmount;
    public float gravConstant;
    void Start()
    {
        Planets = FindObjectsByType<PlanetBehaviour>(FindObjectsSortMode.None);
        planetAmount = Planets.Length;

        bods = new Rigidbody[planetAmount];
        forces = new ConstantForce[planetAmount];
        for (int i = 0; i < planetAmount; i++)
        {
            bods[i] = Planets[i].GetComponent<Rigidbody>();
            forces[i] = Planets[i].GetComponent<ConstantForce>();
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (PlanetBehaviour planet in Planets)
            {
                planet.UpdateSize();
            }
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < planetAmount; i++)
        {
            Vector3 fTot = new Vector3();

            for (int a = 0; a < planetAmount; a++) 
            {
                if (i != a)
                {
                    Vector3 dir = (bods[a].transform.position - bods[i].transform.position).normalized;
                    Vector3 f = (dir * ((gravConstant * bods[i].mass * bods[a].mass) / (Mathf.Pow(Vector3.Distance(bods[i].transform.position, bods[a].transform.position), 2))));
                    fTot += f;
                    
                }
            }
            forces[i].force = fTot / (planetAmount - 1);

        }
    }
}
