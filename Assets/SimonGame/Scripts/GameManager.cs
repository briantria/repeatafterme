using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{ 
    Lobby,
    LevelLoad,
    LevelSay,
    LevelListen,
    LevelOver,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static Action<GameState> OnChangeGameState;

    [SerializeField] private ScoreManager _scoreManager;

    private GameState _currentGameState;

    #region Unity Messages
    private void OnEnable()
    {
        LevelManager.OnGameStart += NextGameState;
        LevelManager.OnLevelSayStart += NextGameState;
        LevelManager.OnLevelSayDone += NextGameState;
        LevelManager.OnLevelAnswerDone += OnAnswerDone;
        ScoreManager.OnRetry += NextGameState;
    }

    private void OnDisable()
    {
        LevelManager.OnGameStart += NextGameState;
        LevelManager.OnLevelSayStart -= NextGameState;
        LevelManager.OnLevelSayDone -= NextGameState;
        LevelManager.OnLevelAnswerDone -= OnAnswerDone;
        ScoreManager.OnRetry -= NextGameState;
    }

    private void Start()
    {
        _scoreManager.HideScore();
        _currentGameState = GameState.Lobby;
        OnChangeGameState?.Invoke(_currentGameState);
    }
    #endregion

    private IEnumerator ShowScoreRoutine(int score)
    {
        yield return new WaitForSeconds(0.2f);
        _scoreManager.DisplayScore(score);

    }

    private void OnAnswerDone(bool isCorrect, int level)
    {
        _currentGameState = isCorrect ? GameState.LevelLoad : GameState.GameOver;
        //Debug.Log("answer done. next state: " + _currentGameState.ToString());
        OnChangeGameState?.Invoke(_currentGameState);

        if (!isCorrect)
        {
            StartCoroutine(ShowScoreRoutine(level));
        }
    }

    public void NextGameState()
    {
        switch (_currentGameState)
        {
            case GameState.Lobby:
                _currentGameState = GameState.LevelLoad;
                break;

            case GameState.LevelLoad:
                _currentGameState = GameState.LevelSay;
                break;

            case GameState.LevelSay:
                _currentGameState = GameState.LevelListen;
                break;

            case GameState.LevelListen:
                _currentGameState = GameState.LevelOver;
                break;

            case GameState.LevelOver:
                _currentGameState = GameState.LevelLoad;
                break;

            case GameState.GameOver:
                _currentGameState = GameState.Lobby;
                break;

            default:
                break;
        }

        //Debug.Log("Current game state: " + _currentGameState.ToString());
        OnChangeGameState?.Invoke(_currentGameState);
    }
}
