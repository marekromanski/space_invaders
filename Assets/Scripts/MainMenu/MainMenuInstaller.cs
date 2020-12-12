using Core;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("Installing Main Menu bindings");
        }
    }
}
