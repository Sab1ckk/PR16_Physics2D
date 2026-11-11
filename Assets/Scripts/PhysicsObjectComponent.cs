using UnityEngine;

namespace PR16_Physics2D
{
    public class PhysicsObjectComponent : MonoBehaviour
    {
        [Header("Данные объекта")]
        public PhysicsObjectData objectData;
        Rigidbody2D rb;
        Collider2D col;

        void Awake() { rb = GetComponent<Rigidbody2D>(); col = GetComponent<Collider2D>(); }
        void Start()
        {
            if (objectData != null) Debug.Log($"Объект {objectData.objectName} готов. Масса: {rb.mass:F2}");
        }

        public string GetInfo()
        {
            if (objectData == null) return "Нет данных";
            return objectData.GetFullInfo() + $"\n<b>Текущее состояние:</b>\nСкорость: {rb.linearVelocity.magnitude:F2}\n" +
                $"Позиция: ({transform.position.x:F2}, {transform.position.y:F2})\nУгол: {transform.eulerAngles.z:F1}°\n" +
                $"Спит: {(rb.IsSleeping() ? "Да" : "Нет")}\n";
        }

        public void ApplyForce(Vector2 force)
        {
            if (rb != null) { rb.AddForce(force, ForceMode2D.Impulse); Debug.Log($"Применена сила: {force}"); }
        }
        public void ResetPosition(Vector3 position)
        {
            if (rb == null) return;
            rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; transform.position = position; transform.rotation = Quaternion.identity;
        }
        public void SetKinematic(bool kinematic)
        {
            if (rb != null) rb.bodyType = kinematic ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        }

        void OnCollisionEnter2D(Collision2D collision) => Debug.Log($"<color=yellow>[Столкновение]</color> {name} столкнулся с {collision.gameObject.name}. Сила: {collision.relativeVelocity.magnitude:F2}");
        void OnCollisionExit2D(Collision2D collision) => Debug.Log($"<color=cyan>[Столкновение]</color> {name} перестал касаться {collision.gameObject.name}");
        void OnTriggerEnter2D(Collider2D other) => Debug.Log($"<color=green>[Триггер]</color> {name} вошёл в триггер {other.gameObject.name}");
        void OnTriggerExit2D(Collider2D other) => Debug.Log($"<color=magenta>[Триггер]</color> {name} вышел из триггера {other.gameObject.name}");
    }
}
