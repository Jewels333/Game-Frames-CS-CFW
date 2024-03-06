using System;

namespace Framework.Structures
{
    /// <summary>
    /// Game State Change event args.
    /// </summary>
    internal class GSCEventArgs : EventArgs
    {
        public GSCEventArgs() { }
    }
    /// <summary>
    /// Registered Script event args. (Used in <see cref="IRegisteredScript"/>)
    /// </summary>
    public class RSEventArgs : EventArgs
    {
        public StateEnumerable State;
        public RSEventArgs(StateEnumerable state)
        {
            State = state;
        }
    }
    public class RCHEventArgs { }
    public class RCHResponse { }
    /// <summary>
    /// The Registered Script interface.
    /// </summary>
    /// <remarks>Includes 
    /// <see cref="int">ID</see>, 
    /// <see cref="StateEnumerable">State</see>,
    /// <see cref="StateChangeHandler">StateChangeHandler</see>,
    /// <see cref="StateChange">StateChange</see>.
    /// </remarks>
    public interface IRegisteredScript
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
        internal void OnMessage(Message message);

        public delegate void StateChangeHandler(RSEventArgs args);
        /// <summary>
        /// The event to be invoked on a state change.
        /// </summary>
        public event StateChangeHandler StateChange;
    }
    /// <summary>
    /// The Existent interface. Applied when a script is not a meta script, and instead exists.
    /// </summary> 
    /// <remarks> Includes
    /// <see cref="RCHResponse">OnRaycastHit</see>,
    /// <see cref="RCHLayerEnumerable">RCHLayer</see>.
    /// </remarks>
    public interface IExistent
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

        internal double X { get; set; }
        internal double Y { get; set; }
    }
    public class UnregisteredScriptException : Exception { }

    public class Message
    {
        public IRegisteredScript Sender;
        public IRegisteredScript Reciever;
        public object Content;
        public MessageType Type;

        public bool IsResponse;

        public Message(object content, MessageType type, IRegisteredScript sender, IRegisteredScript reciever, bool isResponse = false)
        {
            Content = content;
            Type = type;
            Sender = sender;
            Reciever = reciever;
            IsResponse = isResponse;
        }

        public void Deconstruct(out IRegisteredScript sender, out IRegisteredScript reciever, out object content, out MessageType type)
        {
            sender = Sender;
            reciever = Reciever;
            content = Content;
            type = Type;
        }
    }

    public enum MessageType
    {
        ErrorQuery, None, What,
        Response
    }
    public enum StateEnumerable
    {
        None,
        Waiting,
        Stopped,
        Ready,
        Running,
        Started,
        Initializing,
        Errored
    }
    public enum RCHLayerEnumerable
    {
        Hitable,
        Interactable,
        None
    }
}