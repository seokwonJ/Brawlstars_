using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player; // 플레이어의 Transform
    public Vector3 offset; // 카메라와 플레이어 사이의 오프셋
    public Vector3 minPosition; // 카메라의 최소 위치 (x, y, z)
    public Vector3 maxPosition; // 카메라의 최대 위치 (x, y, z)
    public float followSpeed = 2f; // 카메라가 플레이어를 따라가는 속도
    public Vector3 margin; // 플레이어가 카메라를 움직이기 시작하는 경계 (x, y, z)


    private void Start()
    {
        Vector3 startPosition = player.position + offset;
        startPosition.x = Mathf.Clamp(startPosition.x, minPosition.x, maxPosition.x);
        startPosition.y = Mathf.Clamp(startPosition.y, minPosition.y, maxPosition.y);
        startPosition.z = Mathf.Clamp(startPosition.z, minPosition.z, maxPosition.z); // z축 고정
        transform.position = startPosition;
    }
    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = transform.position;

            // 플레이어가 화면 중앙을 벗어나면 카메라 이동
            if (Mathf.Abs(transform.position.x - player.position.x) > margin.x)
            {
                targetPosition.x = Mathf.Lerp(transform.position.x, player.position.x + offset.x, followSpeed * Time.deltaTime);
            }
            if (Mathf.Abs(transform.position.y - player.position.y) > margin.y)
            {
                targetPosition.y = Mathf.Lerp(transform.position.y, player.position.y + offset.y, followSpeed * Time.deltaTime);
            }
            if (Mathf.Abs(transform.position.z - player.position.z) > margin.z)
            {
                targetPosition.z = Mathf.Lerp(transform.position.z, player.position.z + offset.z, followSpeed * Time.deltaTime);
            }

            // 카메라 위치 제한
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minPosition.z, maxPosition.z);

            transform.position = targetPosition;
        }
    }
}
