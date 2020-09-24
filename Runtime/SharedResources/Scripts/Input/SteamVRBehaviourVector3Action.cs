﻿namespace Tilia.SDK.SteamVR.Input
{
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Valve.VR;
    using Zinnia.Action;

    /// <summary>
    /// Listens for the linked <see cref="Vector3"/> behavior and emits the appropriate action.
    /// </summary>
    public class SteamVRBehaviourVector3Action : Vector3Action
    {
        /// <summary>
        /// The type of value to use.
        /// </summary>
        public enum ValueType
        {
            /// <summary>
            /// The actual value of the axis.
            /// </summary>
            Actual,
            /// <summary>
            /// The delta value of the axis.
            /// </summary>
            Delta
        }

        /// <summary>
        /// The SteamVR Vector3 Behavior to link this action to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        SteamVR_Behaviour_Vector3 LinkedVector3Behaviour { get; set; }
        /// <summary>
        /// The value to receive from the axis data.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        ValueType AxisValue { get; set; } = ValueType.Actual;

        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterListeners();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnregisterListeners();
        }

        /// <summary>
        /// Registers the listeners against the <see cref="LinkedVector3Behaviour"/>.
        /// </summary>
        protected virtual void RegisterListeners()
        {
            if (LinkedVector3Behaviour == null)
            {
                return;
            }

            LinkedVector3Behaviour.onAxis.AddListener(Listener);
        }

        /// <summary>
        /// Unregisters the listeners against the <see cref="LinkedVector3Behaviour"/>.
        /// </summary>
        protected virtual void UnregisterListeners()
        {
            if (LinkedVector3Behaviour == null)
            {
                return;
            }

            LinkedVector3Behaviour.onAxis.RemoveListener(Listener);
        }

        /// <summary>
        /// Processes the event and re-emits the data via the <see cref="Vector3Action"/>.
        /// </summary>
        /// <param name="action">The action raising the event.</param>
        /// <param name="source">The source of the input.</param>
        /// <param name="value">The value of the raised event.</param>
        /// <param name="delta">The delta value between now and the previous raised event.</param>
        protected virtual void Listener(SteamVR_Behaviour_Vector3 action, SteamVR_Input_Sources source, Vector3 value, Vector3 delta)
        {
            switch (AxisValue)
            {
                case ValueType.Actual:
                    Receive(value);
                    break;
                case ValueType.Delta:
                    Receive(delta);
                    break;
            }
        }

        /// <summary>
        /// Called before <see cref="LinkedVector3Behaviour"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(LinkedVector3Behaviour))]
        protected virtual void OnBeforeLinkedVector3BehaviourChange()
        {
            UnregisterListeners();
        }

        /// <summary>
        /// Called after <see cref="LinkedVector3Behaviour"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(LinkedVector3Behaviour))]
        protected virtual void OnAfterLinkedVector3BehaviourChange()
        {
            RegisterListeners();
        }
    }
}