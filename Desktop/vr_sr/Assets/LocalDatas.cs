using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SentenceData
{
    public int page;
    public int number;
    public string subject;
    public string sentence;

    public SentenceData(int _page, int _number, string _subject, string _sentnce)
    {
        page = _page;
        number = _number;
        subject = _subject;
        sentence = _sentnce;
    }
}

[System.Serializable]
public struct CommentsData
{
    public int page;
    public int number;
    public bool isPositive;
    public int who;
    public string comments;

    public CommentsData(int _page, int _number, bool _isPositive, int _who, string _comments)
    {
        page = _page;
        number = _number;
        isPositive = _isPositive;
        who = _who;
        comments = _comments;
    }
}

[System.Serializable]
public class RecordData
{
    public int page;
    public int sentenceNumber;
    public float sentenceSecond;
    public int surveyNumber;
    public float surveySecond;

    public RecordData(int _page, int _sentenceNumber, float _sentenceSecond, int _surveyNumber, float _surveySecond)
    {
        page = _page;
        sentenceNumber = _sentenceNumber;
        sentenceSecond = _sentenceSecond;
        surveyNumber = _surveyNumber;
        surveySecond = _surveySecond;
    }
}