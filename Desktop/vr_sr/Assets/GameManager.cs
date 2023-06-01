using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SentenceUI sentenceUI;
    [SerializeField] private SurveyUI surveyUI;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private LocalDataManager dataManager;

    [Header("최종 문항수")]
    [SerializeField] private int maxPage = 0;
    int currentPage = 1;

    [Header("최대 지문 선택 시간")]
    [SerializeField] private int sentenceMinTime = 25;
    [Header("최소 지문 선택 시간")]
    [SerializeField] private int sentenceMaxTime = 30;
    [Header("설문조사 시간")]
    [SerializeField] private float surveyMaxTime = 6;

    [SerializeField] private NPC[] npcs;
    [SerializeField] private float commentDely = 1f;
    [SerializeField] private float commentOverSpareTime = 1.5f;

    [SerializeField] SteamVR_Action_Boolean input;

    void Awake()
    {
        sentenceUI.gameObject.SetActive(false);
        surveyUI.gameObject.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (input.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (Time.timeScale >= 1f)
            {
                Pause();
            }
            else if (Time.timeScale <= 0)
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;

        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f;

        pauseUI.SetActive(false);
    }
    
    IEnumerator GameLoop()
    {
        yield return dataManager.LoadJsonData();

        yield return NPCSittingMotion();

        for (int i = 0; i < maxPage; i++)
        {
            currentPage = i + 1;

            // 기록 데이터 생성
            RecordData newRecord = new RecordData(currentPage, 0, 0, 0, 0);

            yield return SentenceSelect(newRecord);         
            yield return CommentsNPC(newRecord);
            yield return SurveySelect(newRecord);

            // 기록 테이저 저장
            LocalDataManager.instance.AddRecordData(newRecord);
        }

        yield return GameOver();
    }

    IEnumerator NPCSittingMotion()
    {
        yield return new WaitForSeconds(3.0f);
    }

    IEnumerator SentenceSelect(RecordData recordData)
    {
        bool isdone = false;
        System.Action<int, float> requestCallback = (selectNumber, second) =>
        {
            // 기록 데이터 입력
            recordData.sentenceNumber = selectNumber;
            recordData.sentenceSecond = second;

            isdone = true;
        };

        StartCoroutine(sentenceUI.UpdateUI(dataManager.GetSentenceDataToPage(currentPage), Random.Range(sentenceMinTime, sentenceMaxTime + 1), requestCallback));

        yield return new WaitUntil(() => isdone == true);
    }

    IEnumerator SurveySelect(RecordData recordData)
    {
        bool isdone = false;
        System.Action<int, float> requestCallback = (selectNumber, second) =>
        {
            // 기록 데이터 입력
            recordData.surveyNumber = selectNumber;
            recordData.surveySecond = second;

            isdone = true;
        };

        StartCoroutine(surveyUI.UpdateUI(surveyMaxTime, requestCallback));

        yield return new WaitUntil(() => isdone == true);
    }

    IEnumerator CommentsNPC(RecordData recordData)
    {
        int doneCount = 0;
        System.Action requestCallback = () =>
        {
            doneCount++;
        };

        CommentsData[] datas = dataManager.GetCommentsDataToPage(currentPage, recordData.sentenceNumber);

        for (int i = 0; i < npcs.Length; i++)
        {
            StartCoroutine(npcs[i].Comments(datas[i], requestCallback));

            yield return new WaitForSeconds(commentDely);
        }

        yield return new WaitUntil(() => doneCount == npcs.Length);

        yield return new WaitForSeconds(commentOverSpareTime);
    }

    IEnumerator GameOver()
    {
        dataManager.WriteCSV();
        gameoverUI.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }
}
