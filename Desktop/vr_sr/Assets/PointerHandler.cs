using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

namespace VRHelper
{
    public class PointerHandler : MonoBehaviour
    {
        public SteamVR_LaserPointer laserPointer;

        void Start()
        {
            // 콜백 이벤트 동록
            //laserPointer.PointerIn += PointerInside;
            //laserPointer.PointerIn += PointerOutside;
            laserPointer.PointerClick += PointerClick;
        }

        // 포인터가 타겟에 들어간 경우
        public void PointerInside(object sender, PointerEventArgs eventArgs)
        {
            //if (eventArgs.target.CompareTag("UI"))
            //{
            //    Debug.Log("PointerInside");

            //    // 버튼 하이라이트 컬러
            //    Button button = eventArgs.target.GetComponent<Button>();
            //    if (button != null)
            //    {
            //        if (button.interactable)
            //        {
            //            button.GetComponent<Image>().color = button.colors.highlightedColor;
            //        }
            //    }
            //}
        }

        // 포인터가 타겟을 벗어난 경우
        public void PointerOutside(object sender, PointerEventArgs eventArgs)
        {
            //if (eventArgs.target.CompareTag("UI"))
            //{
            //    Debug.Log("PointerOutside");

            //    // 버튼 노말 컬러
            //    Button button = eventArgs.target.GetComponent<Button>();
            //    if (button != null)
            //    {
            //        if (button.interactable)
            //        {
            //            button.GetComponent<Image>().color = button.colors.normalColor;
            //        }
            //    }
            //}
        }

        // 포인터가 타겟을 클릭한 경우
        public void PointerClick(object sender, PointerEventArgs eventArgs)
        {
            if (eventArgs.target.CompareTag("UI"))
            {
                Debug.Log(eventArgs.target.name);

                // 버튼 이벤트 실행
                Button button = eventArgs.target.GetComponent<Button>();
                if (button != null)
                {
                    if (button.interactable)
                        button.onClick.Invoke();
                }

                Toggle toggle = eventArgs.target.GetComponent<Toggle>();
                if(toggle != null)
                {
                    if(toggle.interactable)
                    {
                        toggle.onValueChanged.Invoke(true);
                    }
                }
            }
        }
    }
}
