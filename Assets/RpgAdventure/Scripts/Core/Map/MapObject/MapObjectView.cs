using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace TandC.RpgAdventure.Core.Map.MapObject 
{
    public class MapObjectView
    {
        [Inject] private readonly StructureConfig _config;
        [Inject] private readonly MapObjectViewModel _mapObjectViewModel;

        private Transform _structureParent;
        private Tilemap _tilemap;
        private Vector3 _step;

        private List<MapObjectBehaviour> _spawnedObject = new List<MapObjectBehaviour>();

        private GameObject _currentInteractedObject;

        public void Initialize(Tilemap tilemap)
        {
            _step = AppConstants.TILE_STEP;
            _structureParent = new GameObject("StructureParent").transform;
            _structureParent.transform.position = Vector3.zero;
            _tilemap = tilemap;
            _mapObjectViewModel.StructureDeleteEvent += OnStructureDeleted;
            _mapObjectViewModel.NewStructureCreateEvent += CreateMapObjectView;
        }

        public void CreateMapObjectView(MapObjectModel model)
        {
            GameObject prefab = _config.GetStructureGameobject(model.Type);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab for {model.Type} not found.");
                return;
            }

            GameObject structureObject = MonoBehaviour.Instantiate(prefab, _structureParent);
            structureObject.transform.position = _tilemap.CellToWorld(model.Position) + _step;

            var mapObjectBehaviour = structureObject.GetComponent<MapObjectBehaviour>();
            _spawnedObject.Add(mapObjectBehaviour);

            if (mapObjectBehaviour != null)
            {
                mapObjectBehaviour.Initialize(model);
                mapObjectBehaviour.OnPlayerInteract += HandlePlayerInteraction;
                mapObjectBehaviour.OnPlayerExit += HandlePlayerExit;
            }
            else
            {
                Debug.LogWarning("MapObjectBehaviour not found on the instantiated prefab.");
            }
        }

        private void HandlePlayerInteraction(GameObject obj)
        {
            if (_currentInteractedObject != null)
            {
                ClearCurrentInteraction();
            }

            _currentInteractedObject = obj;
            Debug.Log($"Player started interacting with {_currentInteractedObject.name}");
        }

        private void HandlePlayerExit(GameObject obj)
        {
            if (_currentInteractedObject == obj)
            {
                ClearCurrentInteraction();
            }
        }

        private void ClearCurrentInteraction()
        {
            Debug.Log($"Player stopped interacting with {_currentInteractedObject.name}");
            _currentInteractedObject = null;
        }

        private void OnStructureDeleted(Vector3Int position)
        {
            var objectToDelete = GetObjectAtPosition(position);
            if (objectToDelete != null)
                DeleteStructure(objectToDelete);
        }

        private void DeleteStructure(MapObjectBehaviour structure) 
        {
            _spawnedObject.Remove(structure);
            structure.Dispose();
        }

        public MapObjectBehaviour GetObjectAtPosition(Vector3Int position) => _spawnedObject.FirstOrDefault(t => t.GetPosition() == position);
    }
}

