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

// �̸�Ƽ�� �Ŵ���, �̸�Ƽ���� ��ϰ� ����� �����
// ���� �ۼ��� : ���ȫ
// ������ : 
// ���� ������ : 2024-01-17
public class EmojiManager : MonoBehaviour
{
    public static EmojiManager instance { get; private set; }

    public NetworkEmojiManager networkEmojiManager;

    // ����� �̸�Ƽ���� ����, SetEmoji()�� ���� ���
    [HideInInspector] public EEmojiID[] emojis;
    // �̸�Ƽ�� Prefab ����
    private Dictionary<int, GameObject> emojiObjs;
    private Dictionary<int, Sprite> emojiSprites;
    // �̸�Ƽ�� ��� ������
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
    //�г� Ȱ��ȭ ��Ȱ��ȭ
    private void SetActivePanel()
    {
        bool isPanelActive = InputManager.instance.GetDeviceBtn(InputManager.instance._rightController, CommonUsages.primaryButton);
        //���� ����� �ڵ�
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
    //���� ���� �ִ� ī�޶� �������� �տ� �̸��� ui���
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


            float distanceAdjustment = 9f; // ���ϴ� �Ÿ� ������
            raycastEndpoint += RightControllerRay.transform.forward * distanceAdjustment;

            gameObject.transform.position = new Vector3(raycastEndpointX, raycastEndpointY, raycastEndpointZ);
        }

        Quaternion fix = RController.transform.rotation;
        fix.eulerAngles = new Vector3(fix.eulerAngles.x, fix.eulerAngles.y, 0f);
        gameObject.transform.rotation = fix;
    }

    //////////////////////////////////////////////////
    // Emoji Function
    // �̸�Ƽ�� ���
    public void RegistEmoji(int index, EEmojiID emoticonID)
    {
        emojis[index] = emoticonID;
        if (emojis[index] == EEmojiID.Emoticon_None)
            emojiBtn[index].GetComponent<Image>().sprite = GetEmoticonSprite(emoticonID);
    }

    // �̸�Ƽ�� ������ �ҷ����� (���� ��� ������ ����)
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
