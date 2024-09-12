using DG.Tweening;
using System;
using TandC.RpgAdventure.Core.Map;
using TandC.RpgAdventure.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.Player
{
    public class PlayerController
    {
        public Action OnPlayerMoveStart;
        public Action OnPlayerMoveEnd;

        private Tilemap _tilemap;
        private GameObject _playerPrefab;
        private Vector3 _step;

        [Inject] private readonly FogOfWar _fogOfWar;
        [Inject] private readonly TilemapViewModel _viewModel;

        public GameObject Player { get; private set; }
        private Animator _animator;
        private SpriteRenderer _model;

        public void SetPlayerPrefab(GameObject playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }

        public void Initialize(Tilemap tilemap)
        {
            _tilemap = tilemap;
            _step = AppConstants.TILE_STEP;
        }

        public void RespawnPlayer()
        {
            MonoBehaviour.Destroy(Player);
            SpawnPlayer();
        }

        public void SpawnPlayer(TileModel tileModel = null)
        {
            bool isFirstCreate = false;
            if (tileModel == null)
            {
                tileModel = _viewModel.GetSpawnPlayerTile();
                isFirstCreate = true;
            }

            if (tileModel != null)
            {
                CreatePlayer(tileModel, isFirstCreate);
            }
        }

        public void CreatePlayer(TileModel tileModel, bool isFirstCreate)
        {
            var worldPosition = _tilemap.CellToWorld(tileModel.Position) + _step;
            Player = MonoBehaviour.Instantiate(_playerPrefab, worldPosition, Quaternion.identity);
            _model =  Player.GetComponentInChildren<SpriteRenderer>();
            _animator = _model.GetComponent<Animator>();
            if (isFirstCreate)
            {
                _fogOfWar.UpdateFog(tileModel.Position);
            }
        }

        public void MovePlayerToTile(TileModel tile)
        {
            OnPlayerMoveStart?.Invoke();
            var worldPosition = _tilemap.CellToWorld(tile.Position) + _step;
            Player.transform.DOMove(worldPosition, 0.7f);
            _animator.SetTrigger("Walk");
            _model.flipX = worldPosition.x < Player.transform.position.x;

            Observable.Timer(TimeSpan.FromSeconds(0.7f))
                    .Subscribe(_ =>
            {
                _animator.SetTrigger("Idle");
                OnPlayerMoveEnd?.Invoke();
            }).AddTo(Player);

            UpdateTileAndFog(tile.Position);
        }

        private void UpdateTileAndFog(Vector3Int position)
        {
            
            _fogOfWar.UpdateFog(position);
        }
    }
}