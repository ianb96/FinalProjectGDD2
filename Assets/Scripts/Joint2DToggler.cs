using UnityEngine;

/// fixes joints when disabling and re-enabling the object
public class Joint2DToggler : MonoBehaviour
{
    [SerializeField] private Joint2D joint;
    private Rigidbody2D connectedBody;

    private void Awake()
    {
        joint = joint ? joint : GetComponent<Joint2D>();
        if (joint) connectedBody = joint.connectedBody;
        else Debug.LogError("No joint found.", this);
    }

    private void OnEnable()
    {
        transform.position = connectedBody.position;
        joint.connectedBody = connectedBody;
    }

    private void OnDisable()
    {
        joint.connectedBody = null;
        connectedBody.WakeUp();
    }
}