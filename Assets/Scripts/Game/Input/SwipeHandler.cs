using System;
using Game.Input.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class SwipeHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ISwipeHandler
    {
        [Range(0f, 1f)] public float swipeThreshold = 0.5f;

        public event Action OnSwipeUp = () => { };
        public event Action OnSwipeDown = () => { };
        public event Action OnSwipeRight = () => { };
        public event Action OnSwipeLeft = () => { };

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData data)
        {
            Vector2 dir = (data.position - data.pressPosition).normalized;

            if (dir.x > swipeThreshold)
                OnSwipeRight.Invoke();

            if (dir.x < -swipeThreshold)
                OnSwipeLeft.Invoke();

            if (dir.y > swipeThreshold)
                OnSwipeUp.Invoke();

            if (dir.y < -swipeThreshold)
                OnSwipeDown.Invoke();
        }
    }
}