using _01_Works.HS.Code.Entities;
using _01_Works.HS.Code.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _01_Works.KHJ.Skill
{
    public class SkillComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private int maxCheckEnemyCount = 5;

        public Collider[] Colliders { get; private set; }

        private Entity _entity;
        private Dictionary<Type, Skill> _skillDict;

        [field: SerializeField] public Skill CurrentSkill { get; set; }
        [field: SerializeField] public GameEventChannelSO CameraChannel { get; private set; }

        public void Initialize(Entity entity)
        {
            _entity = entity;
            Colliders = new Collider[maxCheckEnemyCount];
            _skillDict = new Dictionary<Type, Skill>();

            GetComponentsInChildren<Skill>().ToList()
                .ForEach(skill => _skillDict.Add(skill.GetType(), skill));

            _skillDict.Values.ToList()
                .ForEach(skill => skill.InitializeSkill(_entity, this));
        }

        public T GetSkill<T>() where T : Skill
        {
            Type skillType = typeof(T);
            Skill skill = _skillDict.GetValueOrDefault(skillType);
            Debug.Assert(skill != default,
                $"Skill of type {skillType} not found in {gameObject.name}");

            return skill as T;
        }

        public void AddSkill(Skill skill)
        {
            _skillDict.Add(skill.GetType(), skill);
        }

        public void RemoveSkill(Skill skill)
        {
            _skillDict.Remove(skill.GetType());
        }

        public int GetEnemiesInRange(Vector3 position, float range)
            => Physics.OverlapSphereNonAlloc(position, range, Colliders, whatIsTarget);

        public Entity FindClosestEnemy(Vector3 position, float range)
        {
            Entity target = null;

            int cnt = GetEnemiesInRange(position, range);
            float closestDistance = float.MaxValue;
            for (int i = 0; i < cnt; i++)
            {
                if (Colliders[i].TryGetComponent(out Entity entity) == false || entity.IsDead)
                    continue;

                float distance = Vector3.Distance(position, entity.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = entity;
                }
            }

            return target;
        }
    }
}