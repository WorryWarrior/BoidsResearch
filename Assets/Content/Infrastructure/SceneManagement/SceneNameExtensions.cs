using System;

namespace Content.Infrastructure.SceneManagement
{
    public static class SceneNameExtensions
    {
        public static SceneName ToSceneName(this string sceneName)
        {
            return sceneName switch
            {
                "SCN_Boot" => SceneName.Boot,
                "SCN_Hub"      => SceneName.Meta,
                "SCN_Entitas"      => SceneName.CoreEntitas,
                "SCN_LeoEcs"      => SceneName.CoreLeoEcs,
                "SCN_Naive"      => SceneName.CoreNaive,
                _           => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static string ToSceneString(this SceneName sceneName)
        {
            return sceneName switch
            {
                SceneName.Boot => "SCN_Boot",
                SceneName.Meta      => "SCN_Hub",
                SceneName.CoreEntitas => "SCN_Entitas",
                SceneName.CoreLeoEcs => "SCN_LeoEcs",
                SceneName.CoreNaive => "SCN_Naive",
                _                   => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }
    }
}