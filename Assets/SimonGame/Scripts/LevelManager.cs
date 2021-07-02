using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action OnGameStart;
    public static Action OnLevelSayStart;
    public static Action OnLevelSayDone;
    public static Action<bool, int> OnLevelAnswerDone;

    // TODO: say counters
    // TODO: listen press indicators
    // TODO: tally
    // TODO: next level

    [SerializeField] private Button3D _playButton;
    [SerializeField] private List<Button3D> _keyButtons;

    private int _level = 0;
    private GameState _currentGameState;
    private List<LevelItem> _currentLevel;
    private List<LevelItem> _levelAnswer;

    #region Unity Messages
    private void OnEnable()
    {
        Button3D.OnClickDone += OnClickPlay;
        Button3D.OnClick += OnClickButton;
        GameManager.OnChangeGameState += OnChangeGameState;
    }

    private void OnDisable()
    {
        Button3D.OnClickDone -= OnClickPlay;
        Button3D.OnClick -= OnClickButton;
        GameManager.OnChangeGameState -= OnChangeGameState;
    }

    private void Awake()
    {
        _currentLevel = new List<LevelItem>();
        _playButton.Show();
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.gameObject.SetActive(false);
        }
    }
    #endregion

    private void ResetGame()
    {
        _level = 0;
        _currentLevel.Clear();
        _playButton.Show();
        HideKeyButtons();
    }

    public void LoadLevel(int level)
    {
        //Debug.Log("load level " + level.ToString());
        float randDelay = 0.0f;
        if (level > 0)
        {
            float[] validDelays = new float[] { 0.25f, 0.5f };
            int randIdx = UnityEngine.Random.Range(0, validDelays.Length);
            randDelay = validDelays[randIdx];
        }

        _currentLevel.Add(new LevelItem
        {
            KeyIndex = UnityEngine.Random.Range(0, _keyButtons.Count),
            KeyDelay = randDelay
        });

        _levelAnswer = new List<LevelItem>();
        OnLevelSayStart?.Invoke();
        StartCoroutine(LevelPlayRoutine());
    }

    private void OnClickPlay(Button3D button3D)
    {
        if (button3D != _playButton || _currentGameState != GameState.Lobby)
        {
            return;
        }

        StartCoroutine(GameStartRoutine());
    }

    private void OnClickButton(Button3D button3d)
    {
        if (_currentGameState != GameState.LevelListen)
        {
            return;
        }

        _levelAnswer.Add(new LevelItem
        {
            KeyIndex = _keyButtons.IndexOf(button3d)
        });

        if (_levelAnswer.Count != _currentLevel.Count)
        {
            return;
        }

        for (int idx = 0; idx < _currentLevel.Count; ++idx)
        {
            if (_currentLevel[idx].KeyIndex != _levelAnswer[idx].KeyIndex)
            {
                OnLevelAnswerDone?.Invoke(false, _level);
                return;
            }
        }

        OnLevelAnswerDone?.Invoke(true, _level);
    }

    private IEnumerator GameStartRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        _playButton.Hide();

        //yield return new WaitForSeconds(0.5f);
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.Show();
            yield return new WaitForSeconds(0.1f);
        }

        OnGameStart?.Invoke();
    }

    private void HideKeyButtons()
    {
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.Hide();
        }
    }

    private IEnumerator LevelPlayRoutine()
    {
        //Debug.Log("Start Level Say");
        yield return new WaitForSeconds(0.5f);

        foreach (LevelItem levelItem in _currentLevel)
        {
            yield return new WaitForSeconds(levelItem.KeyDelay);
            _keyButtons[levelItem.KeyIndex].InteractWithoutNotify();
        }

        //Debug.Log("Start Level Listen");
        OnLevelSayDone?.Invoke();
    }

    private void OnChangeGameState(GameState gameState)
    {
        _currentGameState = gameState;

        switch (gameState)
        {
            case GameState.Lobby:
                ResetGame();
                break;

            case GameState.LevelLoad:
                LoadLevel(_level++);
                break;

            default:
                break;
        }
    }
}
