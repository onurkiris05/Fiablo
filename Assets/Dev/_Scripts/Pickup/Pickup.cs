using System.Collections;
using UnityEngine;

namespace RPG.Pickup
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private GameObject item;
        [SerializeField] private SphereCollider pickupCollider;
        [SerializeField] private float respawnCooldown = 5f;
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PickupHandler pickupHandler))
            {
                SetItem(false);
                StartCoroutine(ProcessRespawn());
            }
        }

        protected void SetItem(bool state)
        {
            transform.SetChildrenActiveState(state);
            pickupCollider.enabled = state;
        }

        protected IEnumerator ProcessRespawn()
        {
            yield return Helpers.BetterWaitForSeconds(respawnCooldown);
            SetItem(true);
        }
    }
}
