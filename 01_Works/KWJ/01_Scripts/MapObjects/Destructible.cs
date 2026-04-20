using _01_Works.HS.Code.Combat;
using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using _01_Works.KHJ;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour, IDamageable
{
    public UnityEvent OnDestructionEvent;
    
    [SerializeField] private GameEventChannelSO _cameraChannel;
    [SerializeField] private float _impulseForce;
    [Space]
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _breakVisual;
    [SerializeField] private Collider _collider;
    [Space]
    [SerializeField] private ParticleSystem _particleSystem;

    private void Start()
    {
        _breakVisual.SetActive(false);
    }

    [ContextMenu("DamageTest")]
    private void DamageTest()
    {
        ApplyDamage(0, null);
    }

    public void ApplyDamage(int damage, Entity dealer)
    {
        _particleSystem.Play();
        _visual.SetActive(false);
        _breakVisual.SetActive(true);
        _collider.enabled = false;
        
        ImpulseEvent evt = CameraEvents.ImpulseEvent.Initializer(_impulseForce);
        _cameraChannel.RaiseEvent(evt);
        
        OnDestructionEvent?.Invoke();
    }
}
