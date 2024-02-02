using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Intermission 전용 UI, 설정 등을 할 수 있다. (Audience)
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-24
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
            _btnEnter.GetComponentInChildren<TMP_Text>().text = "공연장 들어가기";
        }
        else
        {
            _btnEnter.GetComponentInChildren<TMP_Text>().text = "공연장으로 돌아가기";
        }

    }

    public void OnClickEnterStage()
    {
        stageSceneManager.EnterStage();
    }
}
