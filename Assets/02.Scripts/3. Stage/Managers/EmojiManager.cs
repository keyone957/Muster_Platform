using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public enum EEmojiID
{
    Emoticon_None = -1,
    Emoticon_Smile = 0,
    Emoticon_Sad = 1,
    Emoticon_Angry = 2,
    Emoticon_Surprised = 3,
}

// 이모티콘 매니저, 이모티콘의 등록과 사용을 담당함
// 최초 작성자 : 김기홍
// 수정자 : 
// 최종 수정일 : 2024-01-17
public class EmojiManager : MonoBehaviour
{
    public static EmojiManager instance { get; private set; }

    public NetworkEmojiManager networkEmojiManager;

    // 사용할 이모티콘을 저장, SetEmoji()을 통해 등록
    [HideInInspector] public EEmojiID[] emojis;
    // 이모티콘 Prefab 저장
    private Dictionary<int, GameObject> emojiObjs;
    private Dictionary<int, Sprite> emojiSprites;
    // 이모티콘 사용 딜레이
    [SerializeField] float useDelay = 0.1f;
    DateTime lastUsedTime;
    public int curSelectEmojiIndex = -1;

    //////////////////////////////////////////////////
    // VR UI
    [SerializeField] private GameObject emojiPanel;
    [SerializeField] private GameObject[] emojiBtn;
    private bool isSetCanvasPosCalled = false;
    [SerializeField] private GameObject RController;
    [SerializeField] private XRRayInteractor RightControllerRay;


    //////////////////////////////////////////////////
    // Unity Functions
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        if(SettingManager._instance.role == PlayerManager.Role.Idol)
        {
            emojiPanel.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        emojis = new EEmojiID[4];
        emojiObjs = new Dictionary<int, GameObject>();
        emojiSprites = new Dictionary<int, Sprite>();

        // Set Emoji
        RegistEmoji(0, EEmojiID.Emoticon_Smile);
    }
    private void Update()
    {
        SetActivePanel();

        if (InputManager.instance.GetDeviceBtn(InputManager.instance._leftController, CommonUsages.triggerButton))
        {
            if ((DateTime.Now - lastUsedTime).TotalSeconds < useDelay) return;
            if (curSelectEmojiIndex < 0) return;
            if ((int)emojis[curSelectEmojiIndex] < 0) return;
            lastUsedTime = DateTime.Now;

            if (networkEmojiManager != null)
                networkEmojiManager.RPC_UseEmoticon((int)emojis[curSelectEmojiIndex], false);
        }
        if (InputManager.instance.GetDeviceBtn(InputManager.instance._rightController, CommonUsages.triggerButton))
        {
            if ((DateTime.Now - lastUsedTime).TotalSeconds < useDelay) return;
            if (curSelectEmojiIndex < 0) return;
            if ((int)emojis[curSelectEmojiIndex] < 0) return;
            lastUsedTime = DateTime.Now;

            if (networkEmojiManager != null)
                networkEmojiManager.RPC_UseEmoticon((int)emojis[curSelectEmojiIndex], true);
        }
    }

    //////////////////////////////////////////////////
    // VR UI
    //패널 활성화 비활성화
    private void SetActivePanel()
    {
        bool isPanelActive = InputManager.instance.GetDeviceBtn(InputManager.instance._rightController, CommonUsages.primaryButton);
        //실제 사용할 코드
        emojiPanel.SetActive(isPanelActive);

        if (isPanelActive && !isSetCanvasPosCalled)
        {
            SetCanvasPos();
            isSetCanvasPosCalled = true;
        }
        else if (!isPanelActive)
        {
            isSetCanvasPosCalled = false;
        }
    }
    //현재 보고 있는 카메라 기준으로 앞에 이모지 ui띄움
    private void SetCanvasPos()
    {
        RaycastHit hit;
        bool hasHit = RightControllerRay.TryGetCurrent3DRaycastHit(out hit);
        if (hasHit || !hasHit)
        {
            Vector3 raycastEndpoint = RightControllerRay.transform.position + RightControllerRay.transform.forward * RightControllerRay.maxRaycastDistance * 0.05f;
            float raycastEndpointX = raycastEndpoint.x;
            float raycastEndpointY = raycastEndpoint.y;
            float raycastEndpointZ = raycastEndpoint.z;


            float distanceAdjustment = 9f; // 원하는 거리 조정값
            raycastEndpoint += RightControllerRay.transform.forward * distanceAdjustment;

            gameObject.transform.position = new Vector3(raycastEndpointX, raycastEndpointY, raycastEndpointZ);
        }

        Quaternion fix = RController.transform.rotation;
        fix.eulerAngles = new Vector3(fix.eulerAngles.x, fix.eulerAngles.y, 0f);
        gameObject.transform.rotation = fix;
    }

    //////////////////////////////////////////////////
    // Emoji Function
    // 이모티콘 등록
    public void RegistEmoji(int index, EEmojiID emoticonID)
    {
        emojis[index] = emoticonID;
        if (emojis[index] == EEmojiID.Emoticon_None)
            emojiBtn[index].GetComponent<Image>().sprite = GetEmoticonSprite(emoticonID);
    }

    // 이모티콘 프리팹 불러오기 (없는 경우 프리팹 생성)
    GameObject GetEmoticonPrefabByID(EEmojiID emoticonID)
    {
        GameObject ret;
        if (emojiObjs.TryGetValue((int)emoticonID, out ret)) return ret;

        string path = "Emoticons/" + emoticonID.ToString();
        ret = Instantiate(Resources.Load<GameObject>(path), this.transform);
        emojiObjs[(int)emoticonID] = ret;

        return ret;
    }
    Sprite GetEmoticonSprite(EEmojiID emoticonID)
    {
        Sprite ret;
        if (emojiSprites.TryGetValue((int)emoticonID, out ret)) return ret;
        string path = "Emoticons/I_" + emoticonID.ToString();
        var texture = Instantiate(Resources.Load<Texture2D>(path), this.transform);
        if (texture != null)
        {
            ret = ConvertTexture2DToSprite(texture);
            emojiSprites[(int)emoticonID] = ret;
        }
        else
        {
            ret = null;
        }
        return ret;
    }
    Sprite ConvertTexture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
