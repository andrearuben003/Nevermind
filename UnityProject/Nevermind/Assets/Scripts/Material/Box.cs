using UnityEngine;

[RequireComponent (typeof(FixedJoint2D))]
public class Box : MonoBehaviour
{
    FixedJoint2D joint;
    float originalMass;

    private void Awake()
    {
        joint = GetComponent<FixedJoint2D>();
        joint.enabled = false;

        originalMass = GetComponent<Rigidbody2D>().mass;
    }

    public FixedJoint2D GetJoint()
    {
        return joint;
    }

    public void ResetMass()
    {
        GetComponent<Rigidbody2D>().mass = originalMass;
    }

    public void ChangeMass(float mass)
    {
        GetComponent<Rigidbody2D>().mass = mass;
    }
}
