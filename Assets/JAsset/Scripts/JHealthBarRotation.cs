using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHealthBarRotation : MonoBehaviour
{
    public Transform cameraTransform;
    float crrHP;
    public Animator animator;

    private void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {



    }
    void LateUpdate()
    {
        // 체력바를 카메라를 향하도록 회전시킵니다.
        if (cameraTransform == null) return;
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                         cameraTransform.rotation * Vector3.up);

    }

    public void TwinkleBar()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attacked");
        }
        
    }
 }
