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

    [SerializeField] private Button3D _playButton;
    [SerializeField] private List<Button3D> _keyButtons;

    private GameState _currentGameState;

    #region Unity Messages
    private void OnEnable()
    {
        Button3D.OnClick += OnClickPlay;
        Button3D.OnClick += OnClickKeyButton;
        LevelManager.OnLevelSayStart += NextGameState;
        LevelManager.OnLevelSayDone += NextGameState;
        LevelManager.OnLevelAnswerDone += OnAnswerDone;
    }

    private void OnDisable()
    {
        Button3D.OnClick -= OnClickPlay;
        Button3D.OnClick -= OnClickKeyButton;
        LevelManager.OnLevelSayStart -= NextGameState;
        LevelManager.OnLevelSayDone -= NextGameState;
        LevelManager.OnLevelAnswerDone -= OnAnswerDone;
    }

    private void Start()
    {
        _currentGameState = GameState.Lobby;
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.gameObject.SetActive(false);
        }
    }
    #endregion

    private void OnClickPlay(Button3D button3D)
    {
        if (button3D != _playButton || _currentGameState != GameState.Lobby)
        {
            return;
        }

        _playButton.gameObject.SetActive(false);
        StartCoroutine(ShowKeyButtonsRoutine());
    }

    private void OnClickKeyButton(Button3D button3D)
    {

    }

    private IEnumerator ShowKeyButtonsRoutine()
    {
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.Show();
            yield return new WaitForSeconds(0.1f);
        }

        NextGameState();
    }

    private void OnAnswerDone(bool isCorrect)
    {
        _currentGameState = isCorrect ? GameState.LevelLoad : GameState.GameOver;
        Debug.Log("answer done. next state: " + _currentGameState.ToString());
        OnChangeGameState?.Invoke(_currentGameState);
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

            default:
                break;
        }

        Debug.Log("Current game state: " + _currentGameState.ToString());
        OnChangeGameState?.Invoke(_currentGameState);
    }
}
