using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MovableSystem : SystemBase {

    protected override void OnUpdate() {

        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Movable movable, ref Translation translation, ref Rotation rotation) => {

            translation.Value += movable.speed * movable.direction * deltaTime;
            rotation.Value = math.mul(rotation.Value.value, quaternion.RotateY(movable.speed * deltaTime));

        }).Schedule();
    }
}
