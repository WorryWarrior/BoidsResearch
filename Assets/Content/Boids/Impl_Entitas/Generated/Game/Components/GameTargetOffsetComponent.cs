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

    public TargetOffsetComponent targetOffset { get { return (TargetOffsetComponent)GetComponent(GameComponentsLookup.TargetOffset); } }
    public bool hasTargetOffset { get { return HasComponent(GameComponentsLookup.TargetOffset); } }

    public void AddTargetOffset(Unity.Mathematics.float3 newValue) {
        var index = GameComponentsLookup.TargetOffset;
        var component = (TargetOffsetComponent)CreateComponent(index, typeof(TargetOffsetComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTargetOffset(Unity.Mathematics.float3 newValue) {
        var index = GameComponentsLookup.TargetOffset;
        var component = (TargetOffsetComponent)CreateComponent(index, typeof(TargetOffsetComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTargetOffset() {
        RemoveComponent(GameComponentsLookup.TargetOffset);
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

    static Entitas.IMatcher<GameEntity> _matcherTargetOffset;

    public static Entitas.IMatcher<GameEntity> TargetOffset {
        get {
            if (_matcherTargetOffset == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TargetOffset);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTargetOffset = matcher;
            }

            return _matcherTargetOffset;
        }
    }
}
