using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 점수 관리
// Photon PUN 2 네트워크 뷰를 사용하여 동기화된 스코어를 관리
// 최초 작성자 : 김기홍
// 수정자 : 
// 최종 수정일 : 2023-10-29
public class ScoreManager : MonoBehaviour
{
    // Singleton
    public static ScoreManager instance = null;

    // 플레이어의 점수를 저장
    // < PlayerID, Personal Score>
    private Dictionary<int, int> scores;

    // 총 점수 저장
    int[] teamScore;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion

        teamScore = new int[2] { 0, 0 };
        scores = new Dictionary<int, int>(); // 하나만 존재해야함
                                             // playerid, score

    }

    // 입력받은 Speed를 점수로 변환
    public int ConvertSpeedToScore(float speed)
    {
        return (int)speed * 100;
    }

    // 입력받은 점수를 속한 팀의 총 점수에 더하기
    public int AddScore(int playerID, int teamID, int score)
    {
        if (scores.ContainsKey(playerID) == false)
        {
            scores.Add(playerID, score);
        }
        else
        {
            scores[playerID] += score;
        }
        teamScore[teamID] += score;
        return teamScore[teamID];
    }

    public int GetTeamScore(int teamID)
    {
        return teamScore[teamID];
    }
    public int GetPersonalScore(int playerID)
    {
        return scores[playerID];
    }
}
