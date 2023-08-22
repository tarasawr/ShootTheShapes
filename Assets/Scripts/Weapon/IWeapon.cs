using UnityEngine;

namespace Taras.Weapon
{
    public interface IWeapon
    {
        public void Shoot();
        public void Create(Color color);
    }
}