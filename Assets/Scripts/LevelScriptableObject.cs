using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelAsset", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
public class LevelScriptableObject : ScriptableObject
{
    [SerializeField] private List<LevelItem> _levelData;

    public List<LevelItem> LevelData => _levelData;
}
