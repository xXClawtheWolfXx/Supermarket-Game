using Godot;
using System;
using Godot.Collections;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Tracing;
using Godot.NativeInterop;
using System.Security.Principal;

public partial class NPC : CharacterBody3D {

    [ExportGroup("Movement")]
    [Export] private Array<RayCast3D> raycasts;
    [Export] private ShapeCast3D shapecast;
    [Export] protected NavigationAgent3D agent;
    [Export] private float speed = 10;
    [Export] private float rotateSpeed = 10;

    [ExportGroup("NPC")]
    [Export] private Array<NPCRelationship> frens = new Array<NPCRelationship>();
    [Export] private Array<Task> schedule = new Array<Task>();
    [Export] private int money;
    [Export] private Node3D house;
    [Export] private Job job;

    [ExportGroup("Tasks")]
    [Export] private Task hobby;
    [Export] private Culture culture;
    [Export] private Array<Task> favoriteTasks = new Array<Task>();
    //dialogue

    //NPC AI Stuff
    private NPCNeed[] npcNeeds = new NPCNeed[4];
    private Task currTask;
    private bool isDoingTask = false;
    private int timeLeftOnTask = 0;

    public override void _Ready() {
        for (int i = 0; i < npcNeeds.Length; i++) {
            Need need = (Need)Enum.Parse(typeof(Need), i.ToString());
            int needAmt = GD.RandRange(60, 80);
            npcNeeds[i] = new NPCNeed(need, needAmt);
        }

        //init schedule
        for (int i = 0; i < GameTime.Instance.GetMaxTimeSlots; i++)
            schedule.Add(null);

        GameTime.Instance.OnTimeIncrease += DecreaseAllNeeds;
        GameTime.Instance.OnTimeIncrease += NextTask;
        NextTask(0);
    }

    //-------------movement----------

    public override void _Process(double delta) {
        //check if customer is interacting
        foreach (RayCast3D raycast in raycasts) {
            if (raycast.IsColliding() && raycast.GetCollider() is IInteractable interactable) {
                interactable.Interact(this);
                break;
            }
        }

        //if at destination, no need to calculate potition
        if (agent.IsNavigationFinished()) {
            if (agent.TargetPosition == NPCSpawner.Instance.Position)
                //GD.Print("Finished---------------------");
                NPCSpawner.Instance.DestroyCustomer(this);
            if (isDoingTask && currTask != null && agent.TargetPosition == currTask.GetTaskPosition) {
                currTask.DoTask(this);
                isDoingTask = false;
            }
            return;
        }

        //move character
        Vector3 nextLoc = agent.GetNextPathPosition();
        Vector3 offset = nextLoc - GlobalPosition;
        Vector3 newVel = offset.Normalized() * speed;

        agent.Velocity = newVel;
        MoveAndSlide();

        //rotation
        offset.Y = 0;
        if (GlobalTransform.Origin.IsEqualApprox(GlobalPosition + offset))
            return;
        LookAt(GlobalPosition + offset, Vector3.Up);
    }



    /// Updates targetPosition to specified position
    public void Move(Vector3 pos) {
        agent.TargetPosition = pos;
    }

    //don't collide with 
    public void OnNavigationAgent3DVelocityComputed(Vector3 safeVel) {
        Velocity = safeVel;
        MoveAndSlide();
    }



    //-------------NPC Stuff----------


    public void NextTask(int time) {
        GD.PrintS(Name, timeLeftOnTask);
        //check if already doing task
        if (timeLeftOnTask > 0) {
            timeLeftOnTask--;
            return;
            //schedule[time] = currTask;
        }
        //get next task
        currTask?.SetBeingUsed(false);
        currTask = schedule[time];
        GD.PrintS(Name, "Looking for Task");
        Need lowest = LowestNeed();
        //find next task
        if (currTask is null) {
            currTask = FindNextTask(time, lowest);
            GD.PrintS(Name, currTask == null);
            //if task is null, do favored task
            if (currTask == null) {
                currTask = DoFavoredTasks();
            }
            GD.PrintS(Name, "passed favor", currTask == null);

            //if task is still null, do culture task 
            if (currTask == null) {
                if (!culture.tasks[time].CheckIfCanDoTask(this)) {
                    currTask = null;
                    return;
                }
                currTask = culture.tasks[time];

            }

            GD.PrintS(Name, "passed culture 2", currTask == null);


            currTask.SetBeingUsed(true);

            GD.PrintS(Name, currTask.ToString());
            //consider hobbies next
        }
        //do task
        timeLeftOnTask = currTask.GetDuration;
        Move(currTask.GetTaskPosition);
        isDoingTask = true;

    }

