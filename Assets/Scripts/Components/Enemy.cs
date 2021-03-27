using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Enemy : IComponentData {
    
    public float3 previousCell;
}
