using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action OnLevelSayStart;
    public static Action OnLevelSayDone;

    [SerializeField] private Button3D _midButton;
    [SerializeField] private List<Button3D> _keyButtons;

    private List<LevelItem> _currentLevel;
    private int _level = 0;
    private GameState _currentGameState;

    #region Unity Messages
    private void OnEnable()
    {
        Button3D.OnClick += OnClickMidButton;
        GameManager.OnChangeGameState += OnChangeGameState;
    }

    private void OnDisable()
    {
        Button3D.OnClick -= OnClickMidButton;
        GameManager.OnChangeGameState -= OnChangeGameState;
    }

    private void Awake()
    {
        LoadLevel(0);
    }
    #endregion

    public void LoadLevel(int level)
    {
        LevelScriptableObject levelAsset = Resources.Load<LevelScriptableObject>("Levels/" + level.ToString());
        _currentLevel = levelAsset.LevelData;
    }

    private void OnClickMidButton(Button3D button3d)
    {
        if (button3d != _midButton || _currentGameState != GameState.LevelLoad)
        {
            return;
        }

        OnLevelSayStart?.Invoke();
        StartCoroutine(LevelPlayRoutine());
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
                LoadLevel(_level);
                break;

            default:
                break;
        }
    }
}
