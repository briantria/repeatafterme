using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelItem 
{
    [SerializeField] private int _keyIndex;
    [SerializeField] private float _keyDelay;

    public int KeyIndex { get => _keyIndex; set => _keyIndex = value; }
    public float KeyDelay { get => _keyDelay; set => _keyDelay = value; }
}
