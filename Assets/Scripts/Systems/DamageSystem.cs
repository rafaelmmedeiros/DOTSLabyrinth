using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DamageSystem : SystemBase {

    protected override void OnUpdate() {

        var deltaTime = Time.DeltaTime;

        //
        Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffer, ref Health health) => {

            for (int i = 0; i < collisionBuffer.Length; i++) {

                if (health.InvicibleTimer <= 0 && HasComponent<Damage>(collisionBuffer[i].entity)) {
                    health.value -= GetComponent<Damage>(collisionBuffer[i].entity).value;
                    health.InvicibleTimer = 1;
                }
            }

        }).Schedule();

        //
        Entities.WithNone<Kill>().ForEach((Entity entity, ref Health health) => {

            health.InvicibleTimer -= deltaTime;
            if (health.value <= 0)
                EntityManager.AddComponentData(entity, new Kill() { timer = health.killTimer });

        }).WithStructuralChanges().Run();

        //
        var ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        Entities.ForEach((Entity Entity, int entityInQueryIndex, ref Kill kill) => {

            kill.timer -= deltaTime;
            if (kill.timer <= 0)
                ecb.DestroyEntity(entityInQueryIndex, Entity);

        }).Schedule();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}