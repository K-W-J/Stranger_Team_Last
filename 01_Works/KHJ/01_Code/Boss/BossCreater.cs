using _01_Works.KHJ.Boss;
using KWJ.Etc;
using System;
using UnityEngine;

public class BossCreater : MonoSingleton<BossCreater>
{

    [SerializeField] private Tank tank;
    [SerializeField] private Canvas ab;

    public void Create()
    {
        GameObject a = Instantiate(ab, null).gameObject;
        Tank t = Instantiate(tank, BossPoint.Instance.spawnPoint.position, Quaternion.identity);
        t.gameObject.SetActive(true);
    }
}
