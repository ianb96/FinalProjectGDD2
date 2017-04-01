using UnityEngine;

/// fixes joints when disabling and re-enabling the object
public class Joint2DToggler : MonoBehaviour
{
    [SerializeField] private Joint2D joint;
    private Rigidbody2D connectedBody;
    private Rigidbody2D rb;

    private void Awake()
    {
        joint = joint ? joint : GetComponent<Joint2D>();
        if (joint) connectedBody = joint.connectedBody;
        else Debug.LogError("No joint found.", this);
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        joint.connectedBody = connectedBody;
        transform.position = connectedBody.position;
        transform.rotation = Quaternion.identity;
        rb.WakeUp();
    }

    private void OnDisable()
    {
        joint.connectedBody = null;
        rb.Sleep();
    }
}