using Fusion;
using Fusion.Sockets;
using Fusion.XR.Host.Rig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NetworkEmojiManager : NetworkBehaviour
{
    NetworkRunner runner;
    EmojiManager localEmojiManager;
    [SerializeField] GameObject[] emoticonList_LeftHand;
    [SerializeField] GameObject[] emoticonList_RightHand;

    //////////////////////////////////////////////////
    // Particle이 생성될 위치
    [SerializeField] private Transform particlePosition_Left;
    [SerializeField] private Transform particlePosition_Right;

    private void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        localEmojiManager = FindObjectOfType<EmojiManager>();

        localEmojiManager.networkEmojiManager = this;
    }

    // RpcSources : 전송할 수 있는 피어
    // RpcTargets : 피어가 실행되는 피어
    // 1. All : 모두에게 전송 / 세션 내의 모든 피어에 의해서 실행됨(서버 포함)
    // 2. Proxies : 나 말고 전송 / 객체에 대하여 입력 권한 또는 상태 권한을 갖고 있지 않는 피어에 의해 실행됨
    // 3. InputAuthority : 입력 권한 있는 피어만 전송 / 객체에 대한 입력 권한이 있는 피어에 의해 실행됨
    // 4. StateAuthority : 상태 권한 있는 피어만 전송 / 객체에 대한 상태 권한이 있는 피어에 의해 실행됨
    // RpcInfo
    // - Tick : 어떤 곳에서 틱이 전송되었는지
    // - Source : 어떤 플레이어(PlayerRef)가 보냈는지
    // - Channel : Unrealiable 또는 Reliable RPC로 보냈는지 여부
    // - IsInvokeLocal : 이 RPC를 원래 호출한 로컬 플레이어인지의 여부
    // * 공식 문서엔 HostMode를 설정하지 않았지만 이걸 쓰지 않으면 계속 원격 플레이어가 된다. (기본이 서버 모드여서 그런 듯)
    [Rpc]
    public void RPC_UseEmoticon(int index, bool isRightHand, RpcInfo info = default)
    {
        if (isRightHand)
        {
            var RController = emoticonList_RightHand[index].GetComponentInChildren<ParticleController>();
            RController.transform.position = particlePosition_Right.position;
            RController.EmitParticle();
        }
        else
        {
            var LController = emoticonList_LeftHand[index].GetComponentInChildren<ParticleController>();
            LController.transform.position = particlePosition_Right.position;
            LController.EmitParticle();
        }
    }
}
