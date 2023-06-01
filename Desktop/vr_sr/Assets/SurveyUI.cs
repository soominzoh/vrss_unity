using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyUI : MonoBehaviour
{
    public Toggle[] toggles;
    
    float time = 0;
    bool isTimer = false;

    public GameObject waitUI;
    public GameObject notChoiceUI;
    public Text timerUI;
    float currentLimitTime;
    bool isUpdateUI;

    void Update()
    {
        if (isTimer)
            time += Time.deltaTime;

        if (currentLimitTime <= 0 && isUpdateUI)
        {
            isTimer = false;
            isUpdateUI = false;
        }

        if (isUpdateUI)
        {
            currentLimitTime -= Time.deltaTime;
            timerUI.text = Mathf.RoundToInt(currentLimitTime).ToString();
        }
    }

    public IEnumerator UpdateUI(float limitTime, System.Action<int, float> callback)
    {
        waitUI.SetActive(false);
        notChoiceUI.SetActive(false);

        // 로컬 파라미터 초기화
        this.time = 0;
        isTimer = true;
        isUpdateUI = true;
        currentLimitTime = limitTime;

        // 토클 초기화
        toggles[0].isOn = false;
        toggles[1].isOn = false;
        toggles[2].isOn = false;
        toggles[3].isOn = false;
        toggles[4].isOn = false;

        // 토글 이벤트 초기화
        toggles[0].onValueChanged.RemoveAllListeners();
        toggles[1].onValueChanged.RemoveAllListeners();
        toggles[2].onValueChanged.RemoveAllListeners();
        toggles[3].onValueChanged.RemoveAllListeners();
        toggles[4].onValueChanged.RemoveAllListeners();

        int selectNumber = 0;
        // 버튼 이벤트 등록
        toggles[0].onValueChanged.AddListener((isbool) => {
            if (!isbool)
                return;
            selectNumber = 1;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        toggles[1].onValueChanged.AddListener((isbool) => {
            if (!isbool)
                return;
            selectNumber = 2;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        toggles[2].onValueChanged.AddListener((isbool) => {
            if (!isbool)
                return;
            selectNumber = 3;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        toggles[3].onValueChanged.AddListener((isbool) => {
            if (!isbool)
                return;
            selectNumber = 4;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        toggles[4].onValueChanged.AddListener((isbool) => {
            if (!isbool)
                return;
            selectNumber = 5;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });

        // 활성화 및 페이드 인
        this.gameObject.SetActive(true);
        this.GetComponent<Animation>().Play("FadeIn");
        AudioManager.instance.Play("selectUI");

        // 제한시간까지 대기
        yield return new WaitUntil(() => isUpdateUI == false);

        if (selectNumber == 0)
        {
            selectNumber = Random.Range(0, 5) + 1;
            Debug.Log(selectNumber);
            notChoiceUI.SetActive(true);
            yield return new WaitForSeconds(2.0f);
        }

        // 페이드 아웃 및 비활성화
        this.GetComponent<Animation>().Play("FadeOut");
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);

        // 콜백 실행
        if (callback != null)
            callback(selectNumber, this.time);
    }
}
