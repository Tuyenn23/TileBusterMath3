using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemainingShowAds : MonoBehaviour
{
    public TMP_Text RemaingTime_txt;
    public Image Fill_bar;

    Coroutine coCanPlay;

    int MaxTime;
    float TimeCountDown;
    int Second;

    Tweener tweenCountTime, T_Fill;
    private void OnEnable()
    {
        Fill_bar.fillAmount = 1f;
        MaxTime = 5;
        RemaingTime_txt.text = $"{MaxTime}";
        StartCountdown(6);
    }

    private void Update()
    {
        /*        if (MaxTime < 0) return;

                TimeCountDown += Time.deltaTime;

                if (TimeCountDown > 1)
                {
                    TimeCountDown -= 1;
                    MaxTime -= 1;

                    Second = MaxTime;

                    if (MaxTime >= 0)
                    {
                        RemaingTime_txt.text = $"{Second}";

                        if (MaxTime == 0)
                        {
                            GameManager.ins.CanPlayLevel = false;
                            AdManager.instance.ShowInter(null, null, "ShowInter");
                        }
                    }


                }*/
    }

    void StartCountdown(float time)
    {
        RemaingTime_txt.text = $"{(int)time}";

        tweenCountTime = DOTween.To(() => time, x => time = x, 1, time).SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                RemaingTime_txt.text = $"{(int)time}";
            })
            .OnComplete(() =>
            {
                AdManager.instance.ShowInter(null, null, "ShowInter");
                gameObject.SetActive(false);
            }).SetUpdate(true);

        T_Fill = Fill_bar.DOFillAmount(0f, 6f).SetEase(Ease.Linear).SetUpdate(true);
    }

    public void PlayCoroutineCanPlay()
    {
        coCanPlay = StartCoroutine(IE_DelayChangeCanPlay());
    }

    public IEnumerator IE_DelayChangeCanPlay()
    {
        yield return new WaitForSeconds(0.1f);

        /*        if (GameManager.ins.CurrentGameState != E_GameState.Home)
                {
                    GameManager.ins.CanPlayLevel = true;
                }*/
    }


    private void OnDisable()
    {
        tweenCountTime?.Kill();
        T_Fill?.Kill();

        if (coCanPlay != null)
        {
            StopCoroutine(coCanPlay);
        }
    }
}
