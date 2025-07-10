using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _boardFullText;
    [SerializeField] private TextMeshProUGUI _cantMergeText;

    private Tween _boardFullTween;
    private Coroutine _boardFullCoroutine;

    private Tween _cantMergeTween;
    private Coroutine _cantMergeCoroutine;

    public void PlayBoardFullTextAnimation()
    {
        Vector3 startScale = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 targetScale = Vector3.one;
        float animationDuration = 0.6f;
        float showDuration = 1.5f;

        _boardFullText.gameObject.SetActive(true);
        _boardFullText.transform.localScale = startScale;

        _boardFullTween?.Kill();
        _boardFullTween = _boardFullText.transform.DOScale(targetScale, animationDuration)
            .SetEase(Ease.OutBounce);

        if (_boardFullCoroutine != null)
            StopCoroutine(_boardFullCoroutine);

        _boardFullCoroutine = StartCoroutine(HideAfterDelay(_boardFullText, showDuration));
    }

    public void PlayCantMergeTextAnimation()
    {
        Vector3 startScale = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 targetScale = Vector3.one;
        float animationDuration = 0.5f;
        float showDuration = 1.25f;

        _cantMergeText.gameObject.SetActive(true);
        _cantMergeText.transform.localScale = startScale;

        _cantMergeTween?.Kill();
        _cantMergeTween = _cantMergeText.transform.DOScale(targetScale, animationDuration)
            .SetEase(Ease.OutBounce);

        if (_cantMergeCoroutine != null)
            StopCoroutine(_cantMergeCoroutine);

        _cantMergeCoroutine = StartCoroutine(HideAfterDelay(_cantMergeText, showDuration));
    }

    private IEnumerator HideAfterDelay(TextMeshProUGUI text, float delay)
    {
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(false);
    }
}