using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElanVital.Combat;

namespace ElanVital.Combat
{
    public class BoardHealth : MonoBehaviour, IHealth
    {
        private bool isDead;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
        }
        private int health;
        public int Health
        {
            get
            {
                return health;
            }
        }
        public void ModifyHealth(int value)
        {
            health = Mathf.Max(0, health + value);
            if(health <= 0)
            {
                Destroy(this);
            }
        }
    }
}