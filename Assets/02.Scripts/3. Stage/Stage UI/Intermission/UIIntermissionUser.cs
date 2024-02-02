using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Intermission ���� UI, ���� ���� �� �� �ִ�. (Audience)
// ���� �ۼ��� : ���ȫ
// ������ : -
// ���� ������ : 2024-01-24
public class UIIntermissionUser : UIWindow
{
    private StageSceneManager stageSceneManager;

    [SerializeField] Button _btnEnter = null;
    private void Awake()
    {
        stageSceneManager = FindObjectOfType<StageSceneManager>();

        // Set Button
        _btnEnter.onClick.AddListener(OnClickEnterStage);

        // Text Setting
        if (stageSceneManager._StageLevel == StageSceneManager.StageLevel.Enter)
        {
            _btnEnter.GetComponentInChildren<TMP_Text>().text = "������ ����";
        }
        else
        {
            _btnEnter.GetComponentInChildren<TMP_Text>().text = "���������� ���ư���";
        }

    }

    public void OnClickEnterStage()
    {
        stageSceneManager.EnterStage();
    }
}
