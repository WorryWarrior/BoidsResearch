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

    public LinkedGOComponent linkedGO { get { return (LinkedGOComponent)GetComponent(GameComponentsLookup.LinkedGO); } }
    public bool hasLinkedGO { get { return HasComponent(GameComponentsLookup.LinkedGO); } }

    public void AddLinkedGO(UnityEngine.GameObject newLinkedGO) {
        var index = GameComponentsLookup.LinkedGO;
        var component = (LinkedGOComponent)CreateComponent(index, typeof(LinkedGOComponent));
        component.linkedGO = newLinkedGO;
        AddComponent(index, component);
    }

    public void ReplaceLinkedGO(UnityEngine.GameObject newLinkedGO) {
        var index = GameComponentsLookup.LinkedGO;
        var component = (LinkedGOComponent)CreateComponent(index, typeof(LinkedGOComponent));
        component.linkedGO = newLinkedGO;
        ReplaceComponent(index, component);
    }

    public void RemoveLinkedGO() {
        RemoveComponent(GameComponentsLookup.LinkedGO);
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

    static Entitas.IMatcher<GameEntity> _matcherLinkedGO;

    public static Entitas.IMatcher<GameEntity> LinkedGO {
        get {
            if (_matcherLinkedGO == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LinkedGO);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLinkedGO = matcher;
            }

            return _matcherLinkedGO;
        }
    }
}
