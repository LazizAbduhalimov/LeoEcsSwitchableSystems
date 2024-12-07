using Leopotam.EcsLite;
using UnityEngine;
using AB_Utility.FromSceneToEntityConverter;
using Client.Game.Test;
using Laziz.EcsLite.ExtraSystems;
using Leopotam.EcsLite.Di;

namespace Client {
    public sealed class Startup : MonoBehaviour 
    {
        private EcsWorld _world;        
        private EcsSwitchableSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _initSystems;

        private void Start () 
        {
            _world = new EcsWorld ();
            _initSystems = new EcsSystems(_world);
            _updateSystems = new EcsSwitchableSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);
            Utils.EcsWorld = _world;
            AddInitSystems();
            AddRunSystems();
            AddEditorSystems();

            InjectAllSystems(_initSystems, _updateSystems, _fixedUpdateSystems);
            
            _initSystems.Init();
            _fixedUpdateSystems.Init();
            _updateSystems.ConvertScene().Init();
        }

        private void Update () 
        {
            _updateSystems?.Run ();
        }
        
        private void FixedUpdate() 
        {
            _fixedUpdateSystems?.Run();
        }

        private void AddInitSystems()
        {
            _initSystems
                .Add(new InitToggleSystem())
                .Add(new AssignToggles())
                ;
        }
        
        private void AddRunSystems()
        {
            _updateSystems
                .Add(new VisionSystem())
                .Add(new MoveSystem())
                .Add(new UIUpdateSystem())
                .Add(new SwitchHandleSystem()) // used for switching system
                .Add(new EndRunSystemsNotifier())                         
                ;
        }

        private void OnDestroy () 
        {
            _updateSystems?.Destroy ();
            _updateSystems = null;

            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems = null;

            _world?.Destroy ();
            _world = null;
        }

        private void AddEditorSystems() 
        {
            // #if UNITY_EDITOR
            //     _updateSystems
            //         .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ());
            // #endif
        }

        private void InjectAllSystems(params IEcsSystems[] systems)
        {
            foreach (var system in systems) { }
        }
    }
}