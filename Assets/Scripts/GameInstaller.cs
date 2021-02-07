using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(FindObjectOfType<GameManager>()).NonLazy();
        Container.Bind<UIManager>().FromInstance(FindObjectOfType<UIManager>()).NonLazy();
        Container.Bind<TurnManager>().FromInstance(FindObjectOfType<TurnManager>()).NonLazy();
        Container.Bind<ActionManager>().FromInstance(FindObjectOfType<ActionManager>()).NonLazy();
        Container.Bind<BattleManager>().FromInstance(FindObjectOfType<BattleManager>()).NonLazy();
    }
}