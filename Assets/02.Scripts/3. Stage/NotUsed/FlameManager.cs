using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무대 파티클 관리용
// 최초 작성자 : 김효중
// 수정자 : 
// 최종 수정일 : 2023-12-08
public class FlameManager : MonoBehaviour
{
    public Transform FireCenter;
    public Transform FireLeft;
    public Transform FireRight;
    public Transform smokeCenter;
    public Transform smokeLeft;
    public Transform smokeRight;
    public static FlameManager _instance = null;

    public void ActivateAllFire(bool activate)
    {
        ActiveCenterFire(activate);
        ActiveLeftFire(activate);
        ActiveRightFire(activate);
    }
    public void ActivateAllSmoke(bool activate)
    {
        ActiveCenterSmoke(activate);
        ActiveLeftSmoke(activate);
        ActiveRightSmoke(activate);
    }
    public void ActiveCenterFire(bool activate) {ActivateAllParticles(FireCenter,activate); }
    public void ActiveLeftFire(bool activate)   {ActivateAllParticles(FireLeft,activate); }
    public void ActiveRightFire(bool activate)  {ActivateAllParticles(FireRight,activate); }
    public void ActiveCenterSmoke(bool activate) {ActivateAllParticles(smokeCenter,activate); }
    public void ActiveLeftSmoke(bool activate)   {ActivateAllParticles(smokeLeft,activate); }
    public void ActiveRightSmoke(bool activate)  {ActivateAllParticles(smokeRight,activate); }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        
    }
    void ActivateAllParticles(Transform parentObcet,bool activate)
    {
        // 모든 자식 객체를 확인하며 파티클을 활성화
        foreach (Transform child in parentObcet)
        {
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                if(activate) particleSystem.Play(); // 파티클을 재생
                else particleSystem.Stop(); // 파티클 정지
            }
        }
    }
}
