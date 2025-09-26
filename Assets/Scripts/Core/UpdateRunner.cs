using Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Core
{
    public class UpdateRunner : MonoBehaviour
    {
        private IUpdateService updateService;

        [Inject]
        public void Construct(IUpdateService us)
        {
            updateService = us;
        }

        private void Update()
        {
            updateService.TickUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            updateService.TickFixedUpdate(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            updateService.TickLateUpdate(Time.deltaTime);
        }
    }
}