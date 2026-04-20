using UnityEngine;

namespace KWJ.Etc
{
    public class RotateTowardCamera : MonoBehaviour
    {
        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_camera.transform.position - transform.position);
        }
    }
}