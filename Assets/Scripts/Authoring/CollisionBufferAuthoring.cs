﻿using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class CollisionBufferAuthoring : MonoBehaviour, IConvertGameObjectToEntity {

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

        dstManager.AddBuffer<CollisionBuffer>(entity);
    }
}