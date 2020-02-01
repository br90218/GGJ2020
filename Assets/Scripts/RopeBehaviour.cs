using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpringJoint2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class RopeBehaviour : MonoBehaviour
{
    public Transform OriginTransform, TargetTransform;

    private LineRenderer _line;
    private SpringJoint2D _hingeJoint;
    private FixedJoint2D _fixedJoint;

    public void Start()
    {
        OriginTransform = transform;
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, OriginTransform.position);
        _line.SetPosition(1, TargetTransform.position);
        _hingeJoint = GetComponent<SpringJoint2D>();
        _hingeJoint.connectedBody = TargetTransform.GetComponent<Rigidbody2D>();

        if (transform.parent)
        {
            _fixedJoint.attachedRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        }


    }


    public void Update()
    {
        _line.SetPosition(0, OriginTransform.position);
        _line.SetPosition(1, TargetTransform.position);

    }
}