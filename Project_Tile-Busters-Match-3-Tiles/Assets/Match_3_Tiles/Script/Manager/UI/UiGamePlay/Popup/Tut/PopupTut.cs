using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupTut : AnimScale
{
    public GameObject ObjectMove;
    public Vector3 StartPosObjectMove;

    public GameObject ObjectHand;


    public List<GameObject> L_ObjectMerge;
    public List<GameObject> L_ObjectMoveToOneSide;
    public List<Vector3> L_PosObjectMoveToOneSide;

    public Vector3 EndPos = new Vector3(-29.9f, 13.23f, 15.8f);


    [SerializeField] AnimationUIController animController;
    [SerializeField] private Button btn_GotIt;

    Tweener T_Loops;

    private void Start()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Open_pop);

        InitPos();
        GameManager.instance.isInGame = false;
        btn_GotIt.onClick.AddListener(OnGotIt);

        AnimPopUp();
        AnimBtn();
    }

    public void InitPos()
    {
        StartPosObjectMove = ObjectMove.transform.localPosition;

        for (int i = 0; i < L_ObjectMoveToOneSide.Count; i++)
        {
            L_PosObjectMoveToOneSide.Add(L_ObjectMoveToOneSide[i].transform.localPosition);
        }
    }

    private void OnGotIt()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        PlayerDataManager.setCompletedTut(true);
        GameManager.instance.isInGame = true;
        animController.ClosePopUp();
    }

    private void AnimBtn()
    {
        btn_GotIt.transform.DOScale(1.1f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void AnimPopUp()
    {
        T_Loops = ObjectHand.transform.DOScale(100f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        ObjectMove.transform.DOLocalMove(EndPos, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ObjectHand.gameObject.SetActive(false);
            T_Loops?.Kill();

            StartDelayMerge();
            StartDelayMoveToSide(-270f);

            StartDelayInitPos();
        });

        StartDelayMoveToSide(90f);
    }

    private void StartDelayMoveToSide(float value)
    {
        StartCoroutine(IE_DelayMoveToOneSide(value));
    }

    IEnumerator IE_DelayMoveToOneSide(float Value)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < L_ObjectMoveToOneSide.Count; i++)
        {
            L_ObjectMoveToOneSide[i].transform.localPosition = new Vector3(L_ObjectMoveToOneSide[i].transform.localPosition.x + Value, L_ObjectMoveToOneSide[i].transform.localPosition.y, L_ObjectMoveToOneSide[i].transform.localPosition.z);
        }
    }

    private void StartDelayMerge()
    {
        StartCoroutine(IE_DElayMerge());
    }
    IEnumerator IE_DElayMerge()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < L_ObjectMerge.Count; i++)
        {
            L_ObjectMerge[i].gameObject.SetActive(false);

        }
    }

    private void StartDelayInitPos()
    {
        StartCoroutine(IE_DelayInitPos());
    }

    IEnumerator IE_DelayInitPos()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < L_ObjectMerge.Count; i++)
        {
            L_ObjectMerge[i].gameObject.SetActive(true);
        }

        ObjectMove.transform.localPosition = StartPosObjectMove;

        for (int i = 0; i < L_ObjectMoveToOneSide.Count; i++)
        {
            L_ObjectMoveToOneSide[i].transform.localPosition = L_PosObjectMoveToOneSide[i];
        }
        ObjectHand.transform.localScale = Vector3.one * 120f;
        ObjectHand.gameObject.SetActive(true);

        AnimPopUp();
    }
}
