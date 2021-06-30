using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button3D _midButton;
    [SerializeField] private List<Button3D> _keyButtons;

    private List<LevelItem> _currentLevel;
    private int _levelIndex = 0;

    #region Unity Messages
    private void OnEnable()
    {
        Button3D.OnClick += OnClickMidButton;
    }

    private void OnDisable()
    {
        Button3D.OnClick -= OnClickMidButton;
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
        if (button3d != _midButton)
        {
            return;
        }

        StartCoroutine(LevelPlayRoutine());
    }

    private IEnumerator LevelPlayRoutine()
    {
        foreach (LevelItem levelItem in _currentLevel)
        {
            yield return new WaitForSeconds(levelItem.KeyDelay);
            _keyButtons[levelItem.KeyIndex].InteractWithoutNotify();
        }
    }
}