    private Task FindNextTask(int time, Need lowest) {
        //check if culture thing is neccessary
        Task cultureTask = culture.tasks[time];
        if (cultureTask.GetIsCultureTask) {
            if (cultureTask.CheckIfCanDoTask(this)) {
                GD.PrintS(Name, "is doing culture task");
                return cultureTask;
            }
        }

        //if time is during job time, find job task
        if (job != null && time >= job.GetStartTime && time < job.GetEndTime) {
            Task jobTask = job.GetJobTask(this);
            if (jobTask != null) {
                GD.PrintS(Name, "is doing job task");
                return jobTask;
            }
        }

        List<Task> closest = FindTasksInArea();

        //if needs are low enough, find need task
        PrintNeeds();
        GD.PrintS("needs not high enough", !AreNeedsHighEnough());
        if (!AreNeedsHighEnough()) {
            Task needTask = ScheduleManager.GetNeedTask(this, lowest, closest);
            if (needTask != null) {
                GD.PrintS(Name, "is doing need task");
                //add to lowest need
                npcNeeds[(int)lowest].amount += 10;
                return needTask;
            }
        }

        //do hobby when?
        return null;
    }

    private Task DoFavoredTasks() {
        int randNum = GD.RandRange(0, favoriteTasks.Count - 1);
        Task task = favoriteTasks[randNum];
        //GD.PrintS(Name, task.ToString(), task.CheckIfCanDoTask(this));
        if (task.CheckIfCanDoTask(this)) {
            GD.PrintS(Name, "is doing favored task");
            return task;
        }
        return null;
    }

    public void DecreaseAllNeeds(int time) {
        foreach (NPCNeed npcNeed in npcNeeds) {
            npcNeed.amount -= 10;
        }
    }

    //Is the average 80% or more?
    private bool AreNeedsHighEnough() {
        foreach (NPCNeed n in npcNeeds)
            if (n.amount < 75)
                return false;
        return true;
    }

    public void PrintNeeds() {
        string needs = Name;
        foreach (NPCNeed npcNeed in npcNeeds) {
            needs += npcNeed.need.ToString() + " " + npcNeed.amount + " ";
        }
        GD.PrintS(needs);
    }
    //Find the lowest Need
    public Need LowestNeed() {
        Need lowest = Need.NONE;
        int best = int.MaxValue;
        for (int i = 0; i < npcNeeds.Length; i++)
            if (best > npcNeeds[i].amount) {
                lowest = npcNeeds[i].need;
                best = npcNeeds[i].amount;
            }
        return lowest;
    }

    public List<Task> FindTasksInArea() {
        List<Task> closestsTasks = new List<Task>();

        //GD.PrintS(Name, shapecast.IsColliding());

        if (shapecast.IsColliding()) {
            for (int i = 0; i < shapecast.GetCollisionCount(); i++) {
                //GD.PrintS(Name, ((Node3D)shapecast.GetCollider(i)).Name);
                if (shapecast.GetCollider(i) is not Node3D)
                    continue;

                Task task = CheckIfTaskNode((Node3D)shapecast.GetCollider(i));
                if (task != null)
                    closestsTasks.Add(task);
            }
        }
        //GD.PrintS("closest", Name, closestsTasks.Count);

        return closestsTasks;
    }

    private Task CheckIfTaskNode(Node3D node) {
        foreach (Node child in node.GetChildren())
            if (child is Task task)
                return task;
        return null;
    }
}
