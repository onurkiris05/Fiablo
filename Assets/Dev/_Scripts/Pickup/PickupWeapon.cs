using RPG.Combat;
using UnityEngine;

namespace RPG.Pickup
{
    public class PickupWeapon : Pickup
    {
        [SerializeField] private Weapon weapon;

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PickupHandler pickupHandler))
            {
                pickupHandler.ProcessItemPickup(weapon);
                SetItem(false);
                StartCoroutine(ProcessRespawn());
            }
        }
    }
}