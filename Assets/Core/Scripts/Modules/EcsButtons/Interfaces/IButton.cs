using Leopotam.EcsLite;
using UnityEngine.UI;

namespace UILobby
{
    public interface IButton
    {
        public void Invoke(Button button, int entityToPack, EcsWorld world);
    }
}