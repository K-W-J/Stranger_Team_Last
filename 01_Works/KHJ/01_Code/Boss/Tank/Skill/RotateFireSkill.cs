using _01_Works.JY.Managers;
using _01_Works.KHJ.Bullet;
using UnityEngine;

namespace _01_Works.KHJ.Skill
{
    public class RotateFireSkill : TankSkill
    {
        public bool IsAttacking { get; set; } = false;

        [SerializeField] private float _attackTime; // 총 공격시간
        [SerializeField] private float _rotate; // 돌릴 각도
        private float _currentTime = 0;


        public override void UseSkill()
        {
            base.UseSkill();
            IsAttacking = true;
        }

        protected override void Update()
        {
            base.Update();
            if (!IsAttacking) return;

            _currentTime += Time.deltaTime;

            // 목표 각도와 시간으로 계산
            float rotationThisFrame = (_rotate / _attackTime) * Time.deltaTime;

            Vector3 euler = _tank.GunBarrel.rotation.eulerAngles;
            euler.y += rotationThisFrame; // Y값 1 증가
            _tank.GunBarrel.rotation = Quaternion.Euler(euler);
            FireBullet();
            // 공격 시간 종료
            if (_currentTime > _attackTime)
            {
                _currentTime = 0f;
                IsAttacking = false;
            }
        }

        public async void FireBullet()
        {
            for (int i = 0; i < _tank.muzzles.Length; i++)
            {
                Vector3 euler = _tank.muzzles[i].transform.eulerAngles;
                Bullet.Bullet bullet = BulletManager.Instance.CreateBullet(BulletType.BULLET);
                bullet.transform.position = _tank.muzzles[i].position;
                bullet.transform.rotation = Quaternion.Euler(euler);
                bullet.Fire();

                await Awaitable.WaitForSecondsAsync(0.05f);
            }
        }
    }
}
