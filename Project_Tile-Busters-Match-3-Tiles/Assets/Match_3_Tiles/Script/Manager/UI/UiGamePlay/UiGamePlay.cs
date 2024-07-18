using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class UiGamePlay : MonoBehaviour
{
    [SerializeField] private Button btn_Close;
    [SerializeField] private Button btn_Setting;
    [SerializeField] private TMP_Text coin_txt;
    [SerializeField] private TMP_Text level_txt;

    public ListItemPicked ListItemPicked;
    public ListItem ListItem;

    public PopupWin Popupwin;
    public PopupLose PopupLose;
    public PopupBackGame popupBackGame;
    public PopupPause PopupPause;
    public PopupTut PopupTut;
    public RemainingShowAds remainingShowAds;
    public RectTransform PanelDimItemMerge;

    [Header("Anim")]
    [SerializeField] private RectTransform ContentTop;
    [SerializeField] private RectTransform ContentBottom;
    Tweener T_MoveContentTop, T_MoveContentBottom, T_MoveBackContentTop, T_MoveBackContentBottom;
    private void OnEnable()
    {
        btn_Close.onClick.AddListener(OnBackToHome);
        btn_Setting.onClick.AddListener(OnOpenPopupSetting);

        ActiveAnim();

        IniCoinAndLevel();
    }

    private void Start()
    {
        if (PlayerDataManager.GetCompletedTut())
        {
            PopupTut.gameObject.SetActive(false);
        }
        else
        {
            PopupTut.gameObject.SetActive(true);
        }

    }

    #region Anim Content
    public void ActiveAnim()
    {
        AnimContentTop();
        AnimContenBottom();
    }

    public void AnimContentTop()
    {
        Vector3 _contentTop = ContentTop.anchoredPosition;
        _contentTop.y += 450f;

        ContentTop.anchoredPosition = _contentTop;

        T_MoveContentTop = ContentTop.DOAnchorPosY(0f, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimContenBottom()
    {
        Vector3 _contentBottom = ContentBottom.anchoredPosition;
        _contentBottom.y -= 350f;

        ContentBottom.anchoredPosition = _contentBottom;

        T_MoveContentBottom = ContentBottom.DOAnchorPosY(150f, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimMoveBack()
    {
        AnimBackContentTop();
        AnimBackContenBottom();
    }

    public void AnimBackContentTop()
    {
        T_MoveBackContentTop = ContentTop.DOAnchorPosY(500f, 0.3f).SetEase(Ease.Linear);
    }

    public void AnimBackContenBottom()
    {
        T_MoveBackContentBottom = ContentBottom.DOAnchorPosY(-600f, 0.3f).SetEase(Ease.Linear);
    }

    #endregion

    public void IniCoinAndLevel()
    {
        coin_txt.text = Helper.FormatCurrency(PlayerDataManager.GetCoin());
        level_txt.text = $"Level {PlayerDataManager.GetCurrentLevel()}";
    }

    private void OnBackToHome()
    {
        if (AdsHandle.instance.canShowInterBack)
        {
            AdManager.instance.ShowInter(null, null, "ShowInter");
        }

        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        AnimMoveBack();
        GameManager.instance.StartDontInGame();
        popupBackGame.gameObject.SetActive(true);
    }

    private void OnOpenPopupSetting()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        AnimMoveBack();
        GameManager.instance.StartDontInGame();
        PopupPause.gameObject.SetActive(true);
    }

    public void OnOpenPopupWin()
    {
        AnimMoveBack();
        GameManager.instance.StartDontInGame();
        Popupwin.gameObject.SetActive(true);
    }

    public void OnOpenPopupLose()
    {
        GameManager.instance.StartDontInGame();
        PopupLose.gameObject.SetActive(true);
    }



    private void OnDisable()
    {
        T_MoveContentTop?.Kill();
        T_MoveBackContentBottom?.Kill();
        T_MoveBackContentTop?.Kill();
        T_MoveContentBottom?.Kill();


        btn_Close.onClick.RemoveListener(OnBackToHome);
        btn_Setting.onClick.RemoveListener(OnOpenPopupSetting);
    }
}
