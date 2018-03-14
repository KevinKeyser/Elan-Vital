using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElanVital.Combat
{
    public interface IHealth
    {
        bool IsDead { get; }
        int Health { get; }
        void ModifyHealth(int value);
    }
}