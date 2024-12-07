using Leopotam.EcsLite;
using UnityEngine.UI;

namespace UILobby
{
    public interface IToggle
    {
        public void Invoke(Toggle toggle, int entity, EcsWorld world);
    }
}