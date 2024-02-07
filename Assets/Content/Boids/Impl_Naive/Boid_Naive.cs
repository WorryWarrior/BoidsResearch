using Content.Boids.Impl_Naive.MathUtility;
using Content.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Content.Boids.Impl_Naive
{
    public class Boid_Naive : MonoBehaviour
    {
        [HideInInspector] public Vector3 boidPosition;
        [HideInInspector] public Vector3 forwardDir;
        private Vector3 velocity;

        [HideInInspector] public Vector3 avgFlockHeading;
        [HideInInspector] public Vector3 avgAvoidanceHeading;
        [HideInInspector] public Vector3 centreOfFlockmates;
        [HideInInspector] public int numPerceivedFlockmates;

        private BoidSettingsData _settingsData;

        private Transform _followTarget;

        // Pass a default mask here
        private bool IsHeadingForCollision =>
            Physics.SphereCast(boidPosition, _settingsData.BoundsRadius, forwardDir, out _,
                _settingsData.CollisionAvoidanceDistance/*, _settings.obstacleMask*/);

        public void Initialize(
            BoidSettingsData settingsData,
            Transform followTarget)
        {
            _settingsData = settingsData;
            _followTarget = followTarget;

            boidPosition = transform.position;
            forwardDir = transform.forward;

            float startSpeed = (_settingsData.MinSpeed + _settingsData.MaxSpeed) / 2;
            velocity = transform.forward * startSpeed;
        }

        public void UpdateBoid ()
        {
            Vector3 acceleration = Vector3.zero;

            if (_followTarget != null)
            {
                Vector3 offsetToTarget = _followTarget.position - boidPosition;
                acceleration = BoidHelper.SteerTowards(offsetToTarget, velocity,
                    _settingsData.MaxSpeed, _settingsData.MaxSteerForce) * _settingsData.TargetWeight;
            }

            if (numPerceivedFlockmates != 0)
            {
                centreOfFlockmates /= numPerceivedFlockmates;

                Vector3 offsetToFlockmatesCentre = centreOfFlockmates - boidPosition;

                Vector3 alignmentForce = BoidHelper.SteerTowards(avgFlockHeading, velocity,
                    _settingsData.MaxSpeed, _settingsData.MaxSteerForce) * _settingsData.AlignmentWeight;
                Vector3 cohesionForce = BoidHelper.SteerTowards(offsetToFlockmatesCentre, velocity,
                    _settingsData.MaxSpeed, _settingsData.MaxSteerForce) * _settingsData.CohesionWeight;
                Vector3 separationForce = BoidHelper.SteerTowards(avgAvoidanceHeading, velocity,
                    _settingsData.MaxSpeed, _settingsData.MaxSteerForce) * _settingsData.SeparationWeight;

                acceleration += alignmentForce;
                acceleration += cohesionForce;
                acceleration += separationForce;
            }

            if (_settingsData.CollisionAvoidanceWeight > 0 && IsHeadingForCollision)
            {
                Vector3 collisionAvoidDir = ObstacleRays();
                Vector3 collisionAvoidForce = BoidHelper.SteerTowards(collisionAvoidDir, velocity,
                    _settingsData.MaxSpeed, _settingsData.MaxSteerForce) * _settingsData.CollisionAvoidanceWeight;
                acceleration += collisionAvoidForce;
            }

            velocity += acceleration * Time.deltaTime;
            float speed = velocity.magnitude;
            Vector3 dir = velocity / speed;
            speed = Mathf.Clamp(speed, _settingsData.MinSpeed, _settingsData.MaxSpeed);
            velocity = dir * speed;

            transform.position += velocity * Time.deltaTime;
            transform.forward = dir;
            boidPosition = transform.position;
            forwardDir = dir;
        }

        private Vector3 ObstacleRays()
        {
            Vector3[] rayDirections = BoidHelper._directions;

            for (int i = 0; i < rayDirections.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(rayDirections[i]);
                Ray ray = new(boidPosition, dir);

                if (!Physics.SphereCast(ray, _settingsData.BoundsRadius,
                        _settingsData.CollisionAvoidanceDistance/*, _settings.obstacleMask*/))
                {
                    return dir;
                }
            }

            return forwardDir;
        }
    }
}