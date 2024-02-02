using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//Player 역할 관련 Role enum값으로 설정, 테스트용 역할부여
//최초 작성자: 홍원기
//수정자: 김기홍
//최종 수정일: 2024-01-08
public abstract class PlayerManager : MonoBehaviour
{
    public enum Role
    {
        None,
        Idol,
        Staff,
        Audience
    }

    [SerializeField] private Role playerRole;
    private int playerID;
    private int teamID;

    public void SetRole(Role role) { playerRole = role; }
    public Role GetRole() { return playerRole; }
    public void SetPlayerID(int id) { this.playerID = id; }
    public int GetPlayerID() { return playerID; }
    public void SetTeamID(int id) { this.teamID = id; }
    public int GetTeamID() { return teamID; }

    public abstract void SpawnPlayer();
}
