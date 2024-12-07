using System;
using Leopotam.EcsLite;

namespace Laziz.EcsLite.ExtraSystems
{
    public interface ISwitchableSystem : IEcsSystem { }

    public interface ISwitchableRunSystem : ISwitchableSystem
    {
        public void Run(ISwitchableSystems systems);
    }

    public interface ISwitchablePostRunSystem : ISwitchableSystem
    {
        public void PostRun(ISwitchableSystems systems);
    }

    public interface ISwitchableSystems : IEcsSystems
    {
        public ISwitchableSystems SetActiveSystem<T>(bool isActive) where T : IEcsSystem;
        public ISwitchableSystems SetActiveSystem(Type systemType, bool isActive);
        public ISwitchableSystems Add<T>(T system, bool activeOnStart = true) where T : IEcsSystem;
        public bool GetSystemState<T>();
    }
}