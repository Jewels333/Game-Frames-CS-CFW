using System;

namespace Framework.Structures
{
    /// <summary>
    /// Game State Change event args.
    /// </summary>
    internal class GSCEventArgs : EventArgs
    {
        internal GSCEventArgs() { }
    }
    /// <summary>
    /// Registered Script event args. (Used in <see cref="IRegisteredScript"/>)
    /// </summary>
    internal class RSEventArgs : EventArgs
    {
        public StateEnumerable State;
        internal RSEventArgs(StateEnumerable state)
        {
            State = state;
        }
    }
    internal class RCHEventArgs { }
    internal class RCHResponse { }
    /// <summary>
    /// The Registered Script interface.
    /// </summary>
    /// <remarks>Includes 
    /// <see cref="int">ID</see>, 
    /// <see cref="StateEnumerable">State</see>,
    /// <see cref="StateChangeHandler">StateChangeHandler</see>,
    /// <see cref="StateChange">StateChange</see>.
    /// Usage Example:
    /// <code>
    /// internal class Example : IRegisteredScript
    /// {
    ///     private Core core;
    ///     // Interface fields and properties go here.
    ///     // They need private fields to be used, like this:
    ///     private int _id;
    ///     // Then they need to be EXPLICTLY defined, like this:
    ///     int IRegisteredScript.ID 
    ///     {
    ///         get { return _id; }
    ///         set { _id = value; }
    ///     } // For the rest we will assume backing properties exist
    ///     
    ///     internal Example() 
    ///     {
    ///         core = Core.Instance;
    ///         core.Register(this);
    ///         
    ///         _state = StateEnumerable.Running;
    ///         _stateChange.Invoke(new(_state));
    ///     }
    /// }
    /// </code>
    /// </remarks>
    internal interface IRegisteredScript
    {
        /// <summary>
        /// The ID of the script (given out by core)
        /// </summary>
        internal int ID { get; set;  }
        /// <summary>
        /// The current state of the script.
        /// </summary>
        internal StateEnumerable State { get; set; }

        /// <summary>
        /// The method called when a message is sent to the script.
        /// </summary>
        /// <param name="message">Message sent (of any type)</param>
        /// <param name="sender">The sender of the message.</param>
        internal void OnMessage(object message, object sender);

        internal delegate void StateChangeHandler(RSEventArgs args);
        /// <summary>
        /// The event to be invoked on a state change.
        /// </summary>
        internal event StateChangeHandler StateChange;
    }
    /// <summary>
    /// The Existent interface. Applied when a script is not a meta script, and instead exists.
    /// </summary> 
    /// <remarks> Includes
    /// <see cref="RCHResponse">OnRaycastHit</see>,
    /// <see cref="RCHLayerEnumerable">RCHLayer</see>.
    /// Usage Example:
    /// <code>
    /// internal class Example : IRegisteredScript, IExistent
    /// {
    ///     private Core core;
    ///     // IRegisteredScript member implementations.
    ///     // IExistent field and property implementation.
    ///     // Like IRegisteredScript, this is done with backing properties and explicit declarations.
    ///     
    ///     internal Example()
    ///     {
    ///         core = Core.Instance;
    ///     }
    ///     
    ///     RCHResponse IExistent.OnRaycastHit(RCHEventArgs args)
    ///     {
    ///         // Your response compilation logic here.
    ///         return new();
    ///     }
    /// }
    /// </code>
    /// </remarks>
    internal interface IExistent
    {
        /// <summary>
        /// The method to be called on a raycast hit.
        /// </summary>
        /// <param name="args">Event args.</param>
        /// <returns>What should be given to the sender.</returns>
        internal RCHResponse OnRaycastHit(RCHEventArgs args);
        /// <summary>
        /// The Raycast layer the transform is on.
        /// </summary>
        internal RCHLayerEnumerable RCHLayer { get; set; }
    }
    internal enum StateEnumerable
    {
        None,
        Waiting,
        Stopped,
        Ready,
        Running,
        Started,
        Initializing
    }
    internal enum RCHLayerEnumerable
    {
        Hitable,
        Interactable
    }
}