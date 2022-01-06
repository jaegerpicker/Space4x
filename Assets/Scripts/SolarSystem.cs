using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public static SolarSystem SolarSystemInstance;

    private void OnEnable()
    {
        SolarSystemInstance = this;   
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(mouseRay, out hit) && Input.GetMouseButtonDown(0))
        {
            Star star = Galaxy.GalaxyInstance.ReturnStarFromGameObject(hit.transform.gameObject);
            Debug.Log("This star is called " + star.starName + "\n" + "It has " + star.numberOfPlanets.ToString() + " Planets!" );

            Galaxy.GalaxyInstance.DestroyGalaxy();
            CreateSolarSystem(star);
        }
    }

    GameObject CreateSphereObject(Planet planet, Vector3 cartPosition)
    {
        GameObject planetGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        planetGO.name = planet.planetName;
        planetGO.transform.position = cartPosition;
        planetGO.transform.SetParent(this.transform);

        return planetGO;
    }

    public void CreateSolarSystem(Star star)
    {
        GameObject starGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        starGO.transform.position = Vector3.zero;
        starGO.name = star.starName;
        starGO.transform.SetParent(this.transform);

        for(int i = 0; i < star.planetList.Count; i++)
        {
            Planet planet = star.planetList[i];
            float distance = (i + 1) * 5;
            float angle = Random.Range(0, 2 * Mathf.PI);

            Vector3 planetPos = new Vector3(distance * Mathf.Cos(angle), 0, distance * Mathf.Sin(angle));

            CreateSphereObject(planet, planetPos);
        }
    }
}
