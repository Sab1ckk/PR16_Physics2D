using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PR16_Physics2D
{
    public class PhysicsController : MonoBehaviour
    {
        [Header("UI элементы")]
        public Text statusText;
        public Text objectInfoText;
        public Text configText;
        public Transform objectsContainer;
        [Header("Кнопки")]
        public Button createObjectButton;
        public Button showConfigButton;
        public Button changeGravityButton;
        public Button clearButton;
        public GameObject objectButtonPrefab;
        [Header("ScriptableObject ссылки")]
        public ObjectDatabase objectDatabase;
        public PhysicsConfig physicsConfig;
        public PhysicsObjectFactory objectFactory;
        [Header("Настройки")]
        public float spawnRadius = 5f;
        public float spawnHeight = 8f;
        readonly List<GameObject> spawnedObjects = new List<GameObject>();
        PhysicsObjectData selectedObject;
        int gravityLevel;

        void Start()
        {
            if (createObjectButton) createObjectButton.onClick.AddListener(CreateRandomObject);
            if (showConfigButton) showConfigButton.onClick.AddListener(ShowConfig);
            if (changeGravityButton) changeGravityButton.onClick.AddListener(ChangeGravity);
            if (clearButton) clearButton.onClick.AddListener(ClearAllObjects);
            if (objectFactory) objectFactory.controller = this;
            ApplyPhysicsConfig();
            PopulateObjectList();
            UpdateStatus("Выберите объект для создания");
            UpdateConfigText();
        }

        void PopulateObjectList()
        {
            if (!objectDatabase || !objectsContainer || !objectButtonPrefab) return;
            foreach (Transform child in objectsContainer) Destroy(child.gameObject);
            foreach (var obj in objectDatabase.allObjects)
            {
                if (!obj) continue;
                var buttonObj = Instantiate(objectButtonPrefab, objectsContainer);
                var label = buttonObj.GetComponentInChildren<Text>(); if (label) label.text = obj.objectName;
                var button = buttonObj.GetComponent<Button>(); var captured = obj;
                if (button) button.onClick.AddListener(() => SelectObject(captured));
            }
        }

        void SelectObject(PhysicsObjectData obj)
        {
            selectedObject = obj;
            if (objectInfoText) objectInfoText.text = obj.GetFullInfo();
            UpdateStatus($"Выбран: {obj.objectName}");
        }

        public void CreateRandomObject()
        {
            if (!objectDatabase || !objectFactory) { UpdateStatus("Назначьте базу объектов и фабрику"); return; }
            var data = selectedObject ? selectedObject : objectDatabase.GetRandomObject();
            if (!data) { UpdateStatus("Нет доступных объектов!"); return; }
            Vector3 position = new Vector3(Random.Range(-spawnRadius, spawnRadius), spawnHeight, 0f);
            var created = objectFactory.CreatePhysicsObject(data, position);
            if (created) { spawnedObjects.Add(created); UpdateStatus($"Создан объект: {data.objectName}. Всего: {spawnedObjects.Count}"); }
        }

        void ShowConfig()
        {
            if (!physicsConfig) return;
            if (configText) configText.text = physicsConfig.GetFullInfo().Replace("\n", "  |  ");
            UpdateStatus("Конфигурация отображена"); Debug.Log(physicsConfig.GetFullInfo());
        }

        void ChangeGravity()
        {
            if (!physicsConfig) return;
            gravityLevel = (gravityLevel + 1) % 5;
            Vector2[] levels = { new Vector2(0f, -9.81f), new Vector2(0f, -4.9f), new Vector2(0f, -1.62f), Vector2.zero, new Vector2(0f, 4.9f) };
            string[] names = { "Земля", "Половина", "Луна", "Невесомость", "Антигравитация" };
            physicsConfig.gravity = levels[gravityLevel]; Physics2D.gravity = physicsConfig.gravity;
            UpdateStatus($"Гравитация: {names[gravityLevel]}"); UpdateConfigText();
        }

        void ClearAllObjects()
        {
            foreach (var obj in spawnedObjects) if (obj) Destroy(obj);
            spawnedObjects.Clear(); selectedObject = null;
            UpdateStatus("Всё очищено. Выберите объект");
            if (objectInfoText) objectInfoText.text = "Информация об объекте";
            Debug.Log("Все объекты удалены");
        }

        void ApplyPhysicsConfig()
        {
            if (!physicsConfig) return;
            Physics2D.gravity = physicsConfig.gravity;
            Time.fixedDeltaTime = physicsConfig.fixedTimestep;
            Physics2D.velocityIterations = physicsConfig.velocityIterations;
            Physics2D.positionIterations = physicsConfig.positionIterations;
            Physics2D.queriesHitTriggers = physicsConfig.queriesHitTriggers;
        }
        void UpdateStatus(string message) { if (statusText) statusText.text = message; }
        void UpdateConfigText()
        {
            if (configText && physicsConfig) configText.text = $"Гравитация: ({physicsConfig.gravity.x:F1}, {physicsConfig.gravity.y:F1})  |  Сложность: {physicsConfig.difficulty}";
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) CreateRandomObject();
            if (Input.GetKeyDown(KeyCode.G)) ChangeGravity();
            if (Input.GetKeyDown(KeyCode.Delete)) ClearAllObjects();
            if (Input.GetMouseButtonDown(0) && Camera.main)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mousePos);
                if (hit)
                {
                    var comp = hit.GetComponent<PhysicsObjectComponent>();
                    if (comp) { comp.ApplyForce(new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 10f))); UpdateStatus($"Сила применена к {hit.gameObject.name}"); }
                }
            }
        }

        public void SetStatus(string message) => UpdateStatus(message);
        public void RegisterCollision(PhysicsObjectComponent component, string other) { Debug.Log($"Столкновение: {component.name} ↔ {other}"); }
    }
}
