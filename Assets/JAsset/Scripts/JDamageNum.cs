using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDamageNum : MonoBehaviour
{
    // 이동 속도와 투명도 감소 속도
    public float moveSpeed = 5f;
    public float fadeSpeed = 5f;
    bool top = false;

    // 텍스트 오브젝트의 시작 위치와 목표 위치
    private Vector3 startPos;
    public Vector3 targetPos;
    public Vector3 endPos;

    // 텍스트의 초기 색상
    private Color startColor;

    // TextMesh 컴포넌트
    private TextMesh textMesh;
    private GameObject parentTransform;

    float numRight;
    float numUp;

    void Start()
    {
        // 시작 위치와 텍스트의 초기 색상을 저장
        startPos = transform.position;
        textMesh = GetComponent<TextMesh>();
        targetPos = transform.position;
        numRight = Random.Range(-1.0f, 1.0f);
        numUp = Random.Range(-1.0f, 1.0f);
        //transform.transform.position += Vector3.up * 2; 
    }

    void Update()
    {
        if (parentTransform != null )
        {
            //targetPos = parentTransform.position + transform.up * 1.5f;
            //startPos = parentTransform.position + transform.up * 0.5f;
            //transform.position = new Vector3(parentTransform.position.x, transform.position.y, parentTransform.position.z);
            if (parentTransform.gameObject.tag == "Boss")
            {
                targetPos = parentTransform.transform.position + parentTransform.transform.up * numUp + parentTransform.transform.right * numRight + parentTransform.transform.up * 6.0f;
                startPos = parentTransform.transform.position + parentTransform.transform.up * numUp + parentTransform.transform.right * numRight + parentTransform.transform.up * 4.5f;
                transform.position = new Vector3(startPos.x, transform.position.y, startPos.z);
            }
            else
            {
                targetPos = parentTransform.transform.position + parentTransform.transform.up * numUp + parentTransform.transform.right * numRight + parentTransform.transform.up * 3.0f;
                startPos = parentTransform.transform.position + parentTransform.transform.up * numUp + parentTransform.transform.right * numRight + parentTransform.transform.up * 1.5f;
                transform.position = new Vector3(startPos.x, transform.position.y, startPos.z);
            }
            
        }
        else
        {
        }

    
        
        // 텍스트 오브젝트가 천천히 목표 위치로 올라간다
        if (Vector3.Distance(transform.position, targetPos) > 0.1f && top == false)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetPos) <= 0.1f && top == false)
        {
            top = true;
           // print("Ddd");
        }
        // 목표 위치에 도달했을 때 투명해지면서 내려온다
        if (top)
        {
            // 투명도를 감소시킨다
            Color newColor = textMesh.color;
            newColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = newColor;
            transform.position = Vector3.Lerp(transform.position, startPos, moveSpeed * Time.deltaTime);
            // 텍스트가 완전히 투명해졌다면 제거한다
            if (newColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
     
    public void SetParent(GameObject parent)
    {
        parentTransform = parent;
    }
}