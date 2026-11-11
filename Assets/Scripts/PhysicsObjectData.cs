using UnityEngine;

namespace PR16_Physics2D
{
    
    public class PhysicsObjectData : ScriptableObject
    {
        [Header("Основная информация")]
        public string objectName = "Новый объект";
        [TextArea(3, 5)] public string description = "Описание объекта";
        [Header("Физические параметры")]
        public float mass = 1f;
        public float drag = 0f;
        public float angularDrag = 0.05f;
        public float gravityScale = 1f;
        public bool useGravity = true;
        public bool isKinematic = false;
        public RigidbodyType2D bodyType = RigidbodyType2D.Dynamic;
        [Header("Коллайдер")]
        public ColliderType colliderType = ColliderType.Circle;
        public float colliderRadius = 0.5f;
        public Vector2 colliderSize = new Vector2(1f, 1f);
        public PhysicsMaterial2D physicsMaterial;
        [Header("Триггер")]
        public bool isTrigger = false;
        [Header("Визуал")]
        public Sprite sprite;
        public Color objectColor = Color.white;
        public float scale = 1f;
        [Header("Категория")]
        public ObjectCategory category = ObjectCategory.Ball;
        public ObjectRarity rarity = ObjectRarity.Common;

        public string GetFullInfo()
        {
            string info = $"<b>{objectName}</b>\n";
            info += $"<i>{description}</i>\n\n";
            info += $"Категория: {GetCategoryName()}\nРедкость: {GetRarityName()}\n";
            info += $"Масса: {mass:F2}\nDrag: {drag:F2}\nAngular Drag: {angularDrag:F2}\n";
            info += $"Gravity Scale: {gravityScale:F2}\nТип тела: {bodyType}\n";
            info += $"Коллайдер: {colliderType}\nТриггер: {(isTrigger ? "Да" : "Нет")}\n";
            return info;
        }

        string GetCategoryName()
        {
            switch (category)
            {
                case ObjectCategory.Ball: return "Шар";
                case ObjectCategory.Box: return "Ящик";
                case ObjectCategory.Platform: return "Платформа";
                case ObjectCategory.Trigger: return "Триггер";
                default: return "Неизвестно";
            }
        }

        string GetRarityName()
        {
                switch (rarity)
                {
                    case ObjectRarity.Common: return "Обычный";
                    case ObjectRarity.Rare: return "Редкий";
                    case ObjectRarity.Epic: return "Эпический";
                    case ObjectRarity.Legendary: return "Легендарный";
                    default: return "Неизвестно";
                }
        }
    }

    public enum ColliderType { Circle, Box, Capsule, Polygon }
    public enum ObjectCategory { Ball, Box, Platform, Trigger }
    public enum ObjectRarity { Common, Rare, Epic, Legendary }
}
