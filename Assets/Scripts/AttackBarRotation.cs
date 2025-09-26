using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBarRotation : MonoBehaviour
{
    public Transform cameraTransform;

    private void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;
    }
    void LateUpdate()
    {
        // 체력바를 카메라를 향하도록 회전시킵니다.
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                         cameraTransform.rotation * Vector3.up);
    }
}
