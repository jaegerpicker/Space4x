using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public string planetName { get; protected set; }
    public string planetType { get; protected set; }

    public Planet(string name, string type)
    {
        this.planetName = name;
        this.planetType = type;
    }
}
