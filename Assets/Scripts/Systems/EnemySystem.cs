using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class EnemySystem : SystemBase {

    private Unity.Mathematics.Random random = new Unity.Mathematics.Random(1234);

    protected override void OnUpdate() {

        var raycaster = new MovementRaycast() { physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld };
        random.NextInt();
        var randomTemp = random;

        Entities.ForEach((ref Movable movable, ref Enemy enemy, in Translation translation) => {

            if (math.distance(translation.Value, enemy.previousCell) > 0.9) {

                enemy.previousCell = math.round(translation.Value);

                var validDirection = new NativeList<float3>(Allocator.Temp);

                if (!raycaster.CheckRay(translation.Value, new float3(0, 0, -1), movable.direction))
                    validDirection.Add(new float3(0, 0, -1));

                if (!raycaster.CheckRay(translation.Value, new float3(0, 0, 1), movable.direction))
                    validDirection.Add(new float3(0, 0, 1));

                if (!raycaster.CheckRay(translation.Value, new float3(1, 0, 0), movable.direction))
                    validDirection.Add(new float3(1, 0, 0));

                if (!raycaster.CheckRay(translation.Value, new float3(-1, 0, 0), movable.direction))
                    validDirection.Add(new float3(-1, 0, 0));

                movable.direction = validDirection[randomTemp.NextInt(validDirection.Length)];

                validDirection.Dispose();
            }

        }).Schedule();
    }


    private struct MovementRaycast {

        [ReadOnly] public PhysicsWorld physicsWorld;

        public bool CheckRay(float3 position, float3 direction, float3 currentDirection) {

            if (direction.Equals(-currentDirection))
                return true;

            var raycaster = new RaycastInput() {

                Start = position,
                End = position + (direction * 0.9f),
                Filter = new CollisionFilter() {
                    GroupIndex = 0,
                    BelongsTo = 1u << 1,
                    CollidesWith = 1u << 2
                }
            };

            return physicsWorld.CastRay(raycaster);
        }
    }
}
