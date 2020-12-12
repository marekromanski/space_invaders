using UnityEngine;
using Zenject;

public class LoadingInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("Installing bindings");
        Container.Bind<GameLoader>().AsSingle().NonLazy();
    }
}