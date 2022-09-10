using System;
using Game.Input.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class MonoSwipeHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ISwipeHandler
    {
        [Range(0f, 1f)] public float swipeThreshold = 0.5f;

        public event Action<Vector2Int> OnSwipe = _ => { };

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData data)
        {
            var dir = (data.position - data.pressPosition).normalized;

            if (dir.x > swipeThreshold)
                OnSwipe.Invoke(Vector2Int.right);

            if (dir.x < -swipeThreshold)
                OnSwipe.Invoke(Vector2Int.left);

            if (dir.y > swipeThreshold)
                OnSwipe.Invoke(Vector2Int.up);

            if (dir.y < -swipeThreshold)
                OnSwipe.Invoke(Vector2Int.down);
        }
    }
}