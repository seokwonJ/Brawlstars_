using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JGameOpeningManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Get().PlayBgmSound(SoundManager.EBgmType.BGM_TITLE);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnClickmain()
    {
        // JSampleScene �ε�����
        SceneManager.LoadScene("PlayScene");
    }
}
