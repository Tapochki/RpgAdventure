using System;
using UniRx;
using UnityEngine;

namespace TandC.RpgAdventure.Services 
{
    public class ClickDetector2D : MonoBehaviour
    {
        private Camera mainCamera;

        public IObservable<GameObject> OnObjectClicked => onObjectClicked;
        private Subject<GameObject> onObjectClicked = new Subject<GameObject>();

        private void Start()
        {
            mainCamera = Camera.main;

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => mainCamera.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(OnClick);
        }

        private void OnClick(Vector3 worldPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                onObjectClicked.OnNext(hit.collider.gameObject);
            }
        }
    }
}

