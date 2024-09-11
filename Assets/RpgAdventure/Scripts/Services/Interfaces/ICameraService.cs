using UnityEngine;

namespace TandC.RpgAdventure.Services 
{
    public interface ICameraService
    {
        void Init(Transform playerTransform);
        void BackToPlayer();
    }
}

