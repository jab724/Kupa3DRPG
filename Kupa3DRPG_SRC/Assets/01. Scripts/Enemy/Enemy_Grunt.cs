using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Kupa
{
    public class Enemy_Grunt : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            StartCoroutine("FollowPlayer");
        }

        private IEnumerator FollowPlayer()
        {
            while (true)
            {
                navMeshAgent.SetDestination(Player.PublicData.Transform.position);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}