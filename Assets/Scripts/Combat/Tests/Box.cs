using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElanVital.Combat;
using UnityEngine;

namespace ElanVital.Combat
{
    public class Box : MonoBehaviour, IHealth
    {
        public bool IsDead => health <= 0;

        [SerializeField]
        private int health;
        public int Health => health;

        public void ModifyHealth(int value)
        {
            health = Mathf.Max(0, health + value);
        }
    }
}
