using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public abstract class Installer : MonoBehaviour, IInstaller
    {
        public abstract void Install(IContainerBuilder builder);
    }
}