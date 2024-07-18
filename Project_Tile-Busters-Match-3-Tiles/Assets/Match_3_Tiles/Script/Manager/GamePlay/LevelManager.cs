using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<BrickBase> L_BrickInLevel;

    private void Start()
    {
        BrickBase[] childs = transform.GetComponentsInChildren<BrickBase>();

        L_BrickInLevel = new List<BrickBase>(childs);
    }
}
