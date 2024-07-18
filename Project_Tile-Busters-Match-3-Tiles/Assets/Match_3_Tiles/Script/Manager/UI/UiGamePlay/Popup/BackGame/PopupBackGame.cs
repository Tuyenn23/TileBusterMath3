using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupBackGame : AnimLeftRight
{
    [SerializeField] AnimationUIController animUI;
    [SerializeField] private Button btn_close;
    [SerializeField] private Button btn_Leave;
    [SerializeField] private Image img_Heart;

    Tweener T_Scale, T_Heart;

    private void OnEnable()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Open_pop);

        btn_close.onClick.AddListener(OnClose);
        btn_Leave.onClick.AddListener(OnLeaveGame);

        AnimBtnLeave();
        AnimHeart();
    }

    private void AnimBtnLeave()
    {
        btn_Leave.transform.localScale = Vector3.one;
        T_Scale = btn_Leave.transform.DOScale(Vector3.one * 1.2f, 0.4f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true)
            .SetDelay(0.5f);
    }

    private void AnimHeart()
    {
        img_Heart.color = Color.white;
        T_Heart = img_Heart.DOColor(new Color(0.7f, 0.7f, 0.7f), 5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetDelay(0.5f);
    }

    private void OnClose()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

        GameManager.instance.UiController.UiGamePlay.ActiveAnim();
        GameManager.instance.StartIsInGame();
        animUI.ClosePopUp();
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Close_pop);

    }
    private void OnLeaveGame()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        int CurrentPlayTurn = PlayerDataManager.GetAmoutTurn();

        if (CurrentPlayTurn == 5)
        {
            PlayerDataManager.SetLastPlayHome(DateTime.Now.Ticks.ToString());
        }

        CurrentPlayTurn--;
        PlayerDataManager.SetAmoutTurn(CurrentPlayTurn);

        if (!PlayerDataManager.IsLastPlayTime())
        {
            PlayerDataManager.SetLastPlayHome(DateTime.Now.Ticks.ToString());
            PlayerDataManager.SetIsFirstTimePlay(true);
        }

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        DOTween.KillAll();
        T_Heart?.Kill();
        T_Scale?.Kill();
        btn_close.onClick.RemoveListener(OnClose);
        btn_Leave.onClick.RemoveListener(OnLeaveGame);
    }

}
