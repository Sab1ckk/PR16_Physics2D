using System.Collections.Generic;
using UnityEngine;

namespace PR16_Physics2D
{
    public class ObjectDatabase : ScriptableObject
    {
        [Header("База физических объектов")]
        public List<PhysicsObjectData> allObjects = new List<PhysicsObjectData>();
        public PhysicsObjectData GetObjectByName(string name) => allObjects.Find(obj => obj && obj.objectName == name);
        public List<PhysicsObjectData> GetObjectsByCategory(ObjectCategory category) => allObjects.FindAll(obj => obj && obj.category == category);
        public List<PhysicsObjectData> GetObjectsByRarity(ObjectRarity rarity) => allObjects.FindAll(obj => obj && obj.rarity == rarity);
        public PhysicsObjectData GetRandomObject() => allObjects.Count == 0 ? null : allObjects[Random.Range(0, allObjects.Count)];
        public int GetTotalObjectsCount() => allObjects.Count;
    }
}
    
