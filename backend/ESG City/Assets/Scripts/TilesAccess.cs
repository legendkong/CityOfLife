using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
public class TilesAccess : MonoBehaviour
{
    #region Tree tiles
    public Tile[] trees;
    #endregion

    #region Building tiles
    public Tile[,] buildings;
    public TileBase windmillAnim;
    #endregion

    private void Start()
    {
        trees = new Tile[5];
        buildings = new Tile[11, 3];
        for (int i = 0; i < 10; i++)
        {
            for (int f = 0; f < 3; f++)
            {
                buildings[i, f] = Resources.Load<Tile>("Tiles/building" + i + "_" + f + "");
                Debug.Log(buildings[i,f].name + " successfully loaded");    //debug
            }
        }
        buildings[10, 0] = Resources.Load<Tile>("Tiles/windmill-0");
        Debug.Log(buildings[10, 0].name + " successfully loaded");
        windmillAnim = Resources.Load<AnimatedTile>("Tiles/windmill");
        Debug.Log(windmillAnim.name + " successfully loaded");

        for (int i = 0; i < 5; i++)
        {
            trees[i] = Resources.Load<Tile>("Tiles/tree(" + i + ")");
            Debug.Log(trees[i].name + " successfully loaded");    //debug
        }
    }
}
