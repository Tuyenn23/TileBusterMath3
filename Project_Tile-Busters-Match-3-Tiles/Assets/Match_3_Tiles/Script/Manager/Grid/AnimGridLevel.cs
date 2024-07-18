using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimGridLevel : MonoBehaviour
{
    public E_TypeAnimLevel _typeAnim;

    private void Start()
    {
        RandomType();
        ProcessAnim(_typeAnim);
    }

    private void RandomType()
    {
        System.Random random = new System.Random();

        var values = Enum.GetValues(typeof(E_TypeAnimLevel));

        _typeAnim = (E_TypeAnimLevel)values.GetValue(random.Next(values.Length));
    }

    public void ProcessAnim(E_TypeAnimLevel _typeAnim)
    {
        switch (_typeAnim)
        {
            case E_TypeAnimLevel.Zoom:
                GridManager.instance.transform.localScale = Vector3.zero;
                GridManager.instance.transform.DOScale(1f, 1f).SetEase(Ease.InOutElastic).OnComplete(() =>
                {
                    GameManager.instance.StartIsInGame();
                    GridManager.instance.InitStartPosBrick();
                });

                break;
            case E_TypeAnimLevel.RightLeft:
                GridManager.instance.transform.position = new Vector3(-15f, 0, 0);
                GridManager.instance.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InOutElastic).OnComplete(() =>
                {
                    GameManager.instance.StartIsInGame();
                    GridManager.instance.InitStartPosBrick();
                });
                break;
            case E_TypeAnimLevel.LeftRight:
                GridManager.instance.transform.position = new Vector3(15f, 0, 0);
                GridManager.instance.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InOutElastic).OnComplete(() =>
                {
                    GameManager.instance.StartIsInGame();
                    GridManager.instance.InitStartPosBrick();
                });
                break;
            default:
                break;
        }
    }
}
public enum E_TypeAnimLevel
{
    Zoom,
    LeftRight,
    RightLeft,
}