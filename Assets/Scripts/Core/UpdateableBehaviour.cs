using System;
using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Core
{
    public abstract class UpdateableBehaviour : MonoBehaviour, IUpdateable
    {
        [SerializeField] private UpdateType updateType = UpdateType.Update;
        private IUpdateService updateService;
        private bool isRegistered;

        [Inject]
        public void Construct(IUpdateService us)
        {
            updateService = us;
            if(gameObject.activeSelf)
                RegisterToUpdate();
        }

        protected virtual void OnEnable()
        {
            RegisterToUpdate();
        }
        
        protected virtual void OnDisable()
        {
            UnregisterFromUpdate();
        }

        private void UnregisterFromUpdate()
        {
            if (updateService == null || !isRegistered) return;
            isRegistered = false;
            switch (updateType)
            {
                case UpdateType.Update:
                    updateService.UnregisterFromUpdate(this);
                    break;
                case UpdateType.FixedUpdate:
                    updateService.UnregisterFromFixedUpdate(this);
                    break;
                case UpdateType.LateUpdate:
                    updateService.UnregisterFromLateUpdate(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public abstract void OnUpdate(float deltaTime);
        
        private void RegisterToUpdate()
        {
            if(updateService == null || isRegistered) return;
            isRegistered = true;
            switch (updateType)
            {
                case UpdateType.Update:
                    updateService.RegisterToUpdate(this);
                    break;
                case UpdateType.FixedUpdate:
                    updateService.RegisterToFixedUpdate(this);
                    break;
                case UpdateType.LateUpdate:
                    updateService.RegisterToLateUpdate(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate,
    }
}