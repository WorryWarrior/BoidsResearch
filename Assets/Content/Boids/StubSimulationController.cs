﻿using System;
using Content.Boids.Interfaces;
using Content.Infrastructure.Services.Logging;
using UnityEngine;
using Zenject;

namespace Content.Boids
{
    public class StubSimulationController : MonoBehaviour, IBoidsSimulationController
    {
        public Action Initialized { get; set; }

        private ILoggingService _loggingService;
        
        [Inject]
        private void Construct(
            ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        
        public void InitializeBoids()
        {
            _loggingService.LogError("Stub Boid Simulation Initialized", this);
            
            Initialized?.Invoke();
        }
    }
}