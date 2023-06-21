using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        public Vector3 GetWaypoint(int i) => transform.GetChild(i).position;
        public int GetIndex(int i) => i % transform.childCount;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            for (int i = 0; i < transform.childCount; i++)
            {
                var first = GetIndex(i);
                var second = GetIndex(i + 1);
                Gizmos.DrawLine(GetWaypoint(first), GetWaypoint(second));
                Gizmos.DrawSphere(GetWaypoint(i), 0.3f);
            }
        }
    }
}