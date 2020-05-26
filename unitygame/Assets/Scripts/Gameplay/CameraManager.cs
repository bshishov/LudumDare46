using System;
using Gameplay.States;
using Messages;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject[] cameras;

        private TinyMessageSubscriptionToken _subscription;

        private void Start()
        {
            _subscription = EventBus.Subscribe<GameStateChanged>(OnGameStateChanged);
            SwitchToCamera(1); // switch to static camera
        }

        private void OnGameStateChanged(GameStateChanged obj)
        {
            var state = obj.Content;
            if (state == GameState.Idle)
            {
                SwitchToCamera(0);
            }
            else
            {
                SwitchToCamera(1);
            }
        }

        private void SwitchToCamera(int num)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(false);
            }
            cameras[num].SetActive(true);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(_subscription);
        }
    }
}
