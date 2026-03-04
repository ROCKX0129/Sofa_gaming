using UnityEngine;
using UnityEngine.SocialPlatforms;

public class KeepScale : MonoBehaviour
{

    private Transform parentTransform;
    private Vector3 offset;

    void Start()
    {
        parentTransform = transform.parent;
        offset = transform.localPosition;
        transform.SetParent(null);
    }

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            transform.position = parentTransform.TransformPoint(offset);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
