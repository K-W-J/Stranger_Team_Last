using System.Collections;
using _01_Works.HS.Code.Combat;
using KWJ.OverlapChecker;
using UnityEngine;

namespace KWJ.MapObjects.Trap
{
    public class VisibleCylinder : MonoBehaviour
    {
        [SerializeField] private BoxOverlapChecker _boxChecker;
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private float _speed;
        [SerializeField] private float _distance;
        [SerializeField] private float _startDelay;

        [SerializeField] private int _damage;
        
        private int _direction = 1;

        private bool _isStart;
        
        private void Start()
        {
            StartCoroutine(StartDelay());
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(_startDelay);
            _isStart = true;
        }

        private void Update()
        {
            if(!_isStart) return;
            
            transform.position += transform.forward * (_speed * _direction * Time.deltaTime);
            transform.GetChild(0).Rotate(_speed * 100 * _direction * Time.deltaTime, 0, 0);

            if (Physics.Raycast(transform.position, transform.forward * _direction, _distance, _wallMask))
            {
                if (_direction == 1)
                {
                    _direction = -1;
                }
                else if (_direction == -1)
                {
                    _direction = 1;
                }
            }

            if (_boxChecker.BoxOverlapCheck())
            {
                GameObject[] targets = _boxChecker.GetOverlapData();

                foreach (var target in targets)
                {
                    if (target.TryGetComponent<IDamageable>(out var explosion))
                    {
                        explosion.ApplyDamage(_damage, null);
                    }
                }
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.forward * _distance * _direction);
        }
    }
}