using UnityEngine;

public class LampSwing : MonoBehaviour
{
    public Transform anchorPoint;  // The point around which the lamp will swing
    public float swingForce = 10f; // The force applied to the lamp to make it swing
    public float damping = 1f;     // Damping to reduce the swing over time

    private HingeJoint _hingeJoint;

    void Start()
    {
        _hingeJoint = gameObject.AddComponent<HingeJoint>();
        _hingeJoint.connectedBody = anchorPoint.GetComponent<Rigidbody>();
        // hingeJoint.axis = Vector3.forward;
        _hingeJoint.useSpring = true;
        JointSpring jointSpring = _hingeJoint.spring;
        jointSpring.spring = swingForce;
        jointSpring.damper = damping;
        _hingeJoint.spring = jointSpring;
    }

    void Update()
    {
        // You can add input or other logic to control the lamp's swinging here
    }
}