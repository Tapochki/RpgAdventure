using System;
using TandC.RpgAdventure.Core.Player;
using UnityEngine;

namespace TandC.RpgAdventure.Core.Map.MapObject 
{
    public class MapObjectBehaviour : MonoBehaviour
    {
        private MapObjectModel _model;

        private SpriteRenderer _spriteRenderer;

        public event Action<GameObject> OnPlayerInteract;
        public event Action<GameObject> OnPlayerExit;

        public void Initialize(MapObjectModel model)
        {
            _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            _model = model;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.LogError(collision);
            if (collision.gameObject.TryGetComponent<PlayerView>(out var playerView))
            {
                HandlePlayerInteraction();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerView>(out var playerView))
            {
                HandlePlayerExit();
            }
        }

        public void UpLayer() 
        {
            _spriteRenderer.sortingOrder = 19;
        }

        private void HandlePlayerInteraction()
        {
            Debug.Log($"Player interacted with {_model.Type} at {_model.Position}");
            OnPlayerInteract?.Invoke(gameObject);
        }

        private void HandlePlayerExit()
        {
            Debug.Log($"Player stopped interacting with {_model.Type} at {_model.Position}");
            OnPlayerExit?.Invoke(gameObject);
        }

        public Vector3Int GetPosition()
        {
            return _model.Position;
        }

        public void Dispose() 
        {
            MonoBehaviour.Destroy(gameObject);
        }
    }
}

