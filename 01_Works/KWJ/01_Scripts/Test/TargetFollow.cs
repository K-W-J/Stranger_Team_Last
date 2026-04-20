using UnityEngine;

namespace KWJ.Test
{
    public class TargetFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _height;

        private void Update()
        {
            transform.position = _target.position + Vector3.up * _height;
        }
    }
}