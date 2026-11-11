using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform defaultTarget;
    private Transform currentTarget;
    public Vector3 offset = new Vector3(0f, 20f, 0f);
    public float smoothSpeed = 5f;
    void Start()
    {
        currentTarget = defaultTarget;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void LateUpdate()
    {
        if (currentTarget == null) return;

        Vector3 desiredPosition = currentTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    public void ChangeTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void ReturnToPlayer(float delay)
    {
        StartCoroutine(ReturnRoutine(delay));
    }

    private IEnumerator ReturnRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentTarget = defaultTarget;
    }
}
