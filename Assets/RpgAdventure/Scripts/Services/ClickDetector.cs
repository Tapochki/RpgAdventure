using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;

namespace TandC.RpgAdventure.Services 
{
    public class ClickDetector2D: ILoadUnit
    {
        private Camera mainCamera;

        public IObservable<GameObject> OnObjectClicked => onObjectClicked;
        private Subject<GameObject> onObjectClicked = new Subject<GameObject>();

        public async UniTask Load()
        {
            Initialize();
            await UniTask.CompletedTask;
        }

        private void Initialize()
        {
            mainCamera = Camera.main;

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Select(_ => mainCamera.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(OnClick);
        }

        private void OnClick(Vector3 worldPosition)
        {
            int placeholderLayerMask = LayerMask.GetMask("PlaceholderLayer");
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, placeholderLayerMask);

            if (hit.collider != null)
            {
                onObjectClicked.OnNext(hit.collider.gameObject);
            }
        }
    }
}

