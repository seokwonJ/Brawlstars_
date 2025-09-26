using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    float mx = 0;
    float my = 0;
    bool audioBool=false;
    bool audioBool2 = false;
    bool CZoomOut = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioBool && GameManager.instance.isPlaying)
        {
            audioBool = true;
            SoundManager.Get().PlayBgmSound(SoundManager.EBgmType.BGM_INGAME);
        }
        if (!audioBool2 && GameManager.instance.round == 3)
        {
            audioBool2 = true;
            SoundManager.Get().PlayBgmSound(SoundManager.EBgmType.BGM_WARNING);
        }
        if (!CZoomOut && GameManager.instance.isPlaying == false && GameManager.instance.Boss == null)
        {
            CZoomOut = true;
            StartCoroutine(ZoomOut());
        }
        
        //transform.eulerAngles = new Vector3(-my, mx, 0);
    }

    IEnumerator ZoomOut()
    {
        Camera NowCamera = GetComponent<Camera>();
        float ZoomOutTime = 0f;
        while(true)
        {
            NowCamera.fieldOfView += 0.02f;
            if (ZoomOutTime >1f)
            {
                break;
            }
            ZoomOutTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
