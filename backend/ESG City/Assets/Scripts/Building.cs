using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : TileBase
{
    public enum TileType { BuildingTile, RoadTile, TreeTile };
    public string tileName;
    public TileType tileType;
    public enum BuildingType { HouseBuilding, CompanyBuilding, FactoryBuilding, SustainableBuilding }
    public enum BuildingLevel { Small, Medium, Large }

    public BuildingType buildingType;
    public BuildingLevel buildingLevel;
    public bool isEmptyBuilding = true;
    public Texture2D building;
    public Building(string tileName, BuildingType buildingType)
    {

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
