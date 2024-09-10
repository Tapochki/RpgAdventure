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
    public class PlayerSpawner
    {
        private Tilemap _tilemap;
        private GameObject _playerPrefab;
        private Vector3 _step;

        [Inject] private readonly FogOfWar _fogOfWar;
        [Inject] private readonly TilemapViewModel _viewModel;

        public GameObject Player { get; private set; }
        private Animator _animator;

        public void SetPlayerPrefab(GameObject playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }

        public void Initialize(Tilemap tilemap)
        {
            _tilemap = tilemap;
            _step = AppConstants.TILE_STEP;
            _animator = Player.GetComponentInChildren<Animator>();
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
            if (isFirstCreate)
            {
                _fogOfWar.UpdateFog(tileModel.Position);
            }
        }

        public void MovePlayerToTile(TileModel tile)
        {
            var worldPosition = _tilemap.CellToWorld(tile.Position) + _step;
            Player.transform.DOMove(worldPosition, 1);
            _animator.SetTrigger("Walk");

            Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
            {
                _animator.SetTrigger("Idle");
                _viewModel.SetCurrentCentralTile(tile.Position);
                _fogOfWar.UpdateFog(tile.Position);
            }).AddTo(Player);

            //Player.transform.position = worldPosition;

        }
    }
}