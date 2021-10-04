using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    public class UnderAttackCollider : MonoBehaviour
    {
        public float maxHP;
        public float curHP;
        public float maxSuperArmor;
        public float curSuperArmor;

        [SerializeField] private Animator animator;

        public void UnderAttack(float damage)
        {
            if ((curHP -= damage) <= 0)
            {
                curHP = 0;
                return;
            }
            if ((curSuperArmor -= damage) <= 0)
            {
                curSuperArmor = 0;
                animator?.SetTrigger("UnderAttack");
            }
        }

        private void RecoverySuperArmor()
        {
            curSuperArmor = maxSuperArmor;
        }

        private void OnValidate()
        {
            if (animator == null) animator = GetComponent<Animator>();
        }
    }
}