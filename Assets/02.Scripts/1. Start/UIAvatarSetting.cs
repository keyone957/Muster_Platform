using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIAvatarSetting : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] Button _btnClose = null;
    [SerializeField] Button _btnAdd = null;
    [SerializeField] Button _btnInit = null;
    [SerializeField] Button _btnOK = null;
    private Stream openStream = null;
    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();
        _btnClose.onClick.AddListener(OnClickBack);
        _btnAdd.onClick.AddListener(OnClikcAdd);
        _btnInit.onClick.AddListener(OnClickBack);
        _btnOK.onClick.AddListener(OnClickBack); 
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }

        return true;
    }
    private void OnClickBack()
    {
        startSceneManager._UIManager.Pop();
    }
    public void OnClikcAdd()
    {
        startSceneManager._UIManager.OpenFileList();
    }
    public void OnClickInit()
    {
        // SettingManager._instance._idolPrefab = null;
        // SettingManager._instance._audiencePrefabName = "";
    }
    public void OnClickOK()
    {
        OnClickBack();
    }
}