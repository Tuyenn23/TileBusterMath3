using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using DG.Tweening;

public class PopupLose : AnimScale
{
    [Header("Coin")]
    [SerializeField] private TMP_Text Coin_txt;

    [SerializeField] private Button btn_Giveup;
    [SerializeField] private Button btn_PlayOn;

    [SerializeField] private bool isDeductPlayturn = false;

    [SerializeField] private AnimationUIController AnimationUIController;

    Tweener T_btnPlayOn;

    private void OnEnable()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Lose);
        InitCoin();
        AnimBtnPlayOn();
        DeductPlayTurn();

        btn_Giveup.onClick.AddListener(OnGiveUp);
        btn_PlayOn.onClick.AddListener(OnPlayOn);
    }

    private void DeductPlayTurn()
    {
        if (!isDeductPlayturn)
        {
            int CurrentPlayTurn = PlayerDataManager.GetAmoutTurn();
            CurrentPlayTurn--;
            PlayerDataManager.SetAmoutTurn(CurrentPlayTurn);

            if (!PlayerDataManager.IsLastPlayTime())
            {
                PlayerDataManager.SetLastPlayHome(DateTime.Now.Ticks.ToString());
                PlayerDataManager.SetIsFirstTimePlay(true);
            }

            isDeductPlayturn = true;
        }
    }

    private void InitCoin()
    {
        Coin_txt.text = PlayerDataManager.GetCoin().ToString();
    }

    private void AnimBtnPlayOn()
    {
        btn_PlayOn.transform.localScale = Vector3.one;
        T_btnPlayOn = btn_PlayOn.transform.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetDelay(0.4f);
    }

    private void OnPlayOn()
    {
        AdManager.instance.ShowReward(() =>
        {
            ListItem _listitem = GameManager.instance.UiController.UiGamePlay.ListItem;
            ListItemPicked _listitempicked = GameManager.instance.UiController.UiGamePlay.ListItemPicked;
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

            if (_listitem.L_ContainsItemTileReturn.Count > 0) return;

            GameManager.instance.StartIsInGame();
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.Close_pop);
            AnimationUIController.ClosePopUp();

            for (int i = 0; i < 2; i++)
            {
                ProcessItemLose(_listitempicked.L_Stored[i], _listitem.L_posMoveItem[i].transform.position);
            }

            ProcessItemLose(_listitempicked.L_Stored[_listitempicked.L_Stored.Count - 1], _listitem.L_posMoveItem[2].transform.position, () =>
            {

            });


        }, () =>
        {
            GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
        }, "ShowReward");
    }


    private void ProcessItemLose(BrickBase brick, Vector3 position, System.Action complete = null)
    {
        ListItemPicked _listItemPicked = GameManager.instance.UiController.UiGamePlay.ListItemPicked;
        ListItem _listitem = GameManager.instance.UiController.UiGamePlay.ListItem;



        _listitem.L_ContainsItemTileReturn.Add(brick);

        brick.transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (_listItemPicked.L_Stored.Count > 0)
            {
                _listItemPicked.L_Stored.Remove(brick);
            }

            if (_listItemPicked.Dic_TypeCheck.ContainsKey(brick._typeBrick))
            {
                _listItemPicked.Dic_TypeCheck[brick._typeBrick].Remove(brick);

                if (_listItemPicked.Dic_TypeCheck[brick._typeBrick].Count == 0)
                {
                    _listItemPicked.Dic_TypeCheck.Remove(brick._typeBrick);
                }
            }

            brick.IsMoved = false;
            _listItemPicked.SortElement();
            complete?.Invoke();
        }).SetAutoKill();
    }

    private void OnGiveUp()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);

        if (PlayerDataManager.GetAmoutTurn() == 4)
        {
            PlayerDataManager.SetLastPlayHome(DateTime.Now.Ticks.ToString());
        }

        DOTween.KillAll();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        btn_Giveup.onClick.RemoveListener(OnGiveUp);
        btn_PlayOn.onClick.RemoveListener(OnPlayOn);

        T_btnPlayOn?.Kill();
    }
}
