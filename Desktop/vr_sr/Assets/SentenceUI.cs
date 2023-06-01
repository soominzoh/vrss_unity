using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceUI : MonoBehaviour
{
    public Text titleUI;
    public Button[] buttons;
    
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

    public IEnumerator UpdateUI(SentenceData[] sentenceDatas, float limitTime, System.Action<int, float> callback)
    {

        // 지문이 4개가 아닌 경우 실행하지 않고 경고문 출력
        if(sentenceDatas.Length != 4)
        {
            Debug.LogWarning("Check the sentenceDatas count");
            yield return null;
        }

        waitUI.SetActive(false);
        notChoiceUI.SetActive(false);

        // 로컬 파라미터 초기화
        this.time = 0;
        isTimer = true;
        isUpdateUI = true;
        currentLimitTime = limitTime;

        // 주제 내용 설정
        titleUI.text = sentenceDatas[0].subject;
        // 각 지문 내용 설정
        buttons[0].GetComponent<Text>().text = sentenceDatas[0].sentence;
        buttons[1].GetComponent<Text>().text = sentenceDatas[1].sentence;
        buttons[2].GetComponent<Text>().text = sentenceDatas[2].sentence;
        buttons[3].GetComponent<Text>().text = sentenceDatas[3].sentence;

        // 버튼 이벤트 초기화
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
        buttons[3].onClick.RemoveAllListeners();

        int selectNumber = 0;
        // 버튼 이벤트 등록
        buttons[0].onClick.AddListener(() => {
            selectNumber = 1;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        buttons[1].onClick.AddListener(() => {
            selectNumber = 2;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        buttons[2].onClick.AddListener(() => {
            selectNumber = 3;
            isTimer = false;
            waitUI.SetActive(true);
            AudioManager.instance.Play("click");
        });
        buttons[3].onClick.AddListener(() => {
            selectNumber = 4;
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

        if(selectNumber == 0)
        {
            selectNumber = Random.Range(0, 4) + 1;
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
