using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject commentUI;
    [Header("코멘트 노출 시간")]
    [SerializeField] private float commentTime = 6f; //3f에서 6f로 변경
    [Header("타이핑 시간")]
    [SerializeField] private float typingDelya = 2.0f;

    [SerializeField] Animator animator;

    public void ReceiveAnimationEvent()
    {
        int randomIndex = Random.Range(1, 3);
        animator.SetInteger("index", randomIndex);
    }

    public IEnumerator Comments(CommentsData data, System.Action callback)
    {
        StartCoroutine(CommentsUI("...", null));

        yield return new WaitForSeconds(typingDelya);

        StartCoroutine(CommentsUI(data.comments, callback));

        int randomIndex = Random.Range(0, 2);
        animator.SetInteger("index", randomIndex);
        string trigger = data.isPositive == true ? "positive" : "negative";
        animator.SetTrigger(trigger);

        AudioManager.instance.Play("comment");
    }

    IEnumerator CommentsUI(string comments, System.Action _callback)
    {
        // 페이드로 변경!
        commentUI.SetActive(true);
        commentUI.transform.Find("UI").Find("Text_Commet").GetComponent<Text>().text = comments;

        yield return new WaitForSeconds(commentTime);

        if (_callback != null)
            _callback();

        // 페이드로 변경!
        commentUI.SetActive(false);
    }
}
