using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private MapGenerator mapGenerator;
  
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();
        Container.Bind<MapGenerator>().FromInstance(mapGenerator).AsSingle().NonLazy();
    }
}
