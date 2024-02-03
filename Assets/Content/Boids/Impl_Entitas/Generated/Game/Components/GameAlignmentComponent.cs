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

    public AlignmentComponent alignment { get { return (AlignmentComponent)GetComponent(GameComponentsLookup.Alignment); } }
    public bool hasAlignment { get { return HasComponent(GameComponentsLookup.Alignment); } }

    public void AddAlignment(Unity.Mathematics.float3 newValue) {
        var index = GameComponentsLookup.Alignment;
        var component = (AlignmentComponent)CreateComponent(index, typeof(AlignmentComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAlignment(Unity.Mathematics.float3 newValue) {
        var index = GameComponentsLookup.Alignment;
        var component = (AlignmentComponent)CreateComponent(index, typeof(AlignmentComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAlignment() {
        RemoveComponent(GameComponentsLookup.Alignment);
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

    static Entitas.IMatcher<GameEntity> _matcherAlignment;

    public static Entitas.IMatcher<GameEntity> Alignment {
        get {
            if (_matcherAlignment == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Alignment);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAlignment = matcher;
            }

            return _matcherAlignment;
        }
    }
}
