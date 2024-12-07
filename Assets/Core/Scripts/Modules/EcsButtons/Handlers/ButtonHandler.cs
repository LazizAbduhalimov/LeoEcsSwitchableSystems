using Leopotam.EcsLite;
using UnityEngine.UI;

namespace UILobby
{
    public struct ButtonHandler
    {
        public Button Button;
        public EcsPackedEntity Entity;
        public EcsWorld EntityWorld;

        public void Invoke<T>(Button button, int entity, EcsWorld world) where T: struct
        {
            var packedEntity = world.PackEntity(entity);
            Button = button;
            Entity = packedEntity;
            EntityWorld = world;
            Button.onClick.AddListener(HandleClick<T>);
        }

        private void HandleClick<T>() where T : struct
        {
            Entity.Unpack(EntityWorld, out var entity);
            EntityWorld.GetPool<T>().Add(entity);
        }
    }
}