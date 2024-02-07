# Boids Research

Pet project, developed for exercising in various concepts and techniques.

## Project Structure
Project consists of three major sections:
- [Code Infrastructure](Assets/Content/Infrastructure) - Services, Providers, Factories etc.
- [UI](Assets/Content/Hub) - Main Menu, Settings windows etc.
- [Boids Implementations](Assets/Content/Boids) - Naive, Entitas and DOTS implementations of identical logic

## Technical Details
 - Code infrastructure extensively relies on a number of **services and providers** for asset access, game progress saving/loading,
user input handling and other tasks. It also makes use of **State Machine** to provide intuitive control 
over possible game states with explicit entry points etc.
 - Assets used in the project include:
    - **Zenject** - Architecture DI backbone 
    - **DOTween** - Procedural animations
    - **RSG Promises** - UI reactive logic 
 - Unity solutions used in the project:
    - **Addressables** - Asset management
    - **Input System** - Highly configurable player input handling
    - **Cinemachine** - First-person camera control
- Both UI and gameplay elements are constructed with **factory methods** to meet all dependency requirements.
All dependencies are injected either in constructors, either in separate `Construct` method, when constructors 
could not be used.
- All adaptations of existing simulation parameter calculation code are covered with **Unit Tests**

## Boid Simulation
Project includes three boid simulation implementations:
- **Naive** - based on plain MonoBehaviours
- **Entitas** - makes use of Entitas **ECS** framework, Unity **Job System** and **Burst** compiler to perform calculations,
uses standard GameObjects for visual representation
- **Native** - takes advantage of all Unity DOTS tools, including **Pure Entity Renderer**, native **ECS** implementation and
**multi-threaded job parallelization**