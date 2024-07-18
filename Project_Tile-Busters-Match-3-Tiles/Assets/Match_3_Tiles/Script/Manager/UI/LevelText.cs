using DG.Tweening;
using TMPro;
using UnityEngine;


public class LevelText : MonoBehaviour
{
    public TMP_Text animatingText; // K�o v� th? TextMeshPro GameObject v�o ?�y t? Inspector
    public Color startColor = Color.white; // M�u b?t ??u
    public Color endColor = Color.red; // M�u k?t th�c
    public float colorDuration = 1f; // Th?i gian ?? m?t k� t? ho�n th�nh vi?c ??i m�u

    private DOTweenTMPAnimator textAnimator;

    void Start()
    {
        // Kh?i t?o DOTweenTMPAnimator
        textAnimator = new DOTweenTMPAnimator(animatingText);

        // ??t m�u ban ??u cho t?t c? c�c k� t?
        for (int i = 0; i < textAnimator.textInfo.characterCount; i++)
        {
            textAnimator.DOColorChar(i, startColor, colorDuration);
        }

        // B?t ??u hi?u ?ng ??i m�u
        AnimateColor();
    }

    void AnimateColor()
    {
        // L?p qua t?ng k� t? trong text
        for (int i = 0; i < textAnimator.textInfo.characterCount; i++)
        {
            int index = i;

            // T?o hi?u ?ng ??i m�u cho t?ng k� t?
            textAnimator.DOColorChar(index, endColor, colorDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(index * 0.1f); // T?o ?? tr? gi?a c�c k� t? ?? t?o hi?u ?ng lan t?a
        }
    }
}
