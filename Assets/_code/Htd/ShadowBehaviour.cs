using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    [SerializeField]
    Vector2 movementLimit;
    [SerializeField]
    [Range(0, 20)] float maxDistance;
    Transform parent;
    Transform mainCamera;

    private void Start()
    {
        parent = transform.parent;
        mainCamera = Camera.main.transform;
    }

    private void Update()
    {
        Vector2 distance = new Vector2();
        distance.x = mainCamera.position.x - parent.position.x;
        distance.y = mainCamera.position.z - parent.position.z;

        distance.x = (Mathf.Abs(distance.x) < maxDistance) ? distance.x / maxDistance : ((distance.x > 0) ? 1 : -1);
        distance.y = (Mathf.Abs(distance.y) < maxDistance) ? distance.y / maxDistance : ((distance.y > 0) ? 1 : -1);

        distance.x *= movementLimit.x;
        distance.y *= movementLimit.y;

        transform.localPosition = new Vector3(distance.x, 0, distance.y);
    }
}
