using DG.Tweening;
using TMPro;
using UnityEngine;


public class LevelText : MonoBehaviour
{
    public TMP_Text animatingText; // Kéo và th? TextMeshPro GameObject vào ?ây t? Inspector
    public Color startColor = Color.white; // Màu b?t ??u
    public Color endColor = Color.red; // Màu k?t thúc
    public float colorDuration = 1f; // Th?i gian ?? m?t ký t? hoàn thành vi?c ??i màu

    private DOTweenTMPAnimator textAnimator;

    void Start()
    {
        // Kh?i t?o DOTweenTMPAnimator
        textAnimator = new DOTweenTMPAnimator(animatingText);

        // ??t màu ban ??u cho t?t c? các ký t?
        for (int i = 0; i < textAnimator.textInfo.characterCount; i++)
        {
            textAnimator.DOColorChar(i, startColor, colorDuration);
        }

        // B?t ??u hi?u ?ng ??i màu
        AnimateColor();
    }

    void AnimateColor()
    {
        // L?p qua t?ng ký t? trong text
        for (int i = 0; i < textAnimator.textInfo.characterCount; i++)
        {
            int index = i;

            // T?o hi?u ?ng ??i màu cho t?ng ký t?
            textAnimator.DOColorChar(index, endColor, colorDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(index * 0.1f); // T?o ?? tr? gi?a các ký t? ?? t?o hi?u ?ng lan t?a
        }
    }
}
