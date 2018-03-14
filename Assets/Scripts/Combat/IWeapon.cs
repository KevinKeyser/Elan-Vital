using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElanVital.Combat
{
    public interface IWeapon : IDamage
    {
        bool RequiresBothHands { get; }
    }
}
