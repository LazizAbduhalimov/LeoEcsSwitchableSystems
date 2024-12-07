using System;
using Leopotam.EcsLite;

namespace Client.Game.Test
{
    public class AssignToggles : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            Type[] types =
            {
                typeof(VisionSystem), 
                typeof(MoveSystem),
                typeof(UIUpdateSystem),
            };
            foreach (var type in types)
            {
                foreach (var entity in world.Filter<CSystemSwitchToggle>().Exc<CBusy>().End())
                {
                    ref var toggle = ref world.GetPool<CSystemSwitchToggle>().Get(entity);
                    toggle.SystemType = type;
                    toggle.Toggle.gameObject.SetActive(true);
                    toggle.Toggle.Label.text = type.Name;
                    world.GetPool<CBusy>().Add(entity);
                    break;
                }
            }
        }
    }
}