using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action OnRetry;

    [SerializeField] private GameObject _starObj;
    [SerializeField] private Transform _starGrid;

    private List<GameObject> _stars = new List<GameObject>();

    private void Start()
    {
        _stars.Add(_starObj);
        for (int idx = 1; idx < 5; idx++)
        {
            GameObject obj = Instantiate(_starObj, _starGrid);
            _stars.Add(obj);
        }
    }

    public void HideScore()
    {
        gameObject.SetActive(false);
    }

    public void DisplayScore(int score)
    {
        gameObject.SetActive(true);

        for (int idx = 0; idx < _stars.Count; ++idx)
        {
            _stars[idx].SetActive(idx < score);
        }

        if (score < _stars.Count)
        {
            return;
        }

        for (int idx = _stars.Count; idx < score; ++idx)
        {
            GameObject obj = Instantiate(_starObj, _starGrid);
            obj.SetActive(true);
            _stars.Add(obj);
        }
    }

    public void Retry()
    {
        //Debug.Log("Retry");
        OnRetry?.Invoke();
        HideScore();
    }
}
