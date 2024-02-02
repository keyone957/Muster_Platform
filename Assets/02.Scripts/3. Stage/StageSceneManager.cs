using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSceneManager : MonoBehaviour
{
    public StageSceneUIManager _UIManager { get; private set; }

    PlayerManager playerManager;
    // �ѹ��� �ʱ�ȭ
    bool first = true;

    // Stage Level
    public enum StageLevel
    {
        Enter,
        Intermission,
        Stage
    }
    private StageLevel stageLevel = StageLevel.Enter;
    public StageLevel _StageLevel { get; }

    private void Awake()
    {
        _UIManager = FindObjectOfType<StageSceneUIManager>();
    }
    public void EnterIntermission()
    {
        stageLevel = StageLevel.Intermission;
        _UIManager.OpenIntermissionUI();
    }
    public void EnterStage()
    {
        stageLevel = StageLevel.Stage;

        // PC UI ���� (Notice)
        if (_UIManager.OpenNotice() == false) return;

        // �ʱ� ����
        if (first)
        {
            first = false;
            OVRManager.HMDMounted += EnterStage;
            OVRManager.HMDUnmounted += EnterIntermission;
            playerManager = FindObjectOfType<PlayerManager>();
        }

        // VR ����
        playerManager.SpawnPlayer(); 

    }
}
