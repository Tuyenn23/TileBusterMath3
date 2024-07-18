using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoosterTut", fileName = "Booster")]
public class DataBoosterTut : ScriptableObject
{
    public List<BOOSTER> L_Booster;


    public BOOSTER getBooster(E_TypeBooster _TypeBooster)
    {
        for (int i = 0; i < L_Booster.Count; i++)
        {
            if (L_Booster[i].TypeBooster == _TypeBooster)
            {
                return L_Booster[i];
            }
        }

        return null;
    }
}

[Serializable]
public class BOOSTER
{
    public E_TypeBooster TypeBooster;
    public Sprite Icon;
    public int Price;
    public string name;
}