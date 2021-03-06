﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CollectionSystem : SystemBase {
    protected override void OnUpdate() {
        var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

        Entities
            .WithAll<Player>()
            .ForEach((Entity playerEntity, DynamicBuffer<TriggerBuffer> triggerBuffer) => {

                for (int i = 0; i < triggerBuffer.Length; i++) {

                    var entity = triggerBuffer[i].entity;
                    if (HasComponent<Collectable>(entity) && !HasComponent<Kill>(entity)) {
                        ecb.AddComponent(entity, new Kill() { timer = 0 });
                        //GameManager.instance.AddPoints(GetComponent<Collectable>(e).points);
                    }

                    if (HasComponent<PowerPill>(entity) && !HasComponent<Kill>(entity)) {
                        ecb.AddComponent(playerEntity, GetComponent<PowerPill>(entity));
                        ecb.AddComponent(entity, new Kill() { timer = 0 });
                    }
                }
            }).WithoutBurst().Run();
    }
}
