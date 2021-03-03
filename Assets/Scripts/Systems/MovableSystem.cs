using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class MovableSystem : SystemBase {

    protected override void OnUpdate() {

        //float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref PhysicsVelocity physicsVelocity, in Movable movable) => {

            var step = movable.direction * movable.speed;
            physicsVelocity.Linear = step;

        }).Schedule();
    }
}
