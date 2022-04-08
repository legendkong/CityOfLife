using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    /// <summary>
    /// Tiles that fall within the same category of objects.
    /// </summary>
    public TileBase[] buildingTiles;
    public enum TileType { BuildingTile, RoadTile, TreeTile };
    public string tileName;
    public TileType tileType;
    public enum BuildingType { HouseBuilding, CompanyBuilding, FactoryBuilding, SustainableBuilding }
    public enum BuildingLevel { Small, Medium, Large }

    public BuildingType buildingType;
    public BuildingLevel buildingLevel;
    public bool isEmptyBuilding = true;
    public Texture2D building;

    public void Start()
    {
    }

}
