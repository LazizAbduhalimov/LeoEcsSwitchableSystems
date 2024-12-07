using Leopotam.EcsLite;
using UnityEngine.UI;

namespace UILobby
{
    public struct ToggleHandler
    {
        public Toggle Toggle;
        public EcsPackedEntity Entity;
        public EcsWorld EntityWorld;

        public void Invoke<T>(Toggle toggle, int entity, EcsWorld world) where T: struct, IToggleEvent
        {
            var packedEntity = world.PackEntity(entity);
            Toggle = toggle;
            Entity = packedEntity;
            EntityWorld = world;
            Toggle.onValueChanged.AddListener(HandleClick<T>);
        }

        private void HandleClick<T>(bool isActive) where T : struct, IToggleEvent
        {
            Entity.Unpack(EntityWorld, out var entity);
            EntityWorld.GetPool<T>().Add(entity).IsActive = isActive;
        }
    }
}