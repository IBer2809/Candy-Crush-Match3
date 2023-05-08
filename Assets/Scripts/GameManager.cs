using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { Menu, Play};
    [field: SerializeField] public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        CurrentState = GameState.Menu;
    }

    public void ChangeGameState(GameState state) => CurrentState = state;

}
