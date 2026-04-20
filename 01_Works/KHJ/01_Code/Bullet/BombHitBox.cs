using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.ObjectPool.RunTime;
using _01_Works.HS.Code.Players;
using _01_Works.JY._01_Scripts.Bullet.SpecialBullet;
using UnityEngine;

public class BombHitBox : MonoBehaviour
{
    [SerializeField] private float _range;
    [SerializeField] private ParticleSystem ps;
    private bool wasPlaying;

    [SerializeField] private LayerMask whatIsTargets;

    [SerializeField] private Material reflectMaterial;

    public GameObject GameObject => gameObject;

    public Entity enemy { get; set; }
    public bool IsReflect { get; set; }

    [SerializeField] private bool isSpecial;

    private Rigidbody _rigid;
    private float _originalSpeed;

    protected Pool _pool;

    private BulletParent _myParent;

    void Update()
    {
        if (!wasPlaying && ps.isPlaying)
        {
            Vector3 pos = transform.position;
            pos.y = 0;

            Collider[] hits = Physics.OverlapSphere(pos, _range, whatIsTargets);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    if (!isSpecial)
                    {
                        damageable.ApplyDamage(1, enemy);
                    }
                }
            }
        }

        wasPlaying = ps.isPlaying;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        pos.y = 0;
        Gizmos.DrawWireSphere(pos, _range);
    }
}