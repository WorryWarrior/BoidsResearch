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

    public FlockmateNumberComponent flockmateNumber { get { return (FlockmateNumberComponent)GetComponent(GameComponentsLookup.FlockmateNumber); } }
    public bool hasFlockmateNumber { get { return HasComponent(GameComponentsLookup.FlockmateNumber); } }

    public void AddFlockmateNumber(int newValue) {
        var index = GameComponentsLookup.FlockmateNumber;
        var component = (FlockmateNumberComponent)CreateComponent(index, typeof(FlockmateNumberComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceFlockmateNumber(int newValue) {
        var index = GameComponentsLookup.FlockmateNumber;
        var component = (FlockmateNumberComponent)CreateComponent(index, typeof(FlockmateNumberComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveFlockmateNumber() {
        RemoveComponent(GameComponentsLookup.FlockmateNumber);
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

    static Entitas.IMatcher<GameEntity> _matcherFlockmateNumber;

    public static Entitas.IMatcher<GameEntity> FlockmateNumber {
        get {
            if (_matcherFlockmateNumber == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.FlockmateNumber);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherFlockmateNumber = matcher;
            }

            return _matcherFlockmateNumber;
        }
    }
}
