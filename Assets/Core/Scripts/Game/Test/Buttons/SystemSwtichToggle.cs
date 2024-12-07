using System;
using Core.Scripts.Game.Test.View;
using Leopotam.EcsLite;
using UILobby;
using UnityEngine.UI;

namespace Client.Game.Test
{
    public struct ESystemSwitchToggleClicked : IToggleEvent 
    {
        public bool IsActive { get; set; }
    }
  
    public struct CSystemSwitchToggle : IToggle
    {
        public ToggleWithLabel Toggle;
        public Type SystemType;
        public ToggleHandler Handler; 

        public void Invoke(Toggle toggle, int entity, EcsWorld world)
        {
            Handler.Invoke<ESystemSwitchToggleClicked>(toggle, entity, world);
        }
    }
}