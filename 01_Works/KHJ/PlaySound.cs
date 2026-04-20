using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlaySfx("BossDown");
    }
}
