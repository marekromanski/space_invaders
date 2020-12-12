using UnityEngine;
using Zenject;

namespace Common
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<StartGameSignal>();

            Debug.Log("Installed project bindings");
        }
    }
}