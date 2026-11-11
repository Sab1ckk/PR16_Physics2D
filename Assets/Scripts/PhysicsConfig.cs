using UnityEngine;

namespace PR16_Physics2D
{

    public class PhysicsConfig : ScriptableObject
    {
        [Header("Общие настройки")]
        public string configName = "Основная конфигурация";
        public DifficultyLevel difficulty = DifficultyLevel.Normal;
        [Header("Гравитация")]
        public Vector2 gravity = new Vector2(0f, -9.81f);
        public float gravityMultiplier = 1f;
        [Header("Параметры объектов")]
        public float globalMassMultiplier = 1f;
        public float globalDragMultiplier = 1f;
        public float globalBounciness = 0.5f;
        public float globalFriction = 0.4f;
        [Header("Настройки симуляции")]
        public float fixedTimestep = 0.02f;
        public int velocityIterations = 8;
        public int positionIterations = 3;
        public bool autoSimulation = true;
        public bool queriesHitTriggers = true;
        [Header("Настройки триггеров")]
        public bool showTriggerGizmos = true;
        public Color triggerGizmoColor = new Color(0f, 1f, 0f, 0.3f);

        public string GetFullInfo()
        {
            return $"<b>КОНФИГУРАЦИЯ ФИЗИКИ</b>\nНазвание: {configName}\nСложность: {GetDifficultyName()}\n" +
                $"Гравитация: ({gravity.x:F2}, {gravity.y:F2})\nМножитель гравитации: {gravityMultiplier:F2}\n" +
                $"Множитель массы: {globalMassMultiplier:F2}\nМножитель drag: {globalDragMultiplier:F2}\n" +
                $"Упругость: {globalBounciness:F2}\nТрение: {globalFriction:F2}\nFixed Timestep: {fixedTimestep:F3}";
        }

        string GetDifficultyName()
        {
            switch (difficulty)
            {
                case DifficultyLevel.Easy: return "Лёгкая";
                case DifficultyLevel.Normal: return "Обычная";
                case DifficultyLevel.Hard: return "Сложная";
                case DifficultyLevel.Nightmare: return "Кошмар";
                default: return "Неизвестно";
            }
        }

        public void CycleGravity()
        {
            float y = gravity.y;
            if (y <= -9.81f) gravity = new Vector2(0f, -4.9f);
            else if (y <= -4.9f) gravity = new Vector2(0f, -1.62f);
            else if (y <= -1.62f) gravity = Vector2.zero;
            else if (y <= 0f) gravity = new Vector2(0f, 4.9f);
            else gravity = new Vector2(0f, -9.81f);
            Debug.Log($"Гравитация изменена: ({gravity.x:F2}, {gravity.y:F2})");
        }

        public void CycleDifficulty()
        {
            difficulty = (DifficultyLevel)(((int)difficulty + 1) % 4);
            switch (difficulty)
            {
                case DifficultyLevel.Easy: globalMassMultiplier = 0.5f; globalDragMultiplier = 1.5f; globalBounciness = 0.3f; break;
                case DifficultyLevel.Normal: globalMassMultiplier = 1f; globalDragMultiplier = 1f; globalBounciness = 0.5f; break;
                case DifficultyLevel.Hard: globalMassMultiplier = 2f; globalDragMultiplier = 0.5f; globalBounciness = 0.8f; break;
                case DifficultyLevel.Nightmare: globalMassMultiplier = 5f; globalDragMultiplier = 0.2f; globalBounciness = 1f; break;
            }
        }
    }
    public enum DifficultyLevel { Easy, Normal, Hard, Nightmare }
}

