using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStickList : UIWindow
{
    private StartSceneManager startSceneManager;

    [SerializeField] private RectTransform _contents = null;
    [SerializeField] private GameObject _itemPrefab = null;
    [SerializeField] private Button _btnClose = null;
    [SerializeField] private Button _btnOK = null;
    private void Awake()
    {
        startSceneManager = FindObjectOfType<StartSceneManager>();

        InitializeItems();
        _btnClose.onClick.AddListener(OnClickBack);
        _btnOK.onClick.AddListener(OnClickOK);
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBack();
        }

        return true;
    }
    private void InitializeItems()
    {
        TextAsset loadedText = Resources.Load<TextAsset>(Define._stickListPath);
        StickList stickList = JsonUtility.FromJson<StickList>(loadedText.text);

        for (int i = 0; i < stickList._items.Count; ++i)
        {
            AddItem(stickList._items[i]);
        }
    }

    private void OnClickBack()
    {
        startSceneManager._UIManager.CloseStickList();
        //TitleSystem._Instance._UIManager.OpenAccSetting();
    }
    private void AddItem(StickList.Item item)
    {
        UISticItem itemUI = UIManager.CreateWidget<UISticItem>(_itemPrefab, _contents);
        itemUI.Set(item
            , delegate { OnItemSelected(item._ID); }
            );
    }
    private void OnClickOK()
    {
        Debug.Log("OK");
        // TODO: 선택 적용
    }
    private void OnItemSelected(int id)
    {
        Debug.Log(GetStickID(id));
        // TODO: 선택 적용
    }
    private string GetStickID(int id)
    {
        return string.Format("Stick {0:D3}", id);
    }
}
