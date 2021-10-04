using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    public class PlayerAttackBoxCtrl : MonoBehaviour
    {
        public AttackCollider swordAttack;
        public AttackCollider shieldAttack;

        public void Meele_SwordAttack_Enable(int seq)
        {
            swordAttack.EnableCollider(seq);
        }
        public void Meele_SwordAttack_Disable(int seq)
        {
            swordAttack.DisableCollider(seq);
        }
        public void Meele_ShieldAttack_Enable(int seq)
        {
            shieldAttack.EnableCollider(seq);
        }
        public void Meele_ShieldAttack_Disable(int seq)
        {
            shieldAttack.DisableCollider(seq);
        }
    }
}