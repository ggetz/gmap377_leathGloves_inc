﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartManager : MonoBehaviour {


    public static GameStartManager Instance = null;
    public GameObject LoadingScreenPrefab;
    public Canvas StartScreenCanvas;
    public Canvas LoadingCanvas;
    public Text LoadingText;
    public InputField NameInputField;
    public Button ContinueButton;
    public Button NewGameButton;
    public float LoadingTextCycleTime = 0.3f;

    void Awake() {
        if (!Instance) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    void Start() {
    }

    /*void OnLevelWasLoaded(int level) {
        if (Application.loadedLevelName == "StartScreen") {
            LoadingCanvas.gameObject.SetActive(true);
            StartScreenCanvas = GameObject.Find("StartScreenCanvas").GetComponent<Canvas>();
            NameInputField = GameObject.Find("NameField").GetComponent<InputField>();
            ContinueButton = GameObject.Find("Continue Button").GetComponent<Button>();
            NewGameButton = GameObject.Find("New Game Button").GetComponent<Button>();

            ContinueButton.onClick.RemoveAllListeners();
            ContinueButton.onClick += OnContinueButtonPressed;
            NewGameButton.onClick.RemoveAllListeners();
            NewGameButton.onClick.AddListener(OnNewGameButtonPressed);
        }
        else if (Application.loadedLevelName == "OriginalScene") {

        }

    }*/

    void Update () {
	
	}


    public void OnNewGameButtonPressed() {
        SaveSystem.Instance.PlayerID = NameInputField.text;
        ScoreManager.Instance.PlayerName = NameInputField.text;

        StartScreenCanvas.enabled = false;
        StartCoroutine(CycleLoadingText());
        Application.LoadLevelAsync("OriginalScene");
        StartCoroutine(DestroyLoadingScreenOnLoad());
    }

    public void OnContinueButtonPressed() {
        SaveSystem.Instance.PlayerID = NameInputField.text;
        ScoreManager.Instance.PlayerName = NameInputField.text;

        StartScreenCanvas.enabled = false;
        StartCoroutine(CycleLoadingText());
        SaveSystem.Instance.SetLoadAtGameStart();
        Application.LoadLevelAsync("OriginalScene");
    }

    IEnumerator CycleLoadingText() {
        int i = 0;
        while (LoadingText != null) {
            if (i == 0) {
                LoadingText.text = "Loading";
            }
            else if (i == 1) {
                LoadingText.text = "Loading.";
            }
            else if (i == 2) {
                LoadingText.text = "Loading..";
            }
            else if (i == 3) {
                LoadingText.text = "Loading...";
            }

            i++;
            if (i > 3) i = 0;

            yield return new WaitForSeconds(LoadingTextCycleTime);
        }
    }

    IEnumerator DestroyLoadingScreenOnLoad() {
        while (Application.loadedLevelName != "OriginalScene") {
            yield return null;
        }
        LoadingCanvas.gameObject.SetActive(false);
    }

    IEnumerator LoadSaveOnLoad() {
        while (!SaveSystem.Instance.CanLoad) {
            yield return null;
        }

        SaveSystem.Instance.LoadGame();
    }
    
}