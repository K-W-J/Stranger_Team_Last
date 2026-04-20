using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameFade : MonoSingleton<InGameFade>
{
    [SerializeField] private GameObject _fadeObj;
    private Image _fadeImage;


    private void Awake()
    {
        _fadeImage = _fadeObj.GetComponent<Image>();
    }

    private void Start()
    {
        _fadeImage.gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1);
        seq.Append(_fadeImage.DOFade(0, 0.7f).OnComplete(()
            => _fadeObj.SetActive(false)
        ));
    }

    public void GameOverFadeOut()
    {
        _fadeObj.SetActive(true);
        DOTween.KillAll();
        _fadeImage.DOFade(1, 0.7f).OnComplete(()
            => SceneManager.LoadScene(1)
        );
    }

    public void ClearFadeOut()
    {
        _fadeObj.SetActive(true);
        DOTween.KillAll();
        _fadeImage.DOFade(1, 0.7f).OnComplete(()
            => SceneManager.LoadScene("ClearScene")
        );
    }
}
