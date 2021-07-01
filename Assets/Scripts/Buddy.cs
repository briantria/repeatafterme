using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buddy : MonoBehaviour
{
    [SerializeField] private Transform _neck;

    [Space(8)]
    [SerializeField] private float _lookAtSpeed = 3.0f;
    [SerializeField] private Transform _lookAtGridTarget;
    [SerializeField] private Transform _lookAtMeTarget;

    private Transform _lookAtTarget;

    private void OnEnable()
    {
        GameManager.OnChangeGameState += OnChangeGameState;
    }

    private void OnDisable()
    {
        GameManager.OnChangeGameState -= OnChangeGameState;
    }

    private void Awake()
    {
        _lookAtTarget = _lookAtMeTarget;
    }

    private void Update()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_lookAtTarget.position - _neck.position);
        _neck.rotation = Quaternion.Slerp(_neck.rotation, targetRotation, Time.deltaTime * _lookAtSpeed);
    }

    private void OnChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.LevelSay:
                _lookAtTarget = _lookAtGridTarget;
                break;

            default:
                _lookAtTarget = _lookAtMeTarget;
                break;
        }
    }
}
