﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoidHelper {
    
    private const int numViewDirections = 300;
    public static readonly Vector3[] directions;

    static BoidHelper() 
    {
        directions = new Vector3[numViewDirections];

        float goldenRatio = (1 + Mathf.Sqrt (5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numViewDirections; i++)
        {
            float t = (float) i / numViewDirections;
            float inclination = Mathf.Acos (1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            directions[i] = new Vector3 (x, y, z);
        }
    }
    
    public static Vector3 SteerTowards(Vector3 vector, Vector3 velocity, float maxSpeed, float maxSteerForce)
    {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, maxSteerForce);
    }
    
}