using RPG.Combat;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    PlayerController player;

    public void Init(PlayerController player)
    {
        this.player = player;
    }

    public void ProcessItemPickup(Object item)
    {
        switch (item)
        {
            case Weapon weapon:
                player.EquipWeapon(weapon);
                break;
        }
    }
}