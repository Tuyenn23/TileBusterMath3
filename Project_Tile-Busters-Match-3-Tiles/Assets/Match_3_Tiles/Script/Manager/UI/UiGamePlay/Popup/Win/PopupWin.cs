using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupWin : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] private TMP_Text AmountLife_txt;

    [Header("Coin")]
    [SerializeField] private TMP_Text AmountCoin_txt;

    [SerializeField] private Button btn_claimX5;
    [SerializeField] private Button btn_nothanks;

    [SerializeField] private TMP_Text AmountCoinWinLevel_txt;

    [Header("Anim")]
    [SerializeField] private Image img_WellDone;
    [SerializeField] private RectTransform ContentLight;
    [SerializeField] private RectTransform ContentBottom;

    [SerializeField] private List<Image> L_imgDiamond;


    Tweener T_WellDoneFT, T_WellDontTT, T_light;

    Coroutine C_DelayActive;
    private void OnEnable()
    {
        PlayerDataManager.SetClaimX5(false);

        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Win);

        DeActiveListImg();

        btn_claimX5.onClick.AddListener(Onclaimx5);
        btn_nothanks.onClick.AddListener(OnReject);

        btn_nothanks.gameObject.SetActive(false);
        InitCoin();

        AnimMidle();
        AnimBottom();
        InitCoinCompleteLevel();

        C_DelayActive = StartCoroutine(IE_DelayAcTiveNoThanks());
    }

    IEnumerator IE_DelayAcTiveNoThanks()
    {
        yield return new WaitForSeconds(3f);
        btn_nothanks.gameObject.SetActive(true);
    }

    private void DeActiveListImg()
    {
        for (int i = 0; i < L_imgDiamond.Count; i++)
        {
            L_imgDiamond[i].gameObject.SetActive(false);
        }
    }

    private void AnimMidle()
    {
        ContentLight.transform.localScale = Vector3.zero;
        ContentLight.gameObject.SetActive(false);
        img_WellDone.transform.localScale = Vector3.one * 0.4f;
        T_WellDoneFT = img_WellDone.transform.DOScale(1f, 0.2f).OnComplete(() =>
        {
            T_WellDoneFT?.Kill();
            T_WellDontTT = img_WellDone.transform.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            ContentLight.gameObject.SetActive(true);
        });

        T_light = ContentLight.transform.DOScale(1f, 1f).SetEase(Ease.OutElastic).SetDelay(0.5f).OnComplete(() =>
        {
            for (int i = 0; i < L_imgDiamond.Count; i++)
            {
                L_imgDiamond[i].gameObject.SetActive(true);
            }
        });
    }

    private void AnimBottom()
    {
        Vector3 Content = ContentBottom.anchoredPosition;
        Content.y -= 1000;

        ContentBottom.anchoredPosition = Content;
        ContentBottom.DOAnchorPosY(0, 0.3f).SetDelay(1f);

        btn_claimX5.transform.localScale = Vector3.one;
        btn_claimX5.transform.DOScale(1.1f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetDelay(2f);
    }

    private void InitCoinCompleteLevel()
    {
        AmountCoinWinLevel_txt.text = $"+ {PrefabStorage.Instance.DataLevel.GetLevel(PlayerDataManager.GetCurrentLevel() - 1).CoinCompleteLevel}";
    }

    private void InitCoin()
    {
        AmountCoin_txt.text = PlayerDataManager.GetCoin().ToString();
    }

    private void OnReject()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

        AdManager.instance.ShowInter(() =>
        {
            int newCoin = PlayerDataManager.GetCoin() + PrefabStorage.Instance.DataLevel.GetLevel(PlayerDataManager.GetCurrentLevel() - 1).CoinCompleteLevel;
            PlayerDataManager.SetCoin(newCoin);

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }, () =>
        {
            int newCoin = PlayerDataManager.GetCoin() + PrefabStorage.Instance.DataLevel.GetLevel(PlayerDataManager.GetCurrentLevel() - 1).CoinCompleteLevel;
            PlayerDataManager.SetCoin(newCoin);

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }, "ShowInter");

    }
    private void Onclaimx5()
    {
        AdManager.instance.ShowReward(() =>
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
            PlayerDataManager.SetClaimX5(true);
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }, () =>
        {
            GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
        }, "ShowReward");
    }

    private void OnDisable()
    {
        DOTween.KillAll();

        btn_claimX5.onClick.RemoveListener(Onclaimx5);
        btn_nothanks.onClick.RemoveListener(OnReject);

        StopCoroutine(C_DelayActive);
    }
}
