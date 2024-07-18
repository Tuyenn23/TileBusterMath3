using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDiamond : MonoBehaviour
{
    private void OnEnable()
    {
        AnimDiamond();
    }

    private void AnimDiamond()
    {
        transform.localScale = Vector3.one * 0.7f;
        transform.DOScale(1f, 0.4f).SetLoops(-1,LoopType.Yoyo);
    }
}
