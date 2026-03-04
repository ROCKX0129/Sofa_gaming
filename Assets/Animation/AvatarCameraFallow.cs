using UnityEngine;

public class AvatarCameraFallow : MonoBehaviour
{
    public GameObject Player;
    public Transform headAnchor;
    public float z = -10f;

    void LateUpdate()
    {
        if (!headAnchor) return;

        Vector3 p = headAnchor.position;
        p.z = z;
        transform.position = p;
    }

}
