using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JBullet : MonoBehaviour
{
    GameObject PY;
    Vector3 firPos;
    public int bulletSpeed = 20;
    public int attackDistance = 3;
    Vector3 Pposition;
    public GameObject raiserFactory;
    GameObject Raiser;
    // Start is called before the first frame update
    void Start()
    {

        firPos = transform.position;

        if (raiserFactory != null)
        {
            Raiser = Instantiate(raiserFactory);
            SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_RAISER);
        }
        else
        {
            SoundManager.Get().PlayEftSound(SoundManager.ESoundType.EFT_E3ATTACK);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (raiserFactory != null)
        {
            Raiser.transform.position = transform.position;
            Raiser.transform.forward = -transform.forward;
        }

        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        if (Vector3.Distance(transform.position, firPos) > attackDistance)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //print("뿅");
            other.GetComponent<JHPSystem>().UpdateHP(-800);
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Destroy(Raiser);
    }


}

//IEnumerator WarningText(Text text, Color color)
//{
//    BigStateText.enabled = true;
//    float EndTime = 5f;
//    float currTime = 0;
//   while (true)
//   {
//        // countDonw.gameObject 크기를 1.5로 하자.
//        BigStateText.transform.localScale = Vector3.zero;
//        // iTween 을 이용해서 움직임을 주자
//        Hashtable hash = iTween.Hash(
//            "scale", Vector3.one,
//            "time", 0.5f,
//            "easetype", iTween.EaseType.easeOutBounce,
//            "oncompletetarget", gameObject);
//        iTween.ScaleTo(BigStateText.gameObject, hash);

//        if (currTime > EndTime)
//        {
//            break;
//        }
//        currTime += Time.deltaTime;

//        yield return null;
//   }
//    currTime = 0;
//    while (true)
//    {
//        //BigStateText.transform.localScale = Vector3.one;
//        // iTween 을 이용해서 움직임을 주자
//        BigStateText.transform.localScale = Vector3.one;
//        Hashtable hash = iTween.Hash(
//            "scale", Vector3.one,
//            "time", 0.5f,
//            "easetype", iTween.EaseType.easeOutBounce,
//            "oncompletetarget", gameObject);
//        iTween.ScaleTo(BigStateText.gameObject, hash);
//        if (currTime > EndTime)
//        {
//            break;
//        }
//        currTime += Time.deltaTime;

//        yield return null;
//    }
//    BigStateText.enabled = false;
//}               //StartCoroutine(WarningText(AngryState, AngryState.color));