using UnityEngine;

namespace PR16_Physics2D
{
    public class TriggerZone : MonoBehaviour
    {
        [Header("Настройки триггера")]
        public string zoneName = "Триггер";
        public Color gizmoColor = new Color(0f, 1f, 0f, 0.3f);
        public bool showGizmos = true;
        [Header("События")]
        public bool destroyOnEnter;
        public bool applyForceOnEnter;
        public Vector2 forceToApply = new Vector2(0f, 10f);
        [Header("Статистика")]
        public int objectsEntered;
        public int objectsExited;
        void Start() { var col = GetComponent<Collider2D>(); if (col) col.isTrigger = true; }
        void OnTriggerEnter2D(Collider2D other)
        {
            objectsEntered++;
            Debug.Log($"<color=green>[TriggerZone:{zoneName}]</color> {other.name} вошёл в зону. Всего: {objectsEntered}");
            if (applyForceOnEnter) { var rb = other.GetComponent<Rigidbody2D>(); if (rb) rb.AddForce(forceToApply, ForceMode2D.Impulse); }
            if (destroyOnEnter) Destroy(other.gameObject);
        }
        void OnTriggerExit2D(Collider2D other)
        {
            objectsExited++;
            Debug.Log($"<color=magenta>[TriggerZone:{zoneName}]</color> {other.name} вышел из зоны. Всего: {objectsExited}");
        }
        void OnDrawGizmos()
        {
            if (!showGizmos) return;
            Gizmos.color = gizmoColor;
            var box = GetComponent<BoxCollider2D>();
            if (box) Gizmos.DrawCube(transform.position + (Vector3)box.offset, box.size);
            else { var circle = GetComponent<CircleCollider2D>(); if (circle) Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius); }
        }
    }
}
