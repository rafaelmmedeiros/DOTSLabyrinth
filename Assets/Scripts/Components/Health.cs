using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct Health : IComponentData {

    public float value;
    public float InvicibleTimer;
    public float killTimer;
}
