using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Properties")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color maxColor;
    [SerializeField] private int maxResourceValue;
    [SerializeField] private int maxGridSize = 64;
    
    private static GridGenerator _instance;
    private GameObject[,] _grid;
    
    private static GridGenerator Instance
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
        GridGenerator[] others = FindObjectsOfType<GridGenerator>();
        foreach (var gridGenerator in others)
        {
            if (gridGenerator != Instance)
            {
                Destroy(gridGenerator.gameObject);
            }
        }
        BuildGrid();
    }

    void BuildGrid()
    {
        _grid = new GameObject[maxGridSize,maxGridSize];
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
                    tile.InitTile(i, j, maxResourceValue, maxColor);
                }
                _grid[i, j] = generatedTile;
                generatedTile.transform.SetParent(transform);
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return _grid;
    }
}
