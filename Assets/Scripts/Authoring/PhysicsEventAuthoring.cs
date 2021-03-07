using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PhysicsEventAuthoring : MonoBehaviour, IConvertGameObjectToEntity {

    public float health;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

        dstManager.AddBuffer<CollisionBuffer>(entity);
        dstManager.AddBuffer<TriggerBuffer>(entity);

    }
}
