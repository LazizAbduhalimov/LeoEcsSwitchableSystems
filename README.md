# LeoEcsSwitchableSystems
 This is a extension for the LeoEcsLite systems to switch on/off systems in the runtime
 
# EcsSwitchableSystems - Dynamic System Management for LeoECS Lite  

EcsSwitchableSystems is an extension for the [LeoECS Lite](https://github.com/Leopotam/ecslite). It provides greater flexibility, improved debugging, and runtime optimizations for your ECS-based applications.  

## Features  

- **Runtime System Switching:** Enable or disable any system during runtime without restarting the application.  
- **Seamless Integration:** Works alongside the existing LeoECS Lite framework without modifying its core structure.  
- **Improved Performance:** Skip execution of irrelevant systems to optimize runtime behavior.  
- **Enhanced Debugging:** Isolate specific systems for detailed analysis.  

## Installation  

1. Clone this repository or copy the `EcsSwitchableSystems` implementation into your project.  
2. Ensure the LeoECS Lite framework is referenced in your project.  

## Usage  

Replace the default `EcsSystems` with `EcsSwitchableSystems` and take advantage of runtime system toggling.  

### Creating a EcsSwitchableSystems
```c#
using Laziz.EcsLite.ExtraSystems;
// ...
namespace Client {
    public sealed class Startup : MonoBehaviour 
    {
        private EcsWorld _world;        
        private EcsSwitchableSystems _switchableSystems;

        private void Start () 
        {
            _world = new EcsWorld ();
            _switchableSystems = new EcsSwitchableSystems(_world);
            //...
        }
    }
} 
```

### Adding systems
You can still add default `IEcsRunSystem` or `IEcsPostRunSystem` and define initial state (is it on/off)

```c#
public class RotationSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems) { }
}

public class MoveCameraSystem : IEcsPostRunSystem
{
    public void PostRun(IEcsSystems systems) { }
}

//...

// Add systems in your `Startup.cs` with optional initial state 
_switchableSystems
    .Add(new MovementSystem())   // is active by default
    .Add(new RenderingSystem(), activeOnStart: false) // inactive on start
    .Init();
```
> **IMPORTANT**: Init systems are called despite of state of system

### Running systems
```c#
private void Update () 
{
    _switchableSystems?.Run ();
}
```

### Enable or Disable Systems at Runtime
You can implement `IEcsSwitchableRunSystem` or `IEcsSwitchablePostRunSystem` to use `ISwitchableSystems` methonds
```c#
public class MoveSystem : ISwitchableRunSystem, ISwitchablePostRunSystem
{
    public void Run(ISwitchableSystems systems)
    {
        systems.SetActiveSystem<MoveCameraSystem>(true); // switching ON
        systems.SetActiveSystem<VisionSystem>(false);    // switching OFF
        bool moveState = systems.GetSystemState<MoveCameraSystem>();
        bool visionState = systems.GetSystemState<VisionSystem>();
    }
    
    public void PostRun(ISwitchableSystems systems) { }
}
```

### Do not forget to destroy system
```c#
private void OnDestroy () 
{
    _switchableSystems?.Destroy ();
    _switchableSystems = null;
    //...
}
```
