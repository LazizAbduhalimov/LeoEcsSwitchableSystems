using Laziz.EcsLite.ExtraSystems;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Game.Test
{
    public class VisionSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems) => Debug.Log($"Init of system {GetType().Name}");
        public void Run(IEcsSystems systems) => Debug.Log($"Executing system {GetType().Name.ToUpper()}");
    }
    
    public class MoveSystem : ISwitchablePostRunSystem
    {
        public void PostRun(ISwitchableSystems systems) => Debug.Log($"Executing system {GetType().Name.ToUpper()}");
    }
    
    public class UIUpdateSystem : ISwitchableRunSystem
    {
        public void Run(ISwitchableSystems systems) => Debug.Log($"Executing system {GetType().Name.ToUpper()}");
    }
    
    public class EndRunSystemsNotifier : IEcsRunSystem
    {
        public void Run(IEcsSystems systems) => Debug.Log("#################");
    }

    public class SwitchHandleSystem : ISwitchableRunSystem
    {
        public void Run(ISwitchableSystems systems)
        {
            var world = systems.GetWorld();
            foreach (var entity in world.Filter<ESystemSwitchToggleClicked>().End())
            {
                ref var toggle = ref world.GetPool<CSystemSwitchToggle>().Get(entity);
                var isActive = toggle.Handler.Toggle.isOn;
                var type = toggle.SystemType;
                systems.SetActiveSystem(type, isActive);
                world.GetPool<ESystemSwitchToggleClicked>().Del(entity);
            }
        }
    }
}