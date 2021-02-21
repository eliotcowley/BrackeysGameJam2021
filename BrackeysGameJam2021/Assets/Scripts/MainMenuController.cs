using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuScreen;

    [SerializeField]
    private GameObject controlsScreen;

    [SerializeField]
    private GameObject aboutScreen;

    [SerializeField]
    private Selectable startButton;

    [SerializeField]
    private Selectable controlsBackButton;

    [SerializeField]
    private Selectable aboutBackButton;

    [SerializeField]
    private BoxCollider gerbilSpawner;

    [SerializeField]
    private Vector2 minMaxSpawnTime;

    [SerializeField]
    private GameObject gerbilPrefab;

    private MainMenuState state = MainMenuState.MainMenu;
    private float nextSpawnTime = 0f;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        this.startButton.Select();
        StartCoroutine(SpawnGerbil());
        Time.timeScale = 0.1f;
    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnControlsButtonPressed()
    {
        this.mainMenuScreen.SetActive(false);
        this.controlsScreen.SetActive(true);
        this.state = MainMenuState.Controls;
        EventSystem.current.SetSelectedGameObject(null);
        this.controlsBackButton.Select();
    }

    public void OnAboutButtonPressed()
    {
        this.mainMenuScreen.SetActive(false);
        this.aboutScreen.SetActive(true);
        this.state = MainMenuState.About;
        EventSystem.current.SetSelectedGameObject(null);
        this.aboutBackButton.Select();
    }

    public void OnQuitButtonPressed()
    {
        if (Application.isEditor)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void OnBackButtonPressed()
    {
        if (this.state == MainMenuState.Controls)
        {
            this.controlsScreen.SetActive(false);
        }
        else if (this.state == MainMenuState.About)
        {
            this.aboutScreen.SetActive(false);
        }

        this.mainMenuScreen.SetActive(true);
        this.state = MainMenuState.MainMenu;
        EventSystem.current.SetSelectedGameObject(null);
        this.startButton.Select();
    }

    private float GetNextSpawnTime()
    {
        return Random.Range(this.minMaxSpawnTime.x, this.minMaxSpawnTime.y);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float x = Random.Range(this.gerbilSpawner.bounds.min.x, this.gerbilSpawner.bounds.max.x);
        float y = Random.Range(this.gerbilSpawner.bounds.min.y, this.gerbilSpawner.bounds.max.y);
        float z = Random.Range(this.gerbilSpawner.bounds.min.z, this.gerbilSpawner.bounds.max.z);

        return new Vector3(x, y, z);
    }

    private IEnumerator SpawnGerbil()
    {
        yield return new WaitForSeconds(this.nextSpawnTime);
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Instantiate(this.gerbilPrefab, spawnPoint, Random.rotation);
        this.nextSpawnTime = GetNextSpawnTime();
        StartCoroutine(SpawnGerbil());
    }
}

public enum MainMenuState
{
    MainMenu,
    Controls,
    About
}