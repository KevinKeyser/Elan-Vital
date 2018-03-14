using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElanVital.Combat
{
    public interface IDamage
    {
        int Damage { get; }
        void DealDamage(IHealth health);
    }
}