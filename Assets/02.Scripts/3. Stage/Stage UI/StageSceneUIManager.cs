using UnityEngine;

// Stage 씬의 UI 관리
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-16

public class StageSceneUIManager : UIManager
{
    // UI
    public FadeOverlay _FadeOverlay { get; private set; }
    [SerializeField] private Transform _mainPanel = null;

    // 
    private UIStageUserEnter _stageUserEnterMenu;
    private UIStageIdolEnter _stageIdolEnterMenu;
    private UIIntermissionIdol _intermissionIdol;
    private UIIntermissionUser _intermissionUser;
    private UIIntermissionNotice _intermissionNotice;
    public UIWindow _curOpenWindow;

    private void Awake()
    {
        _FadeOverlay = FindObjectOfType<FadeOverlay>();
    }
    void Start()
    {
        switch (SettingManager._instance.role)
        {
            case PlayerManager.Role.Idol:
                // FlowChart 수정에 따른 코드 수정
                // - Enter 제거, Intermission으로 바로 향하도록 코드 수정
                /*
                if (_stageIdolMenu == null)
                {
                    _stageIdolMenu = OpenWindow(Define._uiIdolEnter, _mainPanel) as UIStageIdolEnter;
                    _curOpenWindow = _stageIdolMenu;
                }
                */
                _intermissionIdol = OpenWindow(Define._uiIntermissionIdol, _mainPanel) as UIIntermissionIdol;
                _curOpenWindow = _intermissionIdol;
                break;

            case PlayerManager.Role.Audience:
                if (_stageUserEnterMenu == null)
                {
                    _stageUserEnterMenu = OpenWindow(Define._uiUserEnter, _mainPanel) as UIStageUserEnter;
                    _curOpenWindow = _stageUserEnterMenu;
                }
                break;
        }
    }
    public void OpenIntermissionUI()
    {
        CloseCurWindow();
        switch (SettingManager._instance.role)
        {
            case PlayerManager.Role.Idol:
                if (_intermissionIdol == null)
                {
                    _intermissionIdol = OpenWindow(Define._uiIntermissionIdol, _mainPanel) as UIIntermissionIdol;
                    _curOpenWindow = _intermissionIdol;
                }
                break;

            case PlayerManager.Role.Audience:
                if (_intermissionUser == null)
                {
                    _intermissionUser = OpenWindow(Define._uiIntermissionUser, _mainPanel) as UIIntermissionUser;
                    _curOpenWindow = _intermissionUser;
                }
                break;
        }
    }
    // 입장 조건 만족 시 true (HMD기기를 착용하고 있다면 true)
    public bool OpenNotice()
    {
        CloseCurWindow();

        _intermissionNotice = OpenWindow(Define._uiIntermissionNotice, _mainPanel) as UIIntermissionNotice;
        _curOpenWindow = _intermissionNotice;
        return InputManager.instance.IsHMDTracking();
    }
    public void CloseCurWindow()
    {
        if (_curOpenWindow != null)
        {
            CloseWindow(_curOpenWindow);
        }
    }
}
