using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelItem 
{
    [SerializeField] private int _keyIndex;
    [SerializeField] private float _keyDelay;

    public int KeyIndex => _keyIndex;
    public float KeyDelay => _keyDelay;
}
