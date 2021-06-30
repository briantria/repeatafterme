using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button3D _playButton;
    [SerializeField] private List<Button3D> _keyButtons;

    #region Unity Messages
    private void OnEnable()
    {
        Button3D.OnClick += OnClickPlay;
        Button3D.OnClick += OnClickKeyButton;
    }

    private void OnDisable()
    {
        Button3D.OnClick -= OnClickPlay;
        Button3D.OnClick -= OnClickKeyButton;
    }

    private void Start()
    {
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.gameObject.SetActive(false);
        }
    }
    #endregion

    private void OnClickPlay(Button3D button3D)
    {
        if (button3D != _playButton)
        {
            return;
        }

        _playButton.gameObject.SetActive(false);
        StartCoroutine(ShowKeyButtonsRoutine());
    }

    private void OnClickKeyButton(Button3D button3D)
    {

    }

    private void StartGame()
    { 
        
    }

    private IEnumerator ShowKeyButtonsRoutine()
    {
        foreach (Button3D keyButton in _keyButtons)
        {
            keyButton.Show();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
