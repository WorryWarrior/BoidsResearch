﻿using System;

namespace Content.Boids.Interfaces
{
    public static class BoidsSimulationTypeExtensions
    {
        public static BoidsSimulationType ToBoidsSimulationType(this string sceneName)
        {
            return sceneName switch
            {
                "PFB_EntitasSystemController" => BoidsSimulationType.Entitas,
                "PFB_LeoEcsSystemController"      => BoidsSimulationType.LeoEcs,
                "PFB_NaiveSystemController"      => BoidsSimulationType.Naive,
                _           => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static string ToSimulationControllerId(this BoidsSimulationType sceneName)
        {
            return sceneName switch
            {
                BoidsSimulationType.Entitas => "PFB_EntitasSystemController",
                BoidsSimulationType.LeoEcs      => "PFB_LeoEcsSystemController",
                BoidsSimulationType.Naive => "PFB_NaiveSystemController",
                _                   => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }
    }
}