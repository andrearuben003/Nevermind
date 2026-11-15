using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof(FixedJoint2D))]
public class Box : MonoBehaviour
{
    FixedJoint2D joint;
    Rigidbody2D rb;

    float originalGravityScale;

    private void Awake()
    {
        joint = GetComponent<FixedJoint2D>();
        joint.enabled = false;

        rb = GetComponent<Rigidbody2D>();

        originalGravityScale = rb.gravityScale;
    }

    public FixedJoint2D GetJoint()
    {
        return joint;
    }

    public void FreezeBox()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    }

    public void FreezeBoxGroup()
    {
        Transform group = transform.parent;

        if (group)
        {
            foreach (Transform t in group)
            {
                t.GetComponent<Box>().FreezeBox();
            }
        } 
        else
        {
            FreezeBox();
        }
    }

    public void FreeBoxPhysics()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void FreeBoxGroupPhysics()
    {
        Transform group = transform.parent;

        if (group)
        {
            foreach (Transform t in group)
            {
                t.GetComponent<Box>().FreeBoxPhysics();
            }
        } 
        else
        {
            FreeBoxPhysics();
        }
    }

    public void ChangeGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    public void ChangeBoxGroupGravityScale(float scale)
    {
        Transform group = transform.parent;

        if (group) {

            group.GetComponent<Rigidbody2D>().gravityScale = scale;

            foreach (Transform t in group)
            {
                t.GetComponent<Box>().ChangeGravityScale(scale);
            }
        } 
        else
        {
            ChangeGravityScale(scale);
        }
    }

    public void ResetGravityScale()
    {
        rb.gravityScale = originalGravityScale;
    }

    public void ResetBoxGroupGravityScale()
    {
        Transform group = transform.parent;

        if (group)
        {

            group.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
            foreach (Transform t in group)
            {
                t.GetComponent<Box>().ResetGravityScale();
            }
        }
        else 
        {
            ResetGravityScale();
        }
    }

    public void Throw(Vector2 direction, float force)
    {
        Transform group = transform.parent;

        if (group)
        {
            group.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
