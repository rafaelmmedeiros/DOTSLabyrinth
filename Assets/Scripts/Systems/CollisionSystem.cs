using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using Unity.Physics.Systems;

public class CollisionSystem : SystemBase {

    private struct CollisionSystemJob : ICollisionEventsJob {

        public BufferFromEntity<CollisionBuffer> collisions;

        public void Execute(CollisionEvent collisionEvent) {

            if (collisions.Exists(collisionEvent.EntityA))
                collisions[collisionEvent.EntityA].Add(new CollisionBuffer() { entity = collisionEvent.EntityB });

            if (collisions.Exists(collisionEvent.EntityB))
                collisions[collisionEvent.EntityB].Add(new CollisionBuffer() { entity = collisionEvent.EntityA });
        }
    }

    private struct TriggerSystemJob : ITriggerEventsJob {

        public BufferFromEntity<TriggerBuffer> triggers;

        public void Execute(TriggerEvent triggerEvent) {

            if (triggers.Exists(triggerEvent.EntityA))
                triggers[triggerEvent.EntityA].Add(new TriggerBuffer() { entity = triggerEvent.EntityB });

            if (triggers.Exists(triggerEvent.EntityB))
                triggers[triggerEvent.EntityB].Add(new TriggerBuffer() { entity = triggerEvent.EntityA });
        }
    }

    protected override void OnUpdate() {

        var physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
        var simulation = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;

        Entities.ForEach((DynamicBuffer<CollisionBuffer> collisions) => {

            collisions.Clear();

        }).Run();

        // POPULATE TO NOT RETURN NULL
        var collisionJobHandle = new CollisionSystemJob() {

            collisions = GetBufferFromEntity<CollisionBuffer>()

        }.Schedule(simulation, ref physicsWorld, this.Dependency);

        collisionJobHandle.Complete();

        //
        Entities.ForEach((DynamicBuffer<TriggerBuffer> triggers) => {

            triggers.Clear();

        }).Run();

        //
        var triggerJobHandle = new TriggerSystemJob() {

            triggers = GetBufferFromEntity<TriggerBuffer>()

        }.Schedule(simulation, ref physicsWorld, this.Dependency);

        triggerJobHandle.Complete();
    }
}
