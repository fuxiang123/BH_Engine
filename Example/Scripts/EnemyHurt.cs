using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BH_Engine.Example
{
    public class EnemyHurt : MonoBehaviour
    {

        // 处理碰撞
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Enemy Hurt" + other.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}