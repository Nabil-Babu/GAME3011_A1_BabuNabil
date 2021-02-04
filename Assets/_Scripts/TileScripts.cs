using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
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
    _NNN,
    _NNW,
    _NWW,
    _NNE,
    _NEE,
    _EEN,
    _EEE,
    _EES,
    _SSS,
    _SSW,
    _SWW,
    _SSE,
    _SEE,
    _WWS,
    _WWW,
    _WWN,
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
    [SerializeField] private GameObject[] farNeighbourArray;
    public TileLevel currentLevel;
    public Color hiddenColor;
    // Internal Variables
    private Dictionary<TileNeighbours, GameObject> _tileNeighbours;
    private Dictionary<TileNeighbours, GameObject> _tileFarNeighbours;
    private Image _image;

    public void Awake()
    {
        _tileNeighbours = new Dictionary<TileNeighbours, GameObject>();
        _tileFarNeighbours = new Dictionary<TileNeighbours, GameObject>();
        _image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Current Resource Level: "+resourceValue);
        Debug.Log("Current tile Level: "+currentLevel);
        RevealTile();
        RevealNeighbours();
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
                case TileNeighbours._NNN:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex, colIndex - 2));
                    break;
                case TileNeighbours._NNW:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 1, colIndex - 2));
                    break;
                case TileNeighbours._NWW:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 2, colIndex - 2));
                    break;
                case TileNeighbours._NNE:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 1, colIndex - 2));
                    break;
                case TileNeighbours._NEE:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 2, colIndex - 2));
                    break;
                case TileNeighbours._EEN:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 2, colIndex - 1));
                    break;
                case TileNeighbours._EEE:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 2, colIndex));
                    break;
                case TileNeighbours._EES:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 2, colIndex + 1));
                    break;
                case TileNeighbours._SSS:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex, colIndex + 2));
                    break;
                case TileNeighbours._SSW:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 1, colIndex + 2));
                    break;
                case TileNeighbours._SWW:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 2, colIndex + 2));
                    break;
                case TileNeighbours._SSE:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 1, colIndex + 2));
                    break;
                case TileNeighbours._SEE:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex + 2, colIndex + 2));
                    break;
                case TileNeighbours._WWS:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 2, colIndex + 1));
                    break;
                case TileNeighbours._WWW:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 2, colIndex));
                    break;
                case TileNeighbours._WWN:
                    _tileFarNeighbours.Add(neighbourTag, GridGenerator.Instance.GetTile(rowIndex - 2, colIndex - 1));
                    break;
            }
        }
        neighbourArray = _tileNeighbours.Values.ToArray();
        farNeighbourArray = _tileFarNeighbours.Values.ToArray();
    }

    public void SetIndex(int rowInd, int colInd)
    {
        rowIndex = rowInd;
        colIndex = colInd;
    }

    public void SetColor(TileLevel level)
    {
        switch (level)
        {
            case TileLevel.Full:
                tileColor = GridGenerator.Instance.stats.fullColor;
                break;
            case TileLevel.Half:
                tileColor = GridGenerator.Instance.stats.halfColor;
                break;
            case TileLevel.Quarter:
                tileColor = GridGenerator.Instance.stats.quarterColor;
                break;
            case TileLevel.Empty:
                tileColor = GridGenerator.Instance.stats.empty;
                break;
        }
    }

    public void HideTile()
    {
        _image.color = hiddenColor;
        isRevealed = false; 
    }

    public void RevealTile()
    {
        _image.color = tileColor;
        isRevealed = true;
    }

    public void InitResource(TileLevel level)
    {
        if (hasResource) return; // breaks out of function if true and set a resource 
        switch (level)
        {
            case TileLevel.Full:
                SetColor(level);
                resourceValue = GridGenerator.Instance.maxResourceValue;
                currentLevel = TileLevel.Full;
                hasResource = true;
                InitAllNeighbourResources();
                break;
            case TileLevel.Half:
                SetColor(level);
                resourceValue = GridGenerator.Instance.maxResourceValue/2;
                currentLevel = TileLevel.Half;
                hasResource = true;
                break;
            case TileLevel.Quarter:
                SetColor(level);
                resourceValue = GridGenerator.Instance.maxResourceValue/4;
                currentLevel = TileLevel.Quarter;
                hasResource = true;
                break;
            case TileLevel.Empty:
                SetColor(level);
                resourceValue = 0;
                currentLevel = TileLevel.Empty;
                hasResource = false;
                break;
        }
        HideTile();
    }

    public void InitAllNeighbourResources()
    {
        
        foreach (var neighbourTile in neighbourArray)
        {
            if (neighbourTile == null) continue;
            if (neighbourTile.GetComponent<TileScripts>().currentLevel == TileLevel.Empty)
            {
                TileLevel nLevel = (currentLevel + 1);
                neighbourTile.GetComponent<TileScripts>().InitResource(nLevel);
            }
        }

        foreach (var farNeighbourTile in farNeighbourArray)
        {
            if (farNeighbourTile == null) continue;
            if (farNeighbourTile.GetComponent<TileScripts>().currentLevel == TileLevel.Empty)
            {
                TileLevel nLevel = currentLevel + 2;
                farNeighbourTile.GetComponent<TileScripts>().InitResource(nLevel);
            }
        }
    }

    public void RevealNeighbours()
    {
        foreach (var neighbourTile in neighbourArray)
        {
            if (neighbourTile == null) continue;
            neighbourTile.GetComponent<TileScripts>().RevealTile();
        }
    }
}
