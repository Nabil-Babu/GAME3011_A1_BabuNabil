using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class GameStateController : MonoBehaviour
{
    [SerializeField] int totalGatheredResource = 0;
    private static GameStateController _instance;

    public static GameStateController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameStateController>(); 
            }
            return _instance;
        }
    }
    private bool _gameEnabled;
    
    public bool scanningMode;
    public bool extractionMode;
    public int scanAttempts = 6;
    public int extractionAttempts = 3;
    public GameObject miniGameWindow;
    public TMP_Text messageTextUI;
    public TMP_Text resourceTextUI;
    public Button scanButton;
    public Button extractButton; 
    public Button resetButton; 

    private string _messageOutput;

    public void Start()
    {
        EnableExtractionMode();
        resetButton.interactable = false;
    }

    public void ToggleMiniGame()
    {
        _gameEnabled = !_gameEnabled;
        miniGameWindow.SetActive(_gameEnabled);
    }

    public void EnableScanMode()
    {
        scanningMode = true;
        extractionMode = false;
        scanButton.interactable = false;
        extractButton.interactable = true;
        _messageOutput = "Enabled Scanning Mode \n" +
                        "Attempts Left: "+scanAttempts.ToString();
        UpdateMessageUI();
    }

    public void EnableExtractionMode()
    {
        extractionMode = true;
        scanningMode = false;
        scanButton.interactable = true;
        extractButton.interactable = false;
        _messageOutput = "Enabled Extraction Mode \n" +
                         "Attempts Left: "+extractionAttempts.ToString();
        UpdateMessageUI();
    }
    
    public void ReduceScanAttempt()
    {
        scanAttempts--;
        if (scanAttempts <= 0)
        {
            scanAttempts = 0;
            _messageOutput = "Scanning Area... \n" +
                             "No Attempts Left \n";
        }
        else
        {
            _messageOutput = "Scanning Area... \n" +
                             "Attempts Left: "+scanAttempts.ToString();
        }
        UpdateMessageUI();
    }

    public void ReduceExtractionAttempt()
    {
        extractionAttempts--;
        if (extractionAttempts <= 0)
        {
            extractionAttempts = 0;
            _messageOutput = "Extracting Node... \n" + 
                             "Adding Resources... \n" +
                             "No More Attempts Left \n" +
                             "You have Extracted "+totalGatheredResource.ToString()+" minerals";
            scanButton.interactable = false;
            extractButton.interactable = false;
            resetButton.interactable = true;
        }
        else
        {
            _messageOutput = "Extracting Node... \n" + 
                             "Adding Resources... \n" +
                             "Attempts Left: "+extractionAttempts.ToString();
        }
        UpdateMessageUI();
    }

    public void AddResourcesToTotal(int amount)
    {
        totalGatheredResource += amount;
        resourceTextUI.text = totalGatheredResource.ToString();
    }
    void UpdateMessageUI()
    {
        messageTextUI.text = _messageOutput;
    }

    public void ResetTheGame()
    {
        totalGatheredResource = 0;
        scanAttempts = 6;
        extractionAttempts = 3;
        resourceTextUI.text = totalGatheredResource.ToString();
        GridGenerator.Instance.ResetGrid();
        resetButton.interactable = false;
        EnableExtractionMode();
    }
}
