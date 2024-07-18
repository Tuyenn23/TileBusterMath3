using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage Instance { get; private set; }

    public LevelManager Level_1;
    public BrickBase BrickBase;
    public GridGroup GridGroup;

    [Header("---DataController---")]
    public DataLevelSO DataLevel;
    public DataBoosterTut DataBooster;

    public ParticleSystem FxMerge;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
