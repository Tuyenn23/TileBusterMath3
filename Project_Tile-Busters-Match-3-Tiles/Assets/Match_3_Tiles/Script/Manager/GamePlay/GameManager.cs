using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UiController UiController;

    public bool isInGame = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        CheckUnlockBooster();

        AdManager.instance.ShowBanner();

        AdsHandle.instance.ShowAllAds();

        if (PlayerDataManager.GetCurrentLevel() >= 2)
        {
           AdsHandle.instance.ShowInterAfterTime();
        }

    }

    private void CheckUnlockBooster()
    {
        if (PlayerDataManager.GetCurrentLevel() == 2)
        {
            UiController.ProcessUnlockBooster(E_TypeBooster.Undo);
        }

        if (PlayerDataManager.GetCurrentLevel() == 3)
        {
            UiController.ProcessUnlockBooster(E_TypeBooster.Shuffle);
        }

        if (PlayerDataManager.GetCurrentLevel() == 4)
        {
            UiController.ProcessUnlockBooster(E_TypeBooster.TilesReturn);
        }

        if (PlayerDataManager.GetCurrentLevel() == 5)
        {
            UiController.ProcessUnlockBooster(E_TypeBooster.Merge);
        }

        if (PlayerDataManager.GetCurrentLevel() == 6)
        {
            UiController.ProcessUnlockBooster(E_TypeBooster.AddSlot);
        }
    }

    private void Update()
    {
        CheckAFKAds();
    }

    public void CheckAFKAds()
    {
        if (Input.GetMouseButton(0) && AdsHandle.instance.CanShowInterAFK)
        {
            /*UiController.PanelAds.gameObject.SetActive(false);*/
            UiController.UiGamePlay.remainingShowAds.gameObject.SetActive(false);

        }

        if (Input.GetMouseButton(0) && !AdsHandle.instance.Detected /*&& !isOpenShop)*/)
        {
            AdsHandle.instance.Detected = true;
            AdsHandle.instance.ShowInterAFK();
        }
    }
    public void StartDontInGame()
    {
        StartCoroutine(IE_DelayDontInGame());
    }

    public void StartIsInGame()
    {
        StartCoroutine(IE_DelayIsInGame());
    }

    IEnumerator IE_DelayDontInGame()
    {
        yield return null;
        isInGame = false;
    }

    IEnumerator IE_DelayIsInGame()
    {
        yield return null;
        isInGame = true;
    }
}
