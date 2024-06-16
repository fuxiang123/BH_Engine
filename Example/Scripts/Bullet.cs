using System.Collections;
using System.Collections.Generic;
using BH_Engine;
using Unity.VisualScripting;
using UnityEngine;
namespace BH_Engine
{
    public class Bullet : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Enemy")
            {
                BulletPoolManager.instance.RecycleBullet(this.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }

}