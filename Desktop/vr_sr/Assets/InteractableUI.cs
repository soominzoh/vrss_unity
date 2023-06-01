using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHelper
{
    public class InteractableUI : MonoBehaviour
    {
        BoxCollider boxCollider;
        RectTransform rectTransform;

        void Start()
        {
            // 해당 오브젝트 사이즈에 맞는 충돌체 생성
            rectTransform = this.GetComponent<RectTransform>();

            boxCollider = this.gameObject.AddComponent<BoxCollider>();
            boxCollider.size = rectTransform.sizeDelta;
        }
    }
}
