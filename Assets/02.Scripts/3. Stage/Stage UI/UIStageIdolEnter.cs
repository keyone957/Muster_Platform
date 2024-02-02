using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Stage(Enter, Intermission, Stage)�� Enter�ܰ��� UI ����
// ����� ������� ����
// ���� �ۼ��� : ���ȫ
// ������ : -
// ���� ������ : 2024-01-26
public class UIStageIdolEnter : UIWindow
{
    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnEnter = null;
    [SerializeField] TMP_Text _textRoomCode = null;

    private void Awake()
    {
        _btnClose.onClick.AddListener(OnClickBack);
        _btnEnter.onClick.AddListener(OnClickEnter);
    }
    private async void Start()
    {
        await NetworkManager._instance.CreateSession(SettingManager._instance.roomCode);

        SettingManager._instance.roomCode = StringUtils.GeneratePassword(8);
        _textRoomCode.text = "Room Code : " + SettingManager._instance.roomCode;
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }
        return true;
    }
    private async void OnClickBack()
    {
        await NetworkManager._instance.DisconnectToSession();
        await SceneLoader._instance.LoadScene(SceneName.Start);
    }
    public void OnClickEnter()
    {
        // TODO: Intermission UI ����
    }
}
