using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action OnLevelSayStart;
    public static Action OnLevelSayDone;
    public static Action<bool> OnLevelAnswerDone;

    // TODO: say counters
    // TODO: listen press indicators
    // TODO: tally
    // TODO: next level

    [SerializeField] private Button3D _midButton;
    [SerializeField] private List<Button3D> _keyButtons;

    private int _level = 0;
    private GameState _currentGameState;
    private List<LevelItem> _currentLevel;
    private List<LevelItem> _levelAnswer;

    #region Unity Messages
    private void OnEnable()
    {
        Button3D.OnClick += OnClickMidButton;
        Button3D.OnClick += OnClickButton;
        GameManager.OnChangeGameState += OnChangeGameState;
    }

    private void OnDisable()
    {
        Button3D.OnClick -= OnClickMidButton;
        Button3D.OnClick -= OnClickButton;
        GameManager.OnChangeGameState -= OnChangeGameState;
    }

    private void Awake()
    {
        LoadLevel(0);
    }
    #endregion

    public void LoadLevel(int level)
    {
        Debug.Log("load level " + level.ToString());
        LevelScriptableObject levelAsset = Resources.Load<LevelScriptableObject>("Levels/" + level.ToString());
        if (levelAsset == null)
        {
            Debug.Log("No more levels.");
            return;
        }
        _currentLevel = levelAsset.LevelData;
    }

    private void OnClickMidButton(Button3D button3d)
    {
        if (button3d != _midButton || _currentGameState != GameState.LevelLoad)
        {
            return;
        }

        _levelAnswer = new List<LevelItem>();
        OnLevelSayStart?.Invoke();
        StartCoroutine(LevelPlayRoutine());
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
                OnLevelAnswerDone?.Invoke(false);
                return;
            }
        }

        OnLevelAnswerDone?.Invoke(true);
    }

    private IEnumerator LevelPlayRoutine()
    {
        foreach (LevelItem levelItem in _currentLevel)
        {
            yield return new WaitForSeconds(levelItem.KeyDelay);
            _keyButtons[levelItem.KeyIndex].InteractWithoutNotify();
        }

        OnLevelSayDone?.Invoke();
    }

    private void OnChangeGameState(GameState gameState)
    {
        _currentGameState = gameState;

        switch (gameState)
        {
            case GameState.LevelLoad:
                LoadLevel(_level++);
                break;

            default:
                break;
        }
    }
}
