using Godot;
using System;
using System.Collections.Generic;

public partial class ScheduleManager : Node3D {

    public static Task GetNextTask(NPC npc) {
        Task newTask = null;


        return newTask;
    }



    /* raycast code if need be
        public override void _PhysicsProcess(double delta) {
            SphereShape3D sphereShape = new SphereShape3D {
                Radius = 20
            };

            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            var query = new PhysicsShapeQueryParameters3D {
                ShapeRid = sphereShape.GetRid(),
                Transform = Transform
            };
            var result = spaceState.IntersectShape(query);
            for (int i = 0; i < result.Count; i++)
                GD.Print(((Node3D)result[i]["collider"]).Name);
        }
    */
}
