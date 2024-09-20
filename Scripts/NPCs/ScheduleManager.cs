using Godot;
using System;
using System.Collections.Generic;

public partial class ScheduleManager : Node3D {

    public static Task GetNextTask(NPC npc, List<Task> potentialTasks) {
        Task newTask = null;


        return newTask;
    }

    public static Task GetNeedTask(NPC npc, Need need, List<Task> tasks) {
        Task newTask = null;

        PriorityQueue<Task, float> potentialTasks = new();
        // List<Task> potentialTasks = new List<Task>();
        foreach (Task task in tasks) {
            if (task.GetNeed == need)
                potentialTasks.Enqueue(task, task.GetTaskScore(npc.Position));
            //potentialTasks. Add(task);
        }

        // newTask = GetBestTask(npc.Position, potentialTasks);
        potentialTasks.TryDequeue(out newTask, out float priority);

        if (newTask == null) return null;

        GD.PrintS(newTask, !newTask.IsBeingUsed);

        //check if we can do the task, if not, get the next one
        while (newTask.IsBeingUsed) {
            potentialTasks.TryDequeue(out newTask, out priority);
            if (newTask == null)
                break;
        }

        return newTask;
    }

    public static Task GetJobTask(NPC npc) {
        Task newTask = null;


        return newTask;
    }

    public static Task GetHobbyTask(NPC npc) {
        Task newTask = null;


        return newTask;
    }

    public static Task GetBestTask(Vector3 pos, List<Task> potentialTasks) {
        Task bestTask = null;
        float bestScore = float.MaxValue;

        foreach (Task task in potentialTasks) {
            float score = task.GetTaskScore(pos);
            if (bestScore > score) {
                bestScore = score;
                bestTask = task;
            }
        }
        return bestTask;
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
