using System;
using System.Collections.Generic;
using Nevermind.MemorySystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nevermind.Lights
{

    [SelectionBase]
    [RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
    public abstract class LightEmitter : MemoryListener
    {
        // Beam data
        protected EdgeCollider2D BeamCollider;
        protected LineRenderer BeamRenderer;
        protected List<Vector2> BeamPoints;
        protected Color BeamColor = Color.white;

        // Memory bind
        protected bool IsLightEnabled = false;

        // Params and consts
        [SerializeField] protected float maxDistance = 100f;
        [SerializeField] protected int maxBeamPoints = 20;
        [SerializeField, Min(0)] protected float hitCollisionEpsilon = 0.01f;
        [SerializeField, Range(0, 1)] protected float alphaOnDeactivation = 0.2f;
        [SerializeField, Range(0, 1)] private float defaultWidthMultiplier = 0.3f;
        protected Vector2 DefaultFirstDirection;

        protected void Awake()
        {
            BeamCollider = GetComponent<EdgeCollider2D>();
            BeamRenderer = GetComponent<LineRenderer>();
            BeamPoints = new List<Vector2>();

            BeamCollider.isTrigger = true;
            BeamRenderer.useWorldSpace = true;
            BeamRenderer.widthMultiplier = defaultWidthMultiplier;
            DefaultFirstDirection = transform.right;
        }

        /** Cast the beam */
        public abstract void BeamCast();

        public abstract void UpdateBeamCollider();
        public abstract void UpdateBeamRenderer();

        protected override void onRemembered(MemoryKey key)
        {
            IsLightEnabled = true;
        }

        protected override void onForgot(MemoryKey key)
        {
            IsLightEnabled = false;
        }
    }
}