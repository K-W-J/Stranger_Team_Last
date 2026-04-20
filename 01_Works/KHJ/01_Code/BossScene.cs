using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BossScene : MonoSingleton<BossScene>
{
    [SerializeField] private PlayableDirector start;
    [SerializeField] private PlayableDirector death;

    public void BossStart()
    {
        start.Play();
    }

    public void BossDeath()
    {
        death.Play();
    }


}
