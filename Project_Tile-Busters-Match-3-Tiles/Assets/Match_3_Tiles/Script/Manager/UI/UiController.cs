using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public UiGamePlay UiGamePlay;

    public PopupBooster popupBooster;
    public PopupNoInternet PopupNoInternet;

    public void ProcessWinLose(E_Result Type)
    {
        switch (Type)
        {
            case E_Result.Processing:
                break;
            case E_Result.Win:
                UiGamePlay.OnOpenPopupWin();
                break;
            case E_Result.Lose:
                UiGamePlay.OnOpenPopupLose();
                break;
            default:
                break;
        }
    }

    public void ProcessUnlockBooster(E_TypeBooster _typeBooster)
    {
        switch (_typeBooster)
        {
            case E_TypeBooster.Undo:
                popupBooster.InitBoosterUnlock(_typeBooster);
                popupBooster.gameObject.SetActive(true);
                break;
            case E_TypeBooster.Merge:
                popupBooster.InitBoosterUnlock(_typeBooster);
                popupBooster.gameObject.SetActive(true);
                break;
            case E_TypeBooster.TilesReturn:
                popupBooster.InitBoosterUnlock(_typeBooster);
                popupBooster.gameObject.SetActive(true);
                break;
            case E_TypeBooster.AddSlot:
                popupBooster.InitBoosterUnlock(_typeBooster);
                popupBooster.gameObject.SetActive(true);
                break;
            case E_TypeBooster.Shuffle:
                popupBooster.InitBoosterUnlock(_typeBooster);
                popupBooster.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


}
