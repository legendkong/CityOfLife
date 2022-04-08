using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections;

public class TileManager : MonoBehaviour
{

    #region Serialized fields
    [SerializeField] private Tilemap grassMap;
    [SerializeField] private Tilemap roadsMap;
    [SerializeField] private Tilemap riversMap;
    [SerializeField] private Tilemap treeMap;
    [SerializeField] private Tilemap buildingMap;
    [SerializeField] private Tilemap buildingTileBase;
    [SerializeField] private Tilemap sustainableMap;
    [SerializeField] private Tilemap treeTileBase;
    [SerializeField] private GameObject cloudSpawners;
    #endregion

    #region ESG variablse
    [Range(100, 500)]
    public float eMetric;
    [Range(100, 500)]
    public float sMetric;
    [Range(100, 500)]
    public float gMetric;
    private float e;
    private float s;
    private float g;
    #endregion

    #region Data access variables
    private TilesAccess tiles;
    private Spawner spawner;
    private CloudSpawner[] cloudGenerators;

    private bool changed;
    private FileSystemWatcher watcher;
    private string dataPath;
    #endregion

    #region City Map variables
    private enum TreeState { Chopped, Grown, Growing };
    private enum BuildingType
    {
        Building0_0,    //dark blue house
        Building0_1,    //dark blue small company
        Building0_2,    //dark blue big company
        Building1_0,    //green house
        Building1_1,    //green small company
        Building1_2,    //green big company
        Building2_0,    //white house
        Building2_1,    //white small company
        Building2_2,    //white big company
        Building3_0,    //turqoise house
        Building3_1,    //turqoise small company
        Building3_2,    //turqoise big company
        Building4_0,    //orange house
        Building4_1,    //orange small company
        Building4_2,    //orange big company
        Building5_0,    //purple house  
        Building5_1,    //purple small company
        Building5_2,    //purple big company
        Building6_0,    //orange house
        Building6_1,    //orange small company
        Building6_2,    //orange big company
        Building7_0,    //yellow house
        Building7_1,    //yellow small company
        Building7_2,    //yellow big company
        Building8_0,    //factory building 1
        Building8_1,    //factory building 2
        Building8_2,    //factory building 3
        Building9_0,    //house for lease
        Building9_1,    //company building for lease
        Building9_2     //factory building for lease
    }

    int medFactoryCount = 0;
    int bigFactoryCount = 0;
    int totalFactoryCount = 18;

    int houseCount = 0;
    int totalHouseCount = 22;
    #endregion

    private void Start()
    {
        tiles = GetComponent<TilesAccess>();
        spawner = GetComponent<Spawner>();
        cloudGenerators = cloudSpawners.GetComponentsInChildren<CloudSpawner>();
        e = (300 - 100f) / 400f;
        s = (300 - 100f) / 400f;
        g = (300 - 100f) / 400f;
        NewMap();
        UpdateMap();
        NewMap();
        UpdateMap();
    }
    private void OnEnable()
    {
        // Only read text files from Downloads directory
        watcher = new FileSystemWatcher();
        dataPath = @"C:\Users\" + System.Environment.UserName + @"\Downloads";
        watcher.Path = dataPath;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastAccess;
        watcher.Filter = "*.txt";
        Debug.Log("Watching Path: " + watcher.Path + " for " + watcher.Filter + " files.");

        // Watch for changes in LastAccess and LastWrite times, and
        // the renaming of files or directories.

        // Begin watching
        watcher.Created += new FileSystemEventHandler(OnChanged);
        watcher.Deleted += new FileSystemEventHandler(OnChanged);
        watcher.Error += new ErrorEventHandler(OnError);

        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
    }

    private void OnDisable()
    {
        if (watcher != null)
        {
            watcher.Changed -= OnChanged;
            watcher.Dispose();
        }
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        string eventFileName = e.Name.Substring(dataPath.Length + 1);
        Debug.Log("New File Detected: " + eventFileName);
        if (eventFileName.StartsWith("CityOfLife_ESG"))
        {
            changed = true;
        }
    }
    private void OnError(object source, ErrorEventArgs e)
    {
        Debug.Log("Error detected: " + e.GetException().GetType().ToString());
    }


