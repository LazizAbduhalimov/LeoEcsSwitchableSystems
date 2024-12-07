using Client.Game.View;
using Leopotam.EcsLite;
using UILobby;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Client.Game.Test
{
    public class InitToggleSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var panel = Object.FindObjectOfType<UIPanelMb>();
            foreach (var toggle in panel.Toggles)
            {
                ref var cToggle = ref InitToggle(toggle.Toggle, world.GetPool<CSystemSwitchToggle>());
                cToggle.Toggle = toggle;
                cToggle.Toggle.gameObject.SetActive(false);
            }
        }
        
        public static ref T InitToggle<T>(Toggle toggle, EcsPool<T> pool, int? entity = null) where T : struct, IToggle
        {
            var world = pool.GetWorld();
            entity ??= world.NewEntity();
            ref var toggleComponent = ref pool.Add(entity.Value);
            toggleComponent.Invoke(toggle, entity.Value, world);
            return ref toggleComponent;
        }
    }
}