using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum TileNeighbours
{
    _NN,
    _NW,
    _NE,
    _EE,
    _SS,
    _SW,
    _SE,
    _WW,
    // _NNN,
    // _NNW,
    TOTAL
}

public enum TileLevel
{
    Full,
    Half,
    Quarter,
    Empty
}
public class TileScripts : MonoBehaviour, IPointerClickHandler
{
    [Header("Tile Properties")]
    [SerializeField] private int rowIndex; 
    [SerializeField] private int colIndex;
    [SerializeField] private int resourceValue;
    [SerializeField] private Color tileColor;
    [SerializeField] private bool isRevealed = false;
    [SerializeField] private bool isInitialized = false;
    [SerializeField] private bool hasResource = false;
    [SerializeField] private GameObject[] neighbourArray;
    public TileLevel currentLevel;
    
    // Internal Variables
    private Dictionary<TileNeighbours, GameObject> _tileNeighbours;
    private Dictionary<TileNeighbours, GameObject> _tileFarNeighbours;
    private Image _image;

    public void Awake()
    {
        _tileNeighbours = new Dictionary<TileNeighbours, GameObject>();
        _image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Current Resource Level: "+resourceValue);
        Debug.Log("Current tile Level: "+currentLevel);
    }

    public void BuildNeighbourDictionary()
    {
        for (int i = 0; i < (int) TileNeighbours.TOTAL; i++)
        {
            TileNeighbours neighbourTag = (TileNeighbours) i; 
            switch (neighbourTag)
            {
                case TileNeighbours._NN:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex, colIndex - 1));
                    break;
                case TileNeighbours._NW:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 1, colIndex - 1));
                    break;
                case TileNeighbours._NE:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 1, colIndex - 1));
                    break;
                case TileNeighbours._EE:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 1, colIndex));
                    break;
                case TileNeighbours._SS:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex, colIndex + 1));
                    break;
                case TileNeighbours._SW:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 1, colIndex + 1));
                    break;
                case TileNeighbours._SE:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 1, colIndex + 1));
                    break;
                case TileNeighbours._WW:
                    _tileNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 1, colIndex));
                    break;
            }
        }
        neighbourArray = _tileNeighbours.Values.ToArray();
    }

    public void SetIndex(int rowInd, int colInd)
    {
        rowIndex = rowInd;
        colIndex = colInd;
    }

    public void InitResource(TileLevel level)
    {
        if (hasResource) return; // breaks out of function if true and set a resource 
        
        switch (level)
        {
            case TileLevel.Full:
                _image.color = GridGenerator.Instance.stats.fullColor;
                resourceValue = GridGenerator.Instance.maxResourceValue;
                currentLevel = TileLevel.Full;
                hasResource = true;
                UpdateNeighbours();
                break;
            case TileLevel.Half:
                _image.color = GridGenerator.Instance.stats.halfColor;
                resourceValue = GridGenerator.Instance.maxResourceValue/2;
                currentLevel = TileLevel.Half;
                hasResource = true;
                UpdateNeighbours();
                break;
            case TileLevel.Quarter:
                _image.color = GridGenerator.Instance.stats.quarterColor;
                resourceValue = GridGenerator.Instance.maxResourceValue/4;
                currentLevel = TileLevel.Quarter;
                hasResource = true;
                break;
            case TileLevel.Empty:
                _image.color = GridGenerator.Instance.stats.empty;
                resourceValue = 0;
                currentLevel = TileLevel.Empty;
                hasResource = false;
                break;
        }
    }

    public void UpdateNeighbours()
    {
        Debug.Log("Updating Neighbour current level: "+currentLevel);
        foreach (var neighbourTile in neighbourArray)
        {
            if (neighbourTile == null) continue;
            if (neighbourTile.GetComponent<TileScripts>().currentLevel == TileLevel.Empty)
            {
                TileLevel nLevel = (currentLevel + 1);
                neighbourTile.GetComponent<TileScripts>().InitResource(nLevel);
            }
        }
    }
}
