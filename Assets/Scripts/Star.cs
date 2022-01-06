using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star
{
    public string starName { get; protected set; }
    public int numberOfPlanets { get; protected set; }

    public List<Planet> planetList;

    public Star(string name, int number)
    {
        this.starName = name;
        this.numberOfPlanets = number;

        this.planetList = new List<Planet>();
    }
}
