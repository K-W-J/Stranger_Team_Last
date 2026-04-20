using UnityEngine;

public class HpBar : MonoSingleton<HpBar>
{
    public void ChangeHpBar(int current, int max)
    {
        float ratio = (float)current / max;
        transform.localScale = new Vector3(ratio, transform.localScale.y, transform.localScale.z);
    }
}
