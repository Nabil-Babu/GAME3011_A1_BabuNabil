using System.Collections;
using System.Collections.Generic;
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
    _WW
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
    
    private Dictionary<TileNeighbours, TileScripts> _tileNeighbours;
    private Image _image;

    public void Awake()
    {
        _tileNeighbours = new Dictionary<TileNeighbours, TileScripts>();
        _image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void BuildNeighbourDictionary()
    {
        // _tileNeighbours.Add(TileNeighbours._NN, null);
        // _tileNeighbours.Add(TileNeighbours._NW, null);
        // _tileNeighbours.Add(TileNeighbours._NE, null);
        // _tileNeighbours.Add(TileNeighbours._EE, null);
        // _tileNeighbours.Add(TileNeighbours._SS, null);
        // _tileNeighbours.Add(TileNeighbours._SE, null);
        // _tileNeighbours.Add(TileNeighbours._SW, null);
        // _tileNeighbours.Add(TileNeighbours._WW, null);
    }

    public void InitTile(int rowInd, int colInd, int rValue, Color initColor)
    {
        rowIndex = rowInd;
        colIndex = colInd;
        resourceValue = rValue;
        _image.color = initColor;
        BuildNeighbourDictionary();
        isInitialized = true; 
    }
}
