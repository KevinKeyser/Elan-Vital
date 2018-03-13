using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ElanVital.Combat
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField]
        private bool requiresBothHands = false;
        public bool RequiresBothHands => requiresBothHands;

        [SerializeField]
        [Range(1, short.MaxValue)]
        private int damage = 1;
        public int Damage => damage;

        [SerializeField]
        private Collider[] damageColliders = new Collider[0];

        public void DealDamage(IHealth health)
        {
            health.ModifyHealth(-damage);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject)
            {
                var health = collision.gameObject.GetComponent<IHealth>();
                if (health != null)
                {
                    DealDamage(health);
                }
            }
        }
    }
}
