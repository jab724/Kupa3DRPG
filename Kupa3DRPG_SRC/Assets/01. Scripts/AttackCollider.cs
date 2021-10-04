using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    [RequireComponent(typeof(Rigidbody))]
    public class AttackCollider : MonoBehaviour
    {
        [System.Serializable]
        public class AttackInfo
        {
            public GameObject collider;
            public float damage;

            public AttackInfo(GameObject gameObject, float damage = 0f)
            {
                collider = gameObject;
                this.damage = damage;
            }
        }
        [SerializeField] private List<AttackInfo> attackInfos = new List<AttackInfo>();

        private Rigidbody rigidbody;
        private int curSeq;

        public void EnableCollider(int seq)
        {
            attackInfos[seq].collider.SetActive(true);
            curSeq = seq;
        }
        public void DisableCollider(int seq)
        {
            attackInfos[seq].collider.SetActive(false);
        }

        private void Awake()
        {

            foreach (var attackInfo in attackInfos)
            {
                attackInfo.collider.SetActive(false);
            }
        }

        private void OnDisable()
        {
            foreach (var attackInfo in attackInfos)
            {
                attackInfo.collider.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<UnderAttackCollider>().UnderAttack(attackInfos[curSeq].damage);
        }

        private void OnValidate()
        {
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }

            if (attackInfos.Count < transform.childCount)
            {
                attackInfos.Clear();
                for (int i = 0; i < transform.childCount; ++i)
                {
                    attackInfos.Add(new AttackInfo(transform.GetChild(i).gameObject));
                }
            }
        }
    }
}
