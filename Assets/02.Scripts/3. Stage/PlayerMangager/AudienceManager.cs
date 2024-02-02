using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AudienceManager : PlayerManager
{
    [Header("Controller")]
    [SerializeField] private XRBaseController audienceLeftController;
    [SerializeField] private XRBaseController audienceRightController;


    //자리 선택했는지 안했는지에 대한 상태값
    public bool _isPlayerSelectSeat = false;
    public int _seatIndex = 0;

    [Header("Temp")]
    [SerializeField] protected Transform spawnPosition;

    //각 컨트롤러에 적용해 줄 프리펩 이름에 따라서 플레이어 응원봉 프리펩 변경하게. 
    //추후에 Define에 경로 넣어서 불러오게 할 예정.
    public void SetStickPrefab(string stickName)
    {
        audienceLeftController.modelPrefab = Resources.Load<GameObject>(stickName).transform;
        audienceRightController.modelPrefab = Resources.Load<GameObject>(stickName).transform;
    }

    public override void SpawnPlayer()
    {
        // temp code
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
    }
}
