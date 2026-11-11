using UnityEngine;

namespace PR16_Physics2D
{
    public class PhysicsObjectFactory : MonoBehaviour
    {
        [Header("Ссылки на ScriptableObject")]
        public ObjectDatabase objectDatabase;
        public PhysicsConfig physicsConfig;
        [Header("Префаб")]
        public GameObject defaultObjectPrefab;
        [Header("Спрайты по умолчанию")]
        public Sprite circleSprite;
        public Sprite squareSprite;
        [Header("Физические материалы")]
        public PhysicsMaterial2D bouncyMaterial;
        public PhysicsMaterial2D frictionMaterial;
        public PhysicsController controller;

        public GameObject CreatePhysicsObject(PhysicsObjectData objectData, Vector3 position)
        {
            if (objectData == null) { Debug.LogWarning("PhysicsObjectData равен null!"); return null; }
            GameObject obj = defaultObjectPrefab != null
                ? Instantiate(defaultObjectPrefab, position, Quaternion.identity)
                : new GameObject();
            obj.name = $"Object_{objectData.objectName}";
            obj.transform.position = position;
            obj.transform.localScale = Vector3.one * objectData.scale;
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>() ?? obj.AddComponent<SpriteRenderer>();
            sr.sprite = objectData.sprite != null
                ? objectData.sprite
                : objectData.colliderType == ColliderType.Circle
                    ? (circleSprite != null ? circleSprite : RuntimeSprite)
                    : (squareSprite != null ? squareSprite : RuntimeSprite);
            sr.color = objectData.objectColor;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>() ?? obj.AddComponent<Rigidbody2D>();
            rb.bodyType = objectData.isKinematic ? RigidbodyType2D.Kinematic : objectData.bodyType;
            rb.mass = objectData.mass * (physicsConfig != null ? physicsConfig.globalMassMultiplier : 1f);
            rb.linearDamping = objectData.drag * (physicsConfig != null ? physicsConfig.globalDragMultiplier : 1f);
            rb.angularDamping = objectData.angularDrag;
            rb.gravityScale = objectData.useGravity ? objectData.gravityScale : 0f;
            rb.useAutoMass = false;
            Collider2D collider = SetupCollider(obj, objectData);
            if (collider != null)
            {
                collider.isTrigger = objectData.isTrigger;
                if (objectData.physicsMaterial != null) collider.sharedMaterial = objectData.physicsMaterial;
            }
            PhysicsObjectComponent component = obj.GetComponent<PhysicsObjectComponent>() ?? obj.AddComponent<PhysicsObjectComponent>();
            component.objectData = objectData;
            Debug.Log($"Создан объект: {objectData.objectName} (Масса: {rb.mass:F2}, Тип: {rb.bodyType})");
            return obj;
        }

        static Sprite runtimeSprite;
        static Sprite RuntimeSprite
        {
            get
            {
                if (runtimeSprite != null) return runtimeSprite;
                var texture = new Texture2D(2, 2);
                texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white }); texture.Apply();
                // A 2x2 fallback must occupy a visible world-space area, not 0.02 units.
                runtimeSprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(.5f, .5f), 1f);
                return runtimeSprite;
            }
        }

        Collider2D SetupCollider(GameObject obj, PhysicsObjectData data)
        {
            Collider2D existing = obj.GetComponent<Collider2D>();
            if (existing != null) Destroy(existing);
            switch (data.colliderType)
            {
                case ColliderType.Circle:
                    var circle = obj.AddComponent<CircleCollider2D>(); circle.radius = data.colliderRadius; return circle;
                case ColliderType.Box:
                    var box = obj.AddComponent<BoxCollider2D>(); box.size = data.colliderSize; return box;
                case ColliderType.Capsule:
                    var capsule = obj.AddComponent<CapsuleCollider2D>(); capsule.size = data.colliderSize; return capsule;
                default:
                    return obj.AddComponent<CircleCollider2D>();
            }
        }

        public void ApplyConfigToObject(Rigidbody2D rb)
        {
            if (rb == null || physicsConfig == null) return;
            rb.mass *= physicsConfig.globalMassMultiplier;
            rb.linearDamping *= physicsConfig.globalDragMultiplier;
            rb.gravityScale *= physicsConfig.gravityMultiplier;
        }
    }
}
