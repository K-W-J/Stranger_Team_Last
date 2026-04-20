using _01_Works.HS.Code.Effects;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.JY._01_Scripts.Dependencies;
using _01_Works.JY.Managers;
using _01_Works.KHJ.Boss;
using _01_Works.KHJ.Bullet;
using Blade.SkillSystem;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class TankAttackCompo : MonoBehaviour, IEntityComponent
{
    [SerializeField] private PoolItemSO _bombEffect;
    [SerializeField] private PoolItemSO _tankFireEffect;
    [SerializeField] private PoolItemSO _bombDecal;

    [SerializeField] private float bulletCnt;
    [SerializeField] private Transform[] muzzles;
    [SerializeField] private Transform[] pivots;
    [SerializeField] private Transform _gunBarrel;
    [SerializeField] private Tank _tank;

    float _maxTime = 0.8f;
    private Entity _entity;
    private bool _canAttack = true;

    // 회전공격
    [SerializeField] private float _atkDelay; // 총알 발사 간격
    [SerializeField] private float _rotate; // 초당 돌릴 각도
    private float lastAttackTime = -Mathf.Infinity;

    public bool IsRotateing { get; set; } = false;

    public void StartRotate(float angle)
    {
        IsRotateing = true;
        _rotate = angle;
    }
    public void StopRotate()
    {
        IsRotateing = false;
    }

    public void SetFireTime(float time)
    {
        _maxTime = time;
    }
    private void Update()
    {
        if (!IsRotateing) return;
        // 목표 각도와 시간으로 계산
        float rotationThisFrame = _rotate * Time.deltaTime;

        Vector3 euler = _gunBarrel.rotation.eulerAngles;
        euler.y += rotationThisFrame; // Y값 1 증가
        _gunBarrel.rotation = Quaternion.Euler(euler);
        
    }




    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    public void FireSplitting()
    {
        for (int i = 0; i < muzzles.Length; i++)
        {
            Vector3 euler = muzzles[i].transform.eulerAngles;
            Bullet bullet = BulletManager.Instance.CreateBullet(BulletType.SPLITTING_BULLET);
            bullet.transform.position = muzzles[i].position;
            bullet.transform.rotation = Quaternion.Euler(euler);
            bullet.Fire();
        }
    }


    public void FireBullet(bool isFuulAttack)
    {
        if (!_canAttack) return;
        for (int i = 0; i < muzzles.Length; i++)
        {
            if (!isFuulAttack)
            {
                if (i % 2 != 0)
                {
                    int num = UnityEngine.Random.Range(1, 100);
                    Vector3 euler = muzzles[i].transform.eulerAngles;
                    Bullet bullet = num >= 98 ? BulletManager.Instance.CreateBullet(BulletType.SPLITTING_BULLET) : BulletManager.Instance.CreateBullet(BulletType.BULLET);
                    bullet.transform.position = muzzles[i].position;
                    bullet.transform.rotation = Quaternion.Euler(euler);
                    bullet.speed *= UnityEngine.Random.Range(0.7f, 1.3f);
                    bullet.Fire();
                    AudioManager.Instance.PlaySfx("GUN1");
                }
            }
            else
            {
                int num = UnityEngine.Random.Range(1, 100);
                Vector3 euler = muzzles[i].transform.eulerAngles;
                Bullet bullet = num >= 98 ? BulletManager.Instance.CreateBullet(BulletType.SPLITTING_BULLET) : BulletManager.Instance.CreateBullet(BulletType.BULLET);
                bullet.transform.position = muzzles[i].position;
                bullet.transform.rotation = Quaternion.Euler(euler);
                bullet.speed *= UnityEngine.Random.Range(0.7f, 1.3f);
                bullet.Fire();
                AudioManager.Instance.PlaySfx("GUN1");
            }
        }
    }

    public void AirAndGroundFire()
    {
        _canAttack = false;
        _maxTime = 0.40f;
        StartCoroutine(AirFireCoroutine(false));
    }

    public void AirFire()
    {
        _maxTime = 0.35f;
        StartCoroutine(AirFireCoroutine(true));
    }

    public IEnumerator AirFireCoroutine(bool isFuulAttack)
    {
        for (int i = 0; i < pivots.Length; i++)
        {
            if (!isFuulAttack)
            {
                if (i % 2 == 0)
                    pivots[i].DOLocalRotate(new Vector3(-55f, pivots[i].localEulerAngles.y, pivots[i].localEulerAngles.z), 2);
            }
            else
                pivots[i].DOLocalRotate(new Vector3(-55f, pivots[i].localEulerAngles.y, pivots[i].localEulerAngles.z), 2);
        }
        yield return new WaitForSeconds(2);

        StartRotate(isFuulAttack ? 200 : 300);
        _canAttack = true;
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < (isFuulAttack ? 14 : 12); i++)
        {
            for (int j = 0; j < muzzles.Length; j++)
            {
                if (!isFuulAttack)
                {
                    if (j % 2 == 0)
                    {
                        Vector3 euler = muzzles[j].transform.eulerAngles;
                        PoolingEffect effect1 = PoolManagerMono.Instance.Pop<PoolingEffect>(_tankFireEffect);
                        effect1.PlayVFX(muzzles[j].position, Quaternion.Euler(euler));
                        StartCoroutine(PushPoolITem(effect1));

                        StartCoroutine(WaitToBomb(1, j));

                        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, _maxTime));
                    }
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, muzzles.Length);
                    Vector3 euler = muzzles[index].transform.eulerAngles;

                    PoolingEffect effect1 = PoolManagerMono.Instance.Pop<PoolingEffect>(_tankFireEffect);
                    StartCoroutine(PushPoolITem(effect1));

                    effect1.PlayVFX(muzzles[index].position, Quaternion.Euler(euler));
                    AudioManager.Instance.PlaySfx("BossFire");

                    StartCoroutine(WaitToBomb(1.5f, index));

                    yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, _maxTime));
                }
                
            }
        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < pivots.Length; i++)
        {
            int index = i;  //  클로저용 안전 변수

            Vector3 targetEuler = pivots[index].rotation.eulerAngles;
            targetEuler.x = 0f;

        }

        yield return new WaitForSeconds(1f);
        _canAttack = false;
        StopRotate();
        
        for (int i = 0; i < pivots.Length; i++)
        {
            if (!isFuulAttack)
            {
                if (i % 2 == 0)
                {
                    pivots[i].DOLocalRotate(new Vector3(0f, pivots[i].localEulerAngles.y, pivots[i].localEulerAngles.z), 1f);
                }
            }
            else
                pivots[i].DOLocalRotate(new Vector3(0f, pivots[i].localEulerAngles.y, pivots[i].localEulerAngles.z), 1f);
        }
        yield return new WaitForSeconds(4f);
        _canAttack = true;
    }

    private IEnumerator WaitToBomb(float time, int index)
    {
        PoolingEffect effect2 = PoolManagerMono.Instance.Pop<PoolingEffect>(_bombEffect);
        
        StartCoroutine(PushPoolITem(effect2));
        //Vector3 pos = _entity.transform.position + muzzles[index].forward * 20;
        Vector3 pos = _entity.transform.position + muzzles[index].forward *
                 Mathf.Clamp((Vector3.Distance(_tank.transform.position, _tank.PlayerFinder.Target.transform.position) * 2 + UnityEngine.Random.Range(-5, 5)), 6, 50) ;
        pos.y = 15f;
        RoundDecal decal = PoolManagerMono.Instance.Pop<RoundDecal>(_bombDecal);
        decal.transform.position = new Vector3(pos.x, 0, pos.z);

        var projector = decal.decalProjector;

        projector.size = Vector3.zero;

        Vector3 targetSize = new Vector3(9.5f, 9.5f, 9.5f);
        DOTween.To(() => projector.size, x => projector.size = x, targetSize, time - 0.5f)
            .SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(time);
        effect2.PlayVFX(pos, Quaternion.Euler(90f, 0f, 0f));
        yield return new WaitForSeconds(0.2f);
        decal.Pade();
    }



    public void StartCooldown()
    {
        lastAttackTime = Time.time;
    }

    public bool IsCooldownComplete()
    {
        return Time.time >= lastAttackTime + _atkDelay;
    }

    public float GetRemainingCooldown()
    {
        return Mathf.Max(0f, (lastAttackTime + _atkDelay) - Time.time);
    }

    private IEnumerator PushPoolITem(IPoolable effect)
    {
        yield return new WaitForSeconds(4f);
        PoolManagerMono.Instance.Push(effect);
    }
}
