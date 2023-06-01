using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class LocalDataManager : MonoBehaviour
{
    public static LocalDataManager instance = null;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    [SerializeField] private List<SentenceData> sentenceDatas;
    [SerializeField] private List<CommentsData> commentsDatas;
    [SerializeField] private List<RecordData> recordDatas = new List<RecordData>();

    public CommentsData[] GetCommentsDataToPage(int _page, int number)
    {
        List<CommentsData> tempList = new List<CommentsData>();
        foreach (var item in commentsDatas)
        {
            if (item.page == _page && item.number == number)
                tempList.Add(item);
        }

        return tempList.ToArray();
    }

    public SentenceData[] GetSentenceDataToPage(int _page)
    {
        List<SentenceData> tempList = new List<SentenceData>();
        foreach (var item in sentenceDatas)
        {
            if (item.page == _page)
                tempList.Add(item);
        }

        return tempList.ToArray();
    }

    public void AddRecordData(RecordData newData)
    {
        recordDatas.Add(newData);
    }

    #region 스크립트 데이터로드
    public IEnumerator LoadJsonData()
    {
        yield return ParsingCommentsData();
        yield return ParsingSentenceData();
    }

    IEnumerator ParsingSentenceData()
    {
        sentenceDatas = new List<SentenceData>();

        JSONArray jsonArray = JSON.Parse(Resources.Load("SentenceData").ToString()) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {

            JSONObject jsonObject = jsonArray[i].AsObject;

            //아이템 오브젝트 생성 및 정보입력
            SentenceData newData = new SentenceData(
                jsonObject["page"],
                jsonObject["number"],
                jsonObject["subject"],
                jsonObject["sentence"]
                );

            sentenceDatas.Add(newData);

            yield return null;
        }
        yield return null;
    }

    IEnumerator ParsingCommentsData()
    {
        commentsDatas = new List<CommentsData>();

        JSONArray jsonArray = JSON.Parse(Resources.Load("CommentsData").ToString()) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {

            JSONObject jsonObject = jsonArray[i].AsObject;

            //아이템 오브젝트 생성 및 정보입력
            CommentsData newData = new CommentsData(
                jsonObject["page"],
                jsonObject["number"],
                jsonObject["positive"] == "TRUE" ? true : false,
                jsonObject["Who"] == "a" ? 0 : 1,
                jsonObject["comments"]
                );

            commentsDatas.Add(newData);

            yield return null;
        }
        yield return null;
    }
    #endregion

    #region 기록 데이터저장

    public void WriteCSV()
    {
        using (var writer = new CsvFileWriter(Application.dataPath + "_recordData.csv"))
        {
            List<string> columns = new List<string>() { "page", "sentenceNumber", "sentenceSecond", "surveyNumber", "surveySecond" };// making Index Row
            writer.WriteRow(columns);
            columns.Clear();

            for (int i = 0; i < recordDatas.Count; i++)
            {
                columns.Add(recordDatas[i].page.ToString()); // page
                columns.Add(recordDatas[i].sentenceNumber.ToString()); // sentence number
                columns.Add(recordDatas[i].sentenceSecond.ToString()); // sentence second
                columns.Add(recordDatas[i].surveyNumber.ToString()); // survey number
                columns.Add(recordDatas[i].surveySecond.ToString()); // survey second

                writer.WriteRow(columns);
                columns.Clear();
            }
        }
    }
    #endregion
}
