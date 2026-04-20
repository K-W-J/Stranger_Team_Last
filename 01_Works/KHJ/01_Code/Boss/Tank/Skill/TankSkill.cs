using _01_Works.JY.Managers;
using _01_Works.KHJ.Boss;
using _01_Works.KHJ.Bullet;
using UnityEngine;

namespace _01_Works.KHJ.Skill
{
    public class TankSkill : Skill
    {
        protected Tank _tank;

        private void Awake()
        {
            _tank = GetComponentInParent<Tank>();
        }

        protected override void Update()
        {
            base.Update();
        }

        public void FireBullet(Transform muzzle)
        {
            Vector3 euler = muzzle.transform.eulerAngles;

            Bullet.Bullet bullet = BulletManager.Instance.CreateBullet(BulletType.BULLET);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = Quaternion.Euler(euler);
            bullet.Fire();
        }
    }
}