using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    public int numberOfStars = 300;
    public int maximumRadius = 100;
    public int minimumRadius = 0;
    public int seedNumber = 69;

    public float minDistBetweenStars;

    public string[] availablePlanetTypes = { "Barren", "Terran", "Gas Giant" };

    public Dictionary<Star, GameObject> starToObjectMap { get; protected set; }

    public static Galaxy GalaxyInstance;

    void OnEnable()
    {
        GalaxyInstance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        SanityChecks();

        starToObjectMap = new Dictionary<Star, GameObject>();

        Random.InitState(seedNumber);

        int failCount = 0;

        for (int i = 0; i < numberOfStars; i++)
        {
            Star starData = new Star("Star" + i, Random.Range(1, 10));
            Debug.Log("Created Star " + starData.starName + " with " + starData.numberOfPlanets.ToString());

            Vector3 cartPositions = RandomPosition();

            Collider[] positionColliders = Physics.OverlapSphere(cartPositions, minDistBetweenStars);

            if(positionColliders.Length == 0)
            {
                GameObject starGO = CreateStar(starData, cartPositions);
                starToObjectMap.Add(starData, starGO);
                CreatePlanetData(starData);
                failCount = 0;
            }
            else
            {
                i--;
                failCount++;
            }

            if(failCount > numberOfStars)
            {
                Debug.LogError("Could not create stars in the galaxy. The distance between stars is too much!");
                break;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SanityChecks()
    {
        if (minimumRadius > maximumRadius)
        {
            int tempValue = maximumRadius;
            maximumRadius = minimumRadius;
            minimumRadius = tempValue;
        }
    }

    Vector3 RandomPosition()
    {
        float distance = Random.Range(minimumRadius, maximumRadius);
        float angle = Random.Range(0, 2 * Mathf.PI);

        Vector3 cartPositions = new Vector3(distance * Mathf.Cos(angle), 0, distance * Mathf.Sin(angle));
        return cartPositions;
    }

    GameObject CreateStar(Star starData, Vector3 position)
    {
        GameObject starGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        starGO.name = starData.starName;
        starGO.transform.position = position;
        starGO.transform.SetParent(this.transform);
        return starGO;
    }

    void CreatePlanetData(Star starData)
    {
        for( int i = 0; i < starData.numberOfPlanets; i++)
        {
            string name = starData.starName + "Planet" + (starData.planetList.Count + 1).ToString();

            int random = Random.Range(1, 100);
            string type = "";

            if(random < 40)
            {
                type = availablePlanetTypes[0];
            }
            else if ( random <= 40 && random < 50)
            {
                type = availablePlanetTypes[1];
            }
            else
            {
                type = availablePlanetTypes[2];
            }
            Planet planet = new Planet(name, type);
            Debug.Log("Create planet " + name + " of type " + type + " in Solar System " + starData.starName);

            starData.planetList.Add(planet);
        }
    }

    public Star ReturnStarFromGameObject(GameObject go)
    {
        if (starToObjectMap.ContainsValue(go))
        {
            int index = starToObjectMap.Values.ToList().IndexOf(go);
            Star star = starToObjectMap.Keys.ToList()[index];

            return star;
        }
        else
        {
            return null;
        }
    }

    public void DestroyGalaxy()
    {
        while(transform.childCount != 0)
        {
            Transform go = transform.GetChild(0);
            go.SetParent(null);
            Destroy(go.gameObject);
        }
    }
}
