using CubeGuns.BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CubeGuns.Pathfinding;

[RequireComponent(typeof(Guard))]
public class GuardBehaviourTree : MonoBehaviour
{
    [RequireAndAssignComponent, SerializeField] Guard guard;

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
                                .Where(x => x != null && !x.Equipped)
                                .OrderBy(x => x.Modules.Count) // This should make it prioritize guns with more modules
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
                            new PathfindingLeaf(this, guard.Movement, graph, () => transform.position, () => (storedObject as MonoBehaviour).transform.position)
                        ),
                        // And pick it up!
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
                            new PathfindingLeaf(this, guard.Movement, graph, () => transform.position, () => (storedObject as MonoBehaviour).transform.position)
                        ),
                        // And pick it up!
                        new Leaf
                        (
                            new ActionLeaf(() => guard.Equipment.PickUp(storedObject as Equippable))
                        )
                    )
                )
                // If not, lets search the entire map for a gun and lets go get it
            ),
            #endregion
            // Patrol
            new Selector
            (
                // Can we see the player?
                new Leaf
                (
                    new ConditionalLeaf(() => false)
                ),
                // If not, lets patrol
                new Sequence
                (
                    // Get next patrol index
                    // Move towards next patrol index
                )
            ),
            // On player seen
            new Sequence
            (
                // If we got this far, it means that we have a gun and that we're currently seeing the player!
                // Make some distance between you and the player
                // SHOOT!!!!
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