    #region debugging stuff
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            e = (eMetric - 100f) / 400f;
            s = (sMetric - 100f) / 400f;
            g = (gMetric - 100f) / 400f;
            NewMap();
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            e = (eMetric - 100f) / 400f;
            s = (sMetric - 100f) / 400f;
            g = (gMetric - 100f) / 400f;
            UpdateMap();
        }
        if (changed)
        {
            StartCoroutine(GetESGFileData());
            UpdateMap();
            changed = false;
        }
    }
    #endregion
    IEnumerator GetESGFileData()
    {
        #region Get the most recent ESG file in the Downloads folder
        string fileToGet;
        string[] fileMatches = Directory.GetFiles(dataPath, "CityOfLife_ESG*.txt");
        if (fileMatches.Length > 1)
        {
            Debug.Log("More than 1 file detected");
            fileToGet = dataPath + @"\CityOfLife_ESG (" + (fileMatches.Length - 1) + ").txt";
            Debug.Log("Reading From : " + fileToGet);
        }
        else
        {
            fileToGet = dataPath + @"\CityOfLife_ESG.txt";
            Debug.Log("Reading From : " + fileToGet);
        }
        #endregion

        #region Access file and get the relevant file
        StreamReader iStream = new StreamReader(fileToGet);

        while (!iStream.EndOfStream)
        {
            string line = iStream.ReadLine();
            string value = line.Substring(5);
            if (line.StartsWith("m_E"))
            {
                e = (float.Parse(value) - 100f) / 400f;
            }
            else if (line.StartsWith("m_S"))
            {
                s = (float.Parse(value) - 100f) / 400f;
            }
            else if (line.StartsWith("m_G"))
            {
                g = (float.Parse(value) - 100f) / 400f;
            }
        }
        iStream.Close();
        #endregion
        yield return null;
    }
    public void NewMap()
    {
        Debug.Log("New Map");
        InstantiateBuildings();
        InstantiateTrees();
        InstantiateMovables();
    }
        
    public void UpdateMap()
    {
        Debug.Log("Update Map");
        UpdateEnvironment();
        UpdateBuildings();
        UpdateMovables();
    }

    private void InstantiateBuildings()
    {
        buildingMap.ClearAllTiles();
        BoundsInt bounds = buildingTileBase.cellBounds;
        TileBase[] allTiles = buildingTileBase.GetTilesBlock(bounds);
        float chanceOfEmptyBuilding = 0.25f;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                int i, j;
                bool isEmptyBuilding = Random.Range(0f, 1f) <= chanceOfEmptyBuilding;
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int localPos = (new Vector3Int(x, y, (int)buildingTileBase.transform.position.y));
                    if (tile.name == "grass")
                    {
                        if (isEmptyBuilding)
                        {
                            i = 0;
                            j = 9;
                        }
                        else
                        {
                            i = 0;
                            j = Random.Range(0, 8);
                        }
                        buildingMap.SetTile(localPos, tiles.buildings[j, i]);
                    }
                    else if (tile.name == "Bitumen")
                    {
                        if (isEmptyBuilding)
                        {
                            i = 1;
                            j = 9;
                        }
                        else
                        {
                            i = Random.Range(1, 3);
                            j = Random.Range(0, 8);
                        }
                        buildingMap.SetTile(localPos, tiles.buildings[j, i]);
                    }
                    else if (tile.name == "BitumenFactoryTile")
                    {
                        if (isEmptyBuilding)
                        {
                            i = 2;
                            j = 9;
                        }
                        else
                        {
                            i = Random.Range(1, 3);
                            j = 8;
                        }
                        buildingMap.SetTile(localPos, tiles.buildings[j, i]);
                    }
                    else
                    {
                        TileBase tileToInstantiate = isEmptyBuilding ? tiles.buildings[10, 0] : tiles.windmillAnim;
                        sustainableMap.SetTile(localPos, tileToInstantiate);
                    }
                }
            }
        }
    }
    private void InstantiateTrees()
    {
        treeMap.ClearAllTiles();
        BoundsInt bounds = treeTileBase.cellBounds;
        TileBase[] allTiles = treeTileBase.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                int f = Random.Range(0, 5);
                if (f == 0) continue;
                float noiseValue = Mathf.PerlinNoise(x * 0.5f, y * 0.5f);
                TileBase tile = allTiles[x + y * bounds.size.x];
                TileBase tileToInstantiate;
                if (noiseValue > e) tileToInstantiate = tiles.trees[0];
                else if (noiseValue > 0.5f + (Mathf.Abs(noiseValue - e) / 3)  && noiseValue < e) tileToInstantiate = tiles.trees[4];
                else tileToInstantiate = tiles.trees[3];
                if (tile != null)
                {
                    Vector3Int localPos = (new Vector3Int(x, y, (int)treeTileBase.transform.position.y));
                    treeMap.SetTile(localPos, tileToInstantiate);
                }
            }
        }
    }

    private void UpdateEnvironment()
    {

        #region Update river colors
        float waterColorScaling = 1 - e / 0.7f;
        if (waterColorScaling > 1) waterColorScaling -= waterColorScaling % 1;
        //Debug.Log("WATER COLOR SCALING :" + waterColorScaling);
        float r = (255f - (220f * waterColorScaling)) / 255f;
        float g = 1;
        float b = (255f - (220f * waterColorScaling)) / 255f; 
        Color c = new Color(r, g, b);

        for (int n = riversMap.cellBounds.xMin; n < riversMap.cellBounds.xMax; n++)
        {
            for (int p = riversMap.cellBounds.yMin; p < riversMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)riversMap.transform.position.y));
                Vector3 place = riversMap.CellToWorld(localPlace);
                if (riversMap.HasTile(localPlace))
                {
                    //Tile at "place" 
                    riversMap.SetColor(localPlace, c);
                }
            }
        }
        #endregion

        #region Update trees

        #region save states and locations of trees
        BoundsInt bounds = treeMap.cellBounds;
        TileBase[] allTiles = treeMap.GetTilesBlock(bounds);
        Dictionary<Vector3Int, TreeState> tileLocations = new Dictionary<Vector3Int, TreeState>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    TreeState treeStatus;
                    if (tile.name == "tree(0)")
                    {
                        treeStatus = TreeState.Chopped;
                    }
                    else if (tile.name == "tree(3)")
                    {
                        treeStatus = TreeState.Grown;
                    }
                    else
                    {
                        treeStatus = TreeState.Growing;
                    }

                    tileLocations.Add(new Vector3Int(x, y, 0), treeStatus);
                }
            }
        }
        #endregion

        #region clear trees and set them again according to new metrics
        treeMap.ClearAllTiles();
        float chanceOfGrowing;
        float chanceOfChopping;
        if (e <= 0.2f)
        {
            chanceOfGrowing = 0.1f;
            chanceOfChopping = 0.9f;
        }
        else if (e <= 0.3f)
        {
            chanceOfGrowing = 0.2f;
            chanceOfChopping = 0.8f;
        }
        else if (e <= 0.4f)
        {
            chanceOfGrowing = 0.3f;
            chanceOfChopping = 0.7f;
        }
        else if (e < 0.5f)
        {
            chanceOfGrowing = 0.5f;
            chanceOfChopping = 0.6f;
        }
        else if (e < 0.6f)
        {
            chanceOfGrowing = 0.6f;
            chanceOfChopping = 0.4f;
        }
        else if (e < 0.7f)
        {
            chanceOfGrowing = 0.65f;
            chanceOfChopping = 0.3f;
        }
        else if (e < 0.8f)
        {
            chanceOfGrowing = 0.75f;
            chanceOfChopping = 0.2f;
        }
        else
        {
            chanceOfGrowing = 0.9f;
            chanceOfChopping = 0f;
        }

        foreach (KeyValuePair<Vector3Int, TreeState> loc in tileLocations)
        {
            TileBase choppedTree = tiles.trees[0];
            TileBase growingTree = tiles.trees[4];
            TileBase grownTree = tiles.trees[3];

            if (loc.Value == TreeState.Grown)
            {
                if (Random.Range(0f, 1f) < chanceOfChopping)
                {
                    treeMap.SetTile(loc.Key, choppedTree);
                }
                else
                    treeMap.SetTile(loc.Key, grownTree);
            }
            else if (loc.Value == TreeState.Growing)
            {
                if (Random.Range(0f, 1f) < chanceOfGrowing)
                {
                    treeMap.SetTile(loc.Key, grownTree);
                }
                else
                {
                    treeMap.SetTile(loc.Key, growingTree);
                }
            }
            else
            {
                if (Random.Range(0f, 1f) < chanceOfGrowing)
                {
                    treeMap.SetTile(loc.Key, growingTree);
                }
                else
                {
                    treeMap.SetTile(loc.Key, choppedTree);
                }
            }
        }
        #endregion

        #endregion
    }

    private void InstantiateMovables()
    {
        int numHumansToInstantiate = 0;
        int numCarsToInstantiate = 0;
        int numProtestsToInstantiate = 0;
        if (houseCount >= 0 && houseCount < 6)
        {
            numHumansToInstantiate = Random.Range(1, 4);
            numCarsToInstantiate = Random.Range(1, 3);
        }
        else if (houseCount < 10)
        {
            numHumansToInstantiate = Random.Range(4, 9);
            numCarsToInstantiate = Random.Range(3, 5);
        }
        else if (houseCount <= totalHouseCount)
        {
            numHumansToInstantiate = Random.Range(9, 15);
            numCarsToInstantiate = Random.Range(5, 7);
        }
        if (g > 0.1f && g < 0.3f)
        {
            numProtestsToInstantiate = 1;
        }
        else if (g >= 0f && g <= 0.1f)
        {
            numProtestsToInstantiate = 2;
        }

        for (int i = 0; i < numHumansToInstantiate; i++)
        {
            spawner.spawnHuman();
        }
        for (int i = 0; i < numCarsToInstantiate; i++)
        {
            spawner.spawnCar();
        }
        for (int i = 0; i < numProtestsToInstantiate; i++)
        {
            spawner.spawnProtest();
        }
    }
    private void UpdateMovables()
    {
        spawner.ClearAllMovables();
        int numHumansToInstantiate = 0;
        int numCarsToInstantiate = 0;
        int numProtestsToInstantiate = 0;
        if (houseCount >= 0 && houseCount < 6)
        {
            numHumansToInstantiate = Random.Range(1, 4);
            numCarsToInstantiate = Random.Range(1, 3);
        }
        else if (houseCount < 10)
        {
            numHumansToInstantiate = Random.Range(4, 9);
            numCarsToInstantiate = Random.Range(3, 5);
        }
        else if (houseCount <= totalHouseCount)
        {
            numHumansToInstantiate = Random.Range(9, 15);
            numCarsToInstantiate = Random.Range(5, 7);
        }
        if (g > 0.1f && g < 0.3f)
        {
            numProtestsToInstantiate = 1;
        }
        else if (g >= 0f && g <= 0.1f)
        {
            numProtestsToInstantiate = 2;
        }

        for (int i = 0; i < numHumansToInstantiate; i++)
        {
            spawner.spawnHuman();
        }
        for (int i = 0; i < numCarsToInstantiate; i++)
        {
            spawner.spawnCar();
        }
        for (int i = 0; i < numProtestsToInstantiate; i++)
        {
            spawner.spawnProtest();
        }
    }

    private void UpdateBuildings()
    {
        medFactoryCount = 0;
        bigFactoryCount = 0;
        totalFactoryCount = 18;

        houseCount = 0;
        totalHouseCount = 22;
        #region hardcoded probabilities at each level of each metric
        float chanceOfCompanyExpansion;
        float chanceOfCompanyDropping;
        if (g <= 0.2f)
        {
            chanceOfCompanyExpansion = 0.3f;
            chanceOfCompanyDropping = 0.9f;
        }
        else if (g <= 0.3f)
        {
            chanceOfCompanyExpansion = 0.3f;
            chanceOfCompanyDropping = 0.8f;
        }
        else if (g <= 0.4f)
        {
            chanceOfCompanyExpansion = 0.4f;
            chanceOfCompanyDropping = 0.7f;
        }
        else if (g < 0.5f)
        {
            chanceOfCompanyExpansion = 0.5f;
            chanceOfCompanyDropping = 0.6f;
        }
        else if (g < 0.6f)
        {
            chanceOfCompanyExpansion = 0.6f;
            chanceOfCompanyDropping = 0.5f;
        }
        else if (g < 0.7f)
        {
            chanceOfCompanyExpansion = 0.65f;
            chanceOfCompanyDropping = 0.6f;
        }
        else if (g < 0.8f)
        {
            chanceOfCompanyExpansion = 0.75f;
            chanceOfCompanyDropping = 0.5f;
        }
        else
        {
            chanceOfCompanyExpansion = 0.9f;
            chanceOfCompanyDropping = 0.5f;
        }

        float chanceOfMovingOut;
        float chanceOfMovingIn;
        if (s <= 0.2f)
        {
            chanceOfMovingIn = 0.3f;
            chanceOfMovingOut = 0.9f;
        }
        else if (s <= 0.3f)
        {
            chanceOfMovingIn = 0.3f;
            chanceOfMovingOut = 0.8f;
        }
        else if (s <= 0.4f)
        {
            chanceOfMovingIn = 0.4f;
            chanceOfMovingOut = 0.7f;
        }
        else if (s < 0.5f)
        {
            chanceOfMovingIn = 0.5f;
            chanceOfMovingOut = 0.6f;
        }
        else if (s < 0.6f)
        {
            chanceOfMovingIn = 0.6f;
            chanceOfMovingOut = 0.4f;
        }
        else if (s < 0.7f)
        {
            chanceOfMovingIn = 0.65f;
            chanceOfMovingOut = 0.3f;
        }
        else if (s < 0.8f)
        {
            chanceOfMovingIn = 0.75f;
            chanceOfMovingOut = 0.2f;
        }
        else
        {
            chanceOfMovingIn = 0.9f;
            chanceOfMovingOut = 0.2f;
        }

        float chanceOfFactoryUpgrade;
        float chanceOfFactoryDowngrade;
        float chanceOfWindmillWorking;
        float chanceOfWindmillBreaking;
        if (e <= 0.2f)
        {
            chanceOfFactoryUpgrade = 0.9f;
            chanceOfFactoryDowngrade = 0.3f;
            chanceOfWindmillWorking = 0;
            chanceOfWindmillBreaking = 1f;
        }
        else if (e <= 0.3f)
        {
            chanceOfFactoryUpgrade = 0.8f;
            chanceOfFactoryDowngrade = 0.3f;
            chanceOfWindmillWorking = 0;
            chanceOfWindmillBreaking = 1f;
        }
        else if (e <= 0.4f)
        {
            chanceOfFactoryUpgrade = 0.5f;
            chanceOfFactoryDowngrade = 0.4f;
            chanceOfWindmillWorking = 0;
            chanceOfWindmillBreaking = 1f;
        }
        else if (e < 0.5f)
        {
            chanceOfFactoryUpgrade =  0.3f;
            chanceOfFactoryDowngrade = 0.5f;
            chanceOfWindmillWorking = 0;
            chanceOfWindmillBreaking = 1f;
        }
        else if (e < 0.6f)
        {
            chanceOfFactoryUpgrade =  0.2f;
            chanceOfFactoryDowngrade = 0.6f;
            chanceOfWindmillWorking = 0;
            chanceOfWindmillBreaking = 0.5f;
        }
        else if (e < 0.7f)
        {
            chanceOfFactoryUpgrade = 0.1f;
            chanceOfFactoryDowngrade =  0.7f;
            chanceOfWindmillWorking = 0.6f;
            chanceOfWindmillBreaking = 0.3f;
        }
        else if (e < 0.8f)
        {
            chanceOfFactoryUpgrade = 0.05f;
            chanceOfFactoryDowngrade = 0.85f;
            chanceOfWindmillWorking = 0.7f;
            chanceOfWindmillBreaking = 0.2f;
        }
        else
        {
            chanceOfFactoryUpgrade = 0.05f;
            chanceOfFactoryDowngrade = 1f;
            chanceOfWindmillWorking = 0.9f;
            chanceOfWindmillBreaking = 0f;
        }
        #endregion

        #region Save states and locations of buildings
        BoundsInt bBounds = buildingMap.cellBounds;
        TileBase[] bTiles = buildingMap.GetTilesBlock(bBounds);
        Dictionary<Vector3Int, BuildingType> tileLocations = new Dictionary<Vector3Int, BuildingType>();
        for (int x = 0; x < bBounds.size.x; x++)
        {
            for (int y = 0; y < bBounds.size.y; y++)
            {
                TileBase tile = bTiles[x + y * bBounds.size.x];
                if (tile != null)
                {
                    string tileName = char.ToUpper(tile.name[0]) + tile.name.Substring(1);
                    BuildingType bType = (BuildingType)System.Enum.Parse(typeof(BuildingType), tileName);   //parse the name of the building into its respective enum
                    tileLocations.Add(new Vector3Int(x, y, 0), bType);
                }
            }
        }
        #endregion

        #region Save location and states of windmills
        BoundsInt susBounds = sustainableMap.cellBounds;
        TileBase[] susTiles = sustainableMap.GetTilesBlock(susBounds);
        Dictionary<Vector3Int, bool> susTileLocations = new Dictionary<Vector3Int, bool>();

        for (int x = 0; x < susBounds.size.x; x++)
        {
            for (int y = 0; y < susBounds.size.y; y++)
            {
                TileBase tile = susTiles[x + y * susBounds.size.x];
                if (tile != null)
                {
                    bool isWindmillActive = tile.name != "windmill-0";
                    susTileLocations.Add(new Vector3Int(x, y, 0), isWindmillActive);
                }
            }
        }
        #endregion

        #region Clear windmills and set them again according to new metrics
        sustainableMap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, bool> loc in susTileLocations)
        {   
            if (Random.Range(0f, 1f) <= chanceOfWindmillWorking)
            {
                sustainableMap.SetTile(loc.Key, tiles.windmillAnim);
            }
            else if (Random.Range(0f, 1f) <= chanceOfWindmillBreaking)
            {
                sustainableMap.SetTile(loc.Key, tiles.buildings[10, 0]);
            }
            else
            {
                TileBase tileToInstantiate = loc.Value ? tiles.windmillAnim : tiles.buildings[10, 0];
                sustainableMap.SetTile(loc.Key, tileToInstantiate);
            }
        }
        #endregion

        #region Clear buildings and set them again according to new metrics
        buildingMap.ClearAllTiles();
        foreach (KeyValuePair<Vector3Int, BuildingType> loc in tileLocations)
        {
            int buildingColor = (int)char.GetNumericValue(loc.Value.ToString()[loc.Value.ToString().Length - 3]);
            int buildingType = (int)char.GetNumericValue(loc.Value.ToString()[loc.Value.ToString().Length - 1]);

            if (buildingColor == 8 || (buildingColor == 9 && buildingType == 2))
            {
                if (Random.Range(0f, 1f) <= chanceOfFactoryUpgrade)
                {
                    if (buildingColor == 9)
                    {
                        buildingColor--;
                        buildingType = 1;
                        buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]);   
                    }
                    else
                    {
                        if (buildingType != 2) buildingType++;
                        buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]); 
                    }
                }
                else
                {
                    if (Random.Range(0f, 1f) <= chanceOfFactoryDowngrade)
                    {
                        if (buildingColor != 9)
                        {
                            switch (buildingType)
                            {
                                case 1:
                                    {
                                        //REMOVE FACTORY AND ADD 4 LEASE SIGN
                                        buildingColor = 9;
                                        buildingType = 2;
                                        break;
                                    }
                                case 2:
                                    {
                                        //CHANGE BIG FACTORY TO MED FACTORY
                                        buildingColor = 8;
                                        buildingType = 1;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                    }
                    buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]); //INSTANTIATE ORIGINAL BUILDING AT LOCATION
                }
                #region Count up the factories
                if (buildingColor == 8)
                {
                    switch (buildingType)
                    {
                        case 1:
                            {
                                medFactoryCount++;
                                break;
                            }
                        case 2:
                            {
                                bigFactoryCount++;
                                break;
                            }
                        default:
                            break;
                    }
                }
                #endregion
            }
            else if ((buildingType == 1 || buildingType == 2))    //IF BUILDING IS A COMPANY SLOT BUILDING
            {
                if (Random.Range(0f, 1f) <= chanceOfCompanyExpansion)
                {
                    if (buildingColor == 9) //IF BUILDING SLOT IS EMPTY
                    {
                        if (buildingType == 1)
                        {
                            buildingMap.SetTile(loc.Key, tiles.buildings[Random.Range(0, 8), 1]);   //INSTANTIATE RANDOM SMALL COMPANY BUILDING
                        }
                    }
                    else
                    {
                        if (buildingType == 1)  //IF BUILDING SLOT IS A SMALL COMPANY
                        {
                            buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, 2]);
                        }
                        else if (buildingType == 2)
                        {
                            buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, 2]);
                        }
                    }
                }
                else
                {
                    if (Random.Range(0f, 1f) <= chanceOfCompanyDropping)
                    {
                        if (buildingColor == 9) //IF BUILDING SLOT IS ALREADY EMPTY
                        {
                            if (buildingType == 1)
                            {
                                buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, 1]);    //INSTANTIATE FOR LEASE SIGN AND CONT
                            }
                        }
                        else
                        {
                            if (buildingType == 2)
                            {
                                buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType - 1]);
                            }
                            else
                            {
                                buildingMap.SetTile(loc.Key, tiles.buildings[9, 1]);
                            }
                        }
                    }
                    else
                    {
                        buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]);
                    }
                }
            }
            else if ((buildingColor != 8) && buildingType == 0) //IS NOT A FACTORY AND IS A HOUSE
            {
                if (Random.Range(0f, 1f) < chanceOfMovingIn)
                {
                    if (buildingColor == 9)
                    {
                        buildingType = 0;
                        buildingColor = Random.Range(0, 8);
                        buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]);   //INSTANTIATE RANDOM SMALL HOUSE 
                    }
                    else
                    {
                        buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]); //INSTANTIATE ORIGINAL BUILDING AT LOCATION
                    }
                }
                else
                {
                    if (Random.Range(0f, 1f) < chanceOfMovingOut)
                    {
                        if (buildingColor != 9)
                        {
                            buildingType = 0;
                            buildingColor = 9;
                            buildingMap.SetTile(loc.Key, tiles.buildings[9, 0]);   //REMOVE HOUSE AND ADD 4 LEASE SIGN
                        }
                        else
                        {
                            buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]); //INSTANTIATE ORIGINAL BUILDING AT LOCATION
                        }
                    }
                    else
                    {
                        buildingMap.SetTile(loc.Key, tiles.buildings[buildingColor, buildingType]); //INSTANTIATE ORIGINAL BUILDING AT LOCATION
                    }
                }
                if (buildingColor != 9) houseCount++;
            }
        }
        #endregion

        float percentageFactories = (float)(medFactoryCount + bigFactoryCount) / totalFactoryCount;
        for (int i = 0; i < cloudGenerators.Length; i++)
        {
            cloudGenerators[i].Enable(percentageFactories != 0);
            if (medFactoryCount / totalFactoryCount > bigFactoryCount / totalFactoryCount)
            {
                cloudGenerators[i]._cloudSpawnSpeed = percentageFactories * 1.4f;
            }
            else
            {
                cloudGenerators[i]._cloudSpawnSpeed = percentageFactories * 2.5f;
            }
        }
    }
}
