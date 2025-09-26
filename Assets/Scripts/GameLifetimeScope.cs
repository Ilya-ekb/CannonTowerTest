using Core;
using Installers;
using Services;
using Services.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<UpdateService>(Lifetime.Singleton).As<IUpdateService>();
        
        builder.RegisterComponentInHierarchy<UpdateRunner>();

        var installers = FindObjectsByType<Installer>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        foreach (var installer in installers)
            installer.Install(builder);
    }
}