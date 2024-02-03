//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Content.Boids.Impl_Entitas.Components;

public partial class GameEntity {

    public AvoidanceComponent avoidance { get { return (AvoidanceComponent)GetComponent(GameComponentsLookup.Avoidance); } }
    public bool hasAvoidance { get { return HasComponent(GameComponentsLookup.Avoidance); } }

    public void AddAvoidance(Unity.Mathematics.float3 newValue) {
        var index = GameComponentsLookup.Avoidance;
        var component = (AvoidanceComponent)CreateComponent(index, typeof(AvoidanceComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAvoidance(Unity.Mathematics.float3 newValue) {
        var index = GameComponentsLookup.Avoidance;
        var component = (AvoidanceComponent)CreateComponent(index, typeof(AvoidanceComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAvoidance() {
        RemoveComponent(GameComponentsLookup.Avoidance);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherAvoidance;

    public static Entitas.IMatcher<GameEntity> Avoidance {
        get {
            if (_matcherAvoidance == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Avoidance);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAvoidance = matcher;
            }

            return _matcherAvoidance;
        }
    }
}
