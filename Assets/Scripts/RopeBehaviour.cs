using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpringJoint2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class RopeBehaviour : MonoBehaviour
{
    public Transform OriginTransform, TargetTransform;
    public Transform hand;
    public RopeRenderer _line;
    private SpringJoint2D _springJoint;
    private FixedJoint2D _fixedJoint;


    public void Awake()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    public void Init(Transform target)
    {
        TargetTransform = target;
        enabled = true;
    }
    
    public void Start()
    {
        if (!TargetTransform)
        {
            Debug.LogError("No Target Transform for rope!");
            return;
        }


        OriginTransform = transform;
        //_line = GetComponent<LineRenderer>();
        //_line.SetPosition(0, OriginTransform.position);
        //_line.SetPosition(1, TargetTransform.position);
        _springJoint = GetComponent<SpringJoint2D>();
        _springJoint.enabled = true;
        _springJoint.connectedBody = TargetTransform.GetComponent<Rigidbody2D>();

        _fixedJoint = GetComponent<FixedJoint2D>();
        _fixedJoint.enabled = true;
        if (transform.parent)
        {
            _fixedJoint.connectedBody = transform.parent.GetComponent<Rigidbody2D>();
        }

    }


    public void Update()
    {
        //UpdateLineGraphics();
        if (Input.GetKey(KeyCode.E))
        {
            PullRope();
        }
        _line.MovePoint(0,hand.position);
        _line.MovePoint(1,TargetTransform.position);
        _line.RepaintRope();
    }


//    private void UpdateLineGraphics()
//    {
//        _line.SetPosition(0, OriginTransform.position);
//        _line.SetPosition(1, TargetTransform.position);
//    }

    public void PullRope()
    {
       _springJoint.distance -= Time.deltaTime * 1f;
    }
}