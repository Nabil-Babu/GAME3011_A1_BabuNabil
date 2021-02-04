using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


[System.Serializable]
public class GridStats
{
    public Color fullColor;
    public Color halfColor;
    public Color quarterColor;
    public Color empty;
}

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Properties")]
    [SerializeField] private GameObject tilePrefab;

    public GridStats stats;
    public int maxResourceValue;
    [SerializeField] private int maxGridSize = 64;
    
    private static GridGenerator _instance;
    private GameObject[,] _grid;
    private List<GameObject> _gridList;
    
    public static GridGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GridGenerator>();
            }

            return _instance; 
        }
    }

    void Start()
    {
        _grid = new GameObject[maxGridSize,maxGridSize];
        _gridList = new List<GameObject>();
        GridGenerator[] others = FindObjectsOfType<GridGenerator>();
        foreach (var gridGenerator in others)
        {
            if (gridGenerator != Instance)
            {
                Destroy(gridGenerator.gameObject);
            }
        }
        BuildGrid();
        BuildTileNeighbours();
        AddRandomResources();
    }

    void BuildGrid()
    {
        for (int i = 0; i < maxGridSize; i++)
        {
            for (int j = 0; j < maxGridSize; j++)
            {
                Vector3 tilePosition =
                    new Vector3(transform.position.x + (i * tilePrefab.GetComponent<RectTransform>().rect.width), transform.position.y - (j * tilePrefab.GetComponent<RectTransform>().rect.height), 0);
                GameObject generatedTile = Instantiate(tilePrefab);
                generatedTile.transform.position = tilePosition;
                if (generatedTile.TryGetComponent<TileScripts>(out var tile))
                {
                    tile.SetIndex(i, j);
                    tile.InitResource(TileLevel.Empty);
                }

                _grid[i, j] = generatedTile;
                generatedTile.transform.SetParent(transform);
                _gridList.Add(generatedTile);
                //gameObject.SetActive(false);
            }
        }
    }

    public GameObject GetTile(int row, int col)
    {
        if (row >= 0 && col >= 0 && row < maxGridSize && col < maxGridSize)
        {
            return _grid[row, col];
        }
        return null;
    }

    public void BuildTileNeighbours()
    {
        foreach (var tile in _gridList)
        {
            if (tile.TryGetComponent<TileScripts>(out var tileScript))
            {
                tileScript.BuildNeighbourDictionary();
            }
        }
    }

    public void AddRandomResources()
    {
        for (int i = 0; i < 50; i++)
        {
            _gridList[Random.Range(0, _gridList.Count)].GetComponent<TileScripts>().InitResource(TileLevel.Full);
        }
    }
}
