using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private SceneLoadingChannelSO sceneLoadingChannel;
    [SerializeField] private GameEventsSO gameEvents;

    public override void InstallBindings()
    {
        Container.Bind<SceneLoadingChannelSO>().FromInstance(sceneLoadingChannel).AsSingle().NonLazy();
        Container.Bind<GameEventsSO>().FromInstance(gameEvents).AsSingle().NonLazy();

        Container.Bind<GameProgressManager>().AsSingle().NonLazy();
        Container.Bind<CollectedAbilitiesPocket>().AsSingle().NonLazy();
    }
}
