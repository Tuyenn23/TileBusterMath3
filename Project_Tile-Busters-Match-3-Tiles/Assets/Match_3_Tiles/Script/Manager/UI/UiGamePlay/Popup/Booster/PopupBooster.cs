using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupBooster : MonoBehaviour
{
    public E_TypeBooster E_currentBooterUnlock;
    public Image Icon_img;
    public TMP_Text Name_txt;
    public Button btn_claim;

    [SerializeField] private RectTransform ContentTop;
    [SerializeField] private RectTransform ContentBottom;
    [SerializeField] private RectTransform ContentLight;

    [SerializeField] private List<Image> L_imgDiamond;
    Tweener T_MoveContentTop, T_MoveContentBottom, T_MoveBackContentTop, T_MoveBackContentBottom;
    Tweener T_WellDoneFT, T_WellDontTT, T_light, T_btnClaim;

    private void OnEnable()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Open_pop);

        btn_claim.onClick.AddListener(OnClaim);
        DeActiveListImg();
        ActiveAnim();
    }

    public void InitBoosterUnlock(E_TypeBooster _type)
    {
        BOOSTER Current = PrefabStorage.Instance.DataBooster.getBooster(_type);
        E_currentBooterUnlock = _type;

        Icon_img.sprite = Current.Icon;
        Name_txt.text = Current.name;
    }

    public void ActiveAnim()
    {
        AnimContentTop();
        AnimContenBottom();
    }

    private void DeActiveListImg()
    {
        for (int i = 0; i < L_imgDiamond.Count; i++)
        {
            L_imgDiamond[i].transform.localScale = Vector3.one * 0.7f;
            L_imgDiamond[i].gameObject.SetActive(false);
        }
    }

    public void AnimContentTop()
    {
        ContentLight.transform.localScale = Vector3.zero;
        ContentLight.gameObject.SetActive(false);


        Vector3 _contentTop = ContentTop.anchoredPosition;
        _contentTop.y += 450f;

        ContentTop.anchoredPosition = _contentTop;

        T_MoveContentTop = ContentTop.DOAnchorPosY(550f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            T_WellDoneFT?.Kill();
            T_btnClaim = btn_claim.transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            ContentLight.gameObject.SetActive(true);
        }).SetAutoKill();

        T_light = ContentLight.transform.DOScale(1.5f, 1f).SetEase(Ease.OutElastic).SetDelay(0.5f).OnComplete(() =>
        {
            for (int i = 0; i < L_imgDiamond.Count; i++)
            {
                L_imgDiamond[i].gameObject.SetActive(true);
            }
        }).SetAutoKill();
    }

    public void AnimContenBottom()
    {
        Vector3 _contentBottom = ContentBottom.anchoredPosition;
        _contentBottom.y -= 500f;

        ContentBottom.anchoredPosition = _contentBottom;

        T_MoveContentBottom = ContentBottom.DOAnchorPosY(-350f, 0.3f).SetEase(Ease.Linear).SetAutoKill();
    }
    private void OnClaim()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

        AnimBackContentTop();
        AnimBackContenBottom();
        AnimBackContenMidle();

        StartCoroutine(IE_DelayBackHome());
    }

    public void AnimBackContentTop()
    {
        T_MoveBackContentTop = ContentTop.DOAnchorPosY(1000f, 0.3f).SetEase(Ease.Linear).SetAutoKill();
    }

    public void AnimBackContenBottom()
    {
        T_MoveBackContentBottom = ContentBottom.DOAnchorPosY(-800f, 0.3f).SetEase(Ease.Linear).SetAutoKill();
    }

    public void AnimBackContenMidle()
    {
        T_MoveBackContentBottom = ContentLight.transform.DOScale(0, 0.3f).SetEase(Ease.Linear).SetAutoKill();
    }

    IEnumerator IE_DelayBackHome()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        T_btnClaim?.Kill();
    }
}
