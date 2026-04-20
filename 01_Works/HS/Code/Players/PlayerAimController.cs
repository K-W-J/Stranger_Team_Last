using _01_Works.HS.Code.Entities;
using UnityEngine;

namespace _01_Works.HS.Code.Players
{
    public class PlayerAimController : MonoBehaviour, IEntityComponent
    {
        public float rotationSpeed = 20f;
        
        private Player _player;
        
        public bool isCanRotate = true;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        private void Update()
        {
            LookAtTargetPos();
        }

        private void LookAtTargetPos()
        {
            if (isCanRotate == false) return;
            if (_player.InputSO.isEnable == false) return;
            if (_player.IsDead) return;
            
            Transform parent = _player.transform;
            Vector3 targetPos = _player.InputSO.GetAimAtCursor(Vector3.up * 1.5f);
            
            if (targetPos != Vector3.zero)
            {
                Vector3 lookDir = targetPos - parent.position;
                lookDir.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                parent.rotation = Quaternion.Lerp(parent.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var start = new Vector3(transform.position.x , 1.5f, transform.position.z);
            Gizmos.DrawLine(start, start + transform.forward * 2f);
        }
    }
}
