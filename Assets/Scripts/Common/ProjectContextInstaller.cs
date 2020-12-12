using UnityEngine;
using Zenject;

namespace Common
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Installed project bindings");
        }
    }
}