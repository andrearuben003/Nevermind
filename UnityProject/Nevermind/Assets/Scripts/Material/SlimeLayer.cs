using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeLayer : MonoBehaviour
{
    Box box;
    FixedJoint2D joint;
    private void Start()
    {
        box = transform.parent.GetComponent<Box>();
        joint = box.GetJoint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            Transform group;
            if (!box.transform.parent)
            {
                if (!collision.transform.parent) //create new group
                {
                    //Create new parent group
                    group = (new GameObject("Box Group")).transform;
                    group.gameObject.AddComponent<Rigidbody2D>();

                    //Connect other box
                    collision.transform.parent = group;
                    collision.GetComponent<Box>().GetJoint().enabled = true;
                    collision.GetComponent<Box>().GetJoint().connectedBody = group.GetComponent<Rigidbody2D>();
                }
                else //exctract the group
                {
                    group = collision.transform.parent;
                }

                //Connect this box to the group
                box.transform.parent = group;
                box.GetJoint().enabled = true;
                box.GetJoint().connectedBody = group.GetComponent<Rigidbody2D>();
            } 
            else 
            {
                //If we are already part of a group merge them
                if (collision.transform.parent && box.transform.parent != collision.transform.parent) 
                {
                    group = box.transform.parent;

                    Transform otherGroup = collision.transform.parent;

                    //Player group always wins
                    //If there is no player my group always wins
                    if(otherGroup.GetComponent<FixedJoint2D>()) 
                        MergeGroup(otherGroup, group); 
                    else 
                        MergeGroup(group, otherGroup);

                }
            }
        } 
        else if (collision.CompareTag("Player"))
        {
            Transform group = box.transform.parent;
            
            if (!group) //Player touch a box without a group
            {
                //Create new parent group
                group = (new GameObject("Box Group")).transform;
                group.gameObject.AddComponent<Rigidbody2D>();

                box.transform.parent = group;
                box.GetJoint().enabled = true;
                box.GetJoint().connectedBody = group.GetComponent<Rigidbody2D>();
            }

            //Connect the group to the player
            group.AddComponent<FixedJoint2D>().connectedBody = collision.attachedRigidbody;

            //If the player is on top of it won't be able jump
            if(transform.right == Vector3.up)
            {
                collision.GetComponent<MovementLimiter>().CanJump = false;
            }

        }

        if(collision.CompareTag("Box") || collision.CompareTag("Player"))
        {
            if (box.transform.parent.GetComponent<FixedJoint2D>())
            {
                ChangeBoxesMass(0.1f);
            }
        }
    }

    private void ChangeBoxesMass(float mass)
    {
        Transform group = box.transform.parent;

        for (int i = 0; i < group.childCount; i++) 
        {
            group.GetChild(i).GetComponent<Box>().ChangeMass(mass); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<MovementLimiter>().CanJump = true;
        }
    }

    /// Merge B into A
    private void MergeGroup(Transform groupA, Transform groupB)
    {

        //Move all elements of the groupB on groupA
        while (groupB.childCount > 0)
        {
            Transform child = groupB.GetChild(0);

            child.parent = groupA;
            child.GetComponent<Box>().GetJoint().connectedBody = groupA.GetComponent<Rigidbody2D>();
        }

        //Destroy empty group
        Destroy(groupB.gameObject);
    }

    protected virtual void OnEnable()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    protected virtual void OnDisable()
    {
        if (gameObject.activeSelf)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            Transform parent = box.transform.parent;

            if (parent)
            {
                joint.connectedBody = null;
                joint.enabled = false;

                box.GetComponent<Box>().ResetMass();
                
                if(box.transform.parent && box.transform.parent.gameObject.activeSelf) //Check this to avoid errors on change of scene
                    box.transform.parent = null;

                if (parent.childCount == 0)
                    Destroy(parent.gameObject);
            }
        }
    }
}
