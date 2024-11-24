using CubeGuns.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CubeGuns.Pathfinding;
using System;

[RequireComponent(typeof(Guard))]
public class GuardBehaviourTree : MonoBehaviour
{
    [RequireAndAssignComponent, SerializeField] Guard guard;
    [RequireAndAssignComponent, SerializeField] Sight sight;

    [SerializeField] NodeGraph graph;
    [SerializeField] float gunCheckRadius;

    BehaviourTree tree;

    object storedObject;


    private void Awake()
    {
        tree = new BehaviourTree
        (
            #region BT: Find Gun
            // If any of these are true, we continue
            new Selector
            (
                // Do we have a gun?
                new Leaf(new ConditionalLeaf(() => guard.Equipment.HeldObject != null)),
                // If not, Can we get a gun quickly? 
                // If all of these are true, returns true.
                new Sequence
                (
                    // Are there any guns nearby?
                    new Leaf
                    (
                        new ConditionalLeaf(() => 
                        {
                            storedObject = Physics.OverlapSphere(transform.position, gunCheckRadius).Select(x => x.GetComponent<ModularGunBase>())
                                .Where(x => x != null /*&& !x.Equipped*/)
                                .OrderByDescending(x => x.Modules.Count) // This should make it prioritize guns with more modules
                                .FirstOrDefault();
                            return storedObject != null;
                        })
                    ),
                    // If so...
                    new Sequence
                    (
                        // Make sure that what we saw is actually a gun!
                        new Leaf
                        (
                            new ConditionalLeaf(() => storedObject is Weapon)
                        ),
                        // Then walk to it...
                        new Leaf
                        (
                            new MoveTo(guard.Movement as AIMovement, graph, () => transform.position, () => (storedObject as MonoBehaviour).transform.position/*, () => (storedObject as Equippable).Equipped*/)
                        ),
                        // And pick it up!
                        new Leaf 
                        (
                            new ConditionalLeaf(() => Vector3.Distance(transform.position, (storedObject as Equippable).transform.position) < 2f)
                        ),
                        new Leaf
                        (
                            new ActionLeaf(() => guard.Equipment.PickUp(storedObject as Equippable))
                        )
                    )
                ),
                // If not, Can we hit the player with a physics object?
                // If all of these are true, returns true.
                new Sequence
                (
                    // Can we see the player?

                    // Can we get a phyisics object?
                    new Leaf
                    (
                        new ConditionalLeaf(() =>
                        {
                            storedObject = Physics.OverlapSphere(transform.position, gunCheckRadius).Select(x => x.GetComponent<EquippableRigidbody>()).Where(x => x != null && !x.Equipped).FirstOrDefault();
                            return storedObject != null;
                        })
                    ),
                    // Pick up that physics object.
                    new Sequence
                    (
                        // Make sure that what we saw is actually an equippable!
                        new Leaf
                        (
                            new ConditionalLeaf(() => storedObject is EquippableRigidbody)
                        ),
                        // Then walk to it...
                        new Leaf
                        (
                            new MoveTo(guard.Movement as AIMovement, graph, () => transform.position, () => (storedObject as MonoBehaviour).transform.position/*, () => (storedObject as Equippable).Equipped*/)
                        ),
                        // And pick it up!
                        new Leaf
                        (
                            new ConditionalLeaf(() => Vector3.Distance(transform.position, (storedObject as Equippable).transform.position) < 2f)
                        ),
                        new Leaf
                        (
                            new ActionLeaf(() => guard.Equipment.PickUp(storedObject as Equippable))
                        )
                    )
                ),
                // Uuuh. Let's stare awkwardly at the player like little freaks then?
                new Sequence
                (
                    new Leaf
                    (
                        new ConditionalLeaf( () => sight.CanSeeThing(Player.Instance.transform) )
                    ),
                    new Leaf
                    (
                        new ActionLeaf( () => transform.LookAt(Player.Instance.transform, Vector3.up) )
                    ),
                    new Leaf
                    (
                        new ConditionalLeaf( () => false )
                    )
                )
            ),
            #endregion
            // Player and Patrol
            new Selector
            (
                // Player check and chase
                new Sequence
                (
                    // Can see player?
                    new Leaf
                    (
                        new ConditionalLeaf( () => sight.CanSeeThing(Player.Instance.transform))
                    ),
                    // Get a safe distance away
                    // Shoot!
                    new Leaf
                    (
                        new AimAndFire(guard, () => Player.Instance.transform, 0.5f)
                    )

                )
                // Patrol

            )
        );
    }

    private void Update() 
    {
        if(tree.Process() == NodeStatus.SUCCEEDED)
        {
            tree.Reset();
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, gunCheckRadius);
    }
}

public class AimAndFire : ILeaf
{
    Func<Transform> target;
    float aimTime;
    Guard source;

    float startTime;

    public AimAndFire(Guard source, Func<Transform> target, float aimTime)
    {
        this.target = target;
        this.aimTime = aimTime;
        this.source = source;
    }

    public NodeStatus Process()
    {
        if(startTime == 0) startTime = Time.time;

        var targetTransform = target();
        var dirToTarget = (targetTransform.position - source.transform.position).CollapseAxis(VectorAxis.Y).normalized;

        if (Time.time - startTime < aimTime)
        {
            source.transform.forward = dirToTarget;
            return NodeStatus.RUNNING;
        }

        if(source.Equipment.HeldObject == null) return NodeStatus.FAILED;

        source.Equipment.UseObject(dirToTarget * 2.5f);
        return NodeStatus.SUCCEEDED;
    }

    public void Reset()
    {
        startTime = 0;
    }
}
