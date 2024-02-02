using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공연 VFX 관리용
// 최초 작성자 : 김기홍
// 수정자 : 
// 최종 수정일 : 2023-11-01
public class StageVFXManager : MonoBehaviour
{
    // Singleton
    public static StageVFXManager instance = null;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
    }
    public void ActiveFire(bool active)
    {
        Debug.Log(active ? "Fire On" : "Fire Off");
    }
}
