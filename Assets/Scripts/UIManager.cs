using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] AppsFlyerObjectScript AppsFlyerObj;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _dataPanel;
    [SerializeField] private GameObject _backToMenuFromPlayButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _dataButton;
    [SerializeField] private Text _dataText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        _menuPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _dataPanel.SetActive(false);
        ActivatePlayPanelButtons(false);
    }

    public void ActivateGame()
    {
        StartCoroutine(GridManager.Instance.SpawnGridAndBeans());
        _menuPanel.SetActive(false);
        _gamePanel.SetActive(true);
        _dataPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Play)
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.Menu);
            GridManager.Instance.DeleteGrid();
        }

        _menuPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _dataPanel.SetActive(false);
        ActivatePlayPanelButtons(false);

    }

    public void ActivatePlayPanelButtons(bool value)
    {
        _backToMenuFromPlayButton.SetActive(value);
        _restartButton.SetActive(value);
    }

    public void RestartLevel()
    {
        ActivatePlayPanelButtons(false);
        GameManager.Instance.ChangeGameState(GameManager.GameState.Menu);
        GridManager.Instance.DeleteGrid();
        StartCoroutine(GridManager.Instance.SpawnGridAndBeans());
    }

    public void ShowData()
    {
        _dataPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _menuPanel.SetActive(false);
        AppsFlyerObj.onConversionDataSuccess("Data");
        _dataText.text = AppsFlyerObj.GetConvData();
    }


}
