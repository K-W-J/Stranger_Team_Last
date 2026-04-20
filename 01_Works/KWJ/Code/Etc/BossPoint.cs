using System;
using UnityEngine;

namespace KWJ.Etc
{
    public class BossPoint : MonoSingleton<BossPoint>
    {
        public Transform[] MovePoints;
        public Transform spawnPoint;

        private void Start()
        {
        }
    }
}
