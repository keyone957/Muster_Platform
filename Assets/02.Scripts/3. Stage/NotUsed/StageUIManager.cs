using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 공연 UI 관리용
// 최초 작성자 : 김효중
// 수정자 : 
// 최종 수정일 : 2023-11-07
public class StageUIManager : UIManager
{
    [SerializeField]
    private UIStageScreen _mainScreen =null;
    [SerializeField]
    private UIStageScreen _leftScreen= null;
    [SerializeField]
    private UIStageScreen _rightScreen = null;
    public static StageUIManager instance = null;

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
    }
    public void ActiveMainScreen(bool activate) =>  _mainScreen.GetComponentInChildren<Canvas>().enabled = activate;
    public void ActiveLeftScreen(bool activate) => _leftScreen.GetComponentInChildren<Canvas>().enabled = activate;
    public void ActiveRightScreen(bool activate) => _rightScreen.GetComponentInChildren<Canvas>().enabled = activate;

    public void SetMainScreenText<T>(T midText) => _mainScreen.SetMidText(midText.ToString());
    public void SetMainScreenText<T>(T midText,T subText)
    {
         _mainScreen.SetMidText(midText.ToString());
         _mainScreen.SetSubText(subText.ToString());
    }
    public void SetLeftScreenText<T>(T midText) => _leftScreen.SetMidText(midText.ToString());
    public void SetLeftScreenText<T>(T midText,T subText)
    {
         _leftScreen.SetMidText(midText.ToString());
         _leftScreen.SetSubText(subText.ToString());
    }
    public void SetRightScreenText<T>(T midText) => _rightScreen.SetMidText(midText.ToString());
    public void SetRightScreenText<T>(T midText,T subText)
    {
         _rightScreen.SetMidText(midText.ToString());
         _rightScreen.SetSubText(subText.ToString());
    }
    public void SetSlider(float now, float max) => _mainScreen.SetSlider(now, max);
    public void ActivateSlider(bool activate) => _mainScreen.ActivateSlider(activate);
}