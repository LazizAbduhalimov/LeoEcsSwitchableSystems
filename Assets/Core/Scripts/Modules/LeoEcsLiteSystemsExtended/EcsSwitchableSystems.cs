using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;

namespace Laziz.EcsLite.ExtraSystems
{
    public sealed class EcsSwitchableSystems : EcsSystems, ISwitchableSystems
    {
        private readonly List<IEcsSystem> _allSystems;
        private readonly Dictionary<Type, (IEcsSystem, bool)> _runSystems;
        private readonly Dictionary<Type, (IEcsSystem, bool)> _postRunSystems;
        
        private Type[] _runSystemsKeys;
        private Type[] _postRunSystemsKeys;

        public EcsSwitchableSystems(EcsWorld defaultWorld, object shared = null) : base(defaultWorld, shared)
        {
            _allSystems = GetAllSystems();
            _runSystems = new Dictionary<Type, (IEcsSystem, bool)>(128);
            _postRunSystems = new Dictionary<Type, (IEcsSystem, bool)>(128);
        }

        public override void Init()
        {
            base.Init();
            _runSystemsKeys = _runSystems.Keys.ToArray();
            _postRunSystemsKeys = _postRunSystems.Keys.ToArray();
        }

        public ISwitchableSystems SetActiveSystem<T>(bool isActive) where T : IEcsSystem
        {
            if (UpdateSystemState(_runSystems, isActive, typeof(T)) ||
                UpdateSystemState(_postRunSystems, isActive, typeof(T)))
            {
                return this;
            }

#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            throw new Exception($"Cannot find system of type {typeof(T).Name}");
#endif
            return this;
        }
        
        public ISwitchableSystems SetActiveSystem(Type type,bool isActive)
        {
            if (UpdateSystemState(_runSystems, isActive, type) ||
                UpdateSystemState(_postRunSystems, isActive, type))
            {
                return this;
            }

#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            throw new Exception($"Cannot find system of type {type.Name}");
#endif
            return this;
        }

        private static bool UpdateSystemState<TSystem>(IDictionary<Type, (TSystem, bool)> systems, bool state, Type type) 
            where TSystem : IEcsSystem
        {
            if (!systems.TryGetValue(type, out var system)) return false;
            systems[type] = (system.Item1, state);
            return true;
        }

        public ISwitchableSystems Add<T>(T system, bool activeOnStart = true) where T : IEcsSystem
        {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (_inited)
            {
                throw new Exception("Cant add system after initialization.");
            }
#endif
            _allSystems.Add(system);
            switch (system)
            {
                case IEcsRunSystem runSystem:
                    _runSystems.Add(typeof(T), (runSystem, activeOnStart));
                    break;
                case ISwitchableRunSystem runSystem:
                    _runSystems.Add(typeof(T), (runSystem, activeOnStart));
                    break;
                case IEcsPostRunSystem postRunSystem:
                    _postRunSystems.Add(typeof(T), (postRunSystem, activeOnStart));
                    break;
                case ISwitchablePostRunSystem postRunSystem:
                    _postRunSystems.Add(typeof(T), (postRunSystem, activeOnStart));
                    break;
            }

            return this;
        }

        public bool GetSystemState<T>() 
        {
            if (_runSystems.TryGetValue(typeof(T), out var value1)) return value1.Item2;
            if (_postRunSystems.TryGetValue(typeof(T), out var value2)) return value2.Item2;
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            throw new Exception($"Can't find system type of {typeof(T).Name}");
#endif
            return false;
        }

        public override void Run()
        {
            base.Run();
            foreach (var key in _runSystemsKeys)
            {
                var (system, haveToRun) = _runSystems[key];
                if (!haveToRun) continue;
                if (system is IEcsRunSystem runSystem) runSystem.Run(this); 
                if (system is ISwitchableRunSystem runSwitchableSystem) runSwitchableSystem.Run(this); 
                DoCheckForLeaks(system.GetType().Name);
            }
            foreach (var key in _postRunSystemsKeys)
            {
                var (system, haveToRun) = _postRunSystems[key];
                if (!haveToRun) continue;
                if (system is IEcsPostRunSystem runSystem) runSystem.PostRun(this); 
                if (system is ISwitchablePostRunSystem runSwitchableSystem) runSwitchableSystem.PostRun(this); 
                DoCheckForLeaks(system.GetType().Name);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            _runSystems.Clear();
            _postRunSystems.Clear();
        }

        private void DoCheckForLeaks(string systemName)
        {
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            var worldName = CheckForLeakedEntities(this);
            if (worldName != null)
            {
                throw new Exception(
                    $"Empty entity detected in world \"{worldName}\" after {systemName}.Run().");
            }
#endif
        }
    }
    
}