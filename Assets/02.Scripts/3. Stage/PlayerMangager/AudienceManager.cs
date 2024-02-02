using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AudienceManager : PlayerManager
{
    [Header("Controller")]
    [SerializeField] private XRBaseController audienceLeftController;
    [SerializeField] private XRBaseController audienceRightController;


    //�ڸ� �����ߴ��� ���ߴ����� ���� ���°�
    public bool _isPlayerSelectSeat = false;
    public int _seatIndex = 0;

    [Header("Temp")]
    [SerializeField] protected Transform spawnPosition;

    //�� ��Ʈ�ѷ��� ������ �� ������ �̸��� ���� �÷��̾� ������ ������ �����ϰ�. 
    //���Ŀ� Define�� ��� �־ �ҷ����� �� ����.
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
