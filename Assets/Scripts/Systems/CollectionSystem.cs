using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CollectionSystem : SystemBase {

    protected override void OnUpdate() {

        var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

        Entities
            .WithAll<Player>()
            .ForEach((DynamicBuffer<TriggerBuffer> triggerBuffer) => {

                for (int i = 0; i < triggerBuffer.Length; i++) {

                    var entity = triggerBuffer[i].entity;

                    if (HasComponent<Collectable>(entity) && HasComponent<Kill>(entity)) {
                        ecb.AddComponent(entity, new Kill() { timer = 0 });
                    }

                }

            }).Run();
    }
}
