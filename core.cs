using System;
using System.Collections.Generic;
using System.Linq;
using Shift.Structures;

namespace Shift
{
    /// <summary>
    /// The Core class singleton. Used as a central script and manager.
    /// </summary>
    /// <remarks>
    /// The core is sealed by default, making it non-inheritable. It is an internal class, and is non-modifiable. 
    /// However, if you find yourself needing to extend the core's functionality, you can delete the sealed part, which will allow it to be inherited.
    /// Then, you can create a derived class from the core that you can edit.
    /// </remarks>
    internal sealed class Core
    {
        /// <summary>
        /// The <see cref="Core">Core's</see> instance. Please use ONLY this instance.
        /// </summary> 
        /// <remarks>
        /// Usage Example:
        /// <code>
        /// internal class Example : IRegisteredScript
        /// {
        ///     private Core core;
        ///     internal Example()
        ///     {
        ///         core = Core.Instance;
        ///     }
        /// }
        /// </code>
        /// </remarks>
        internal static Core Instance { get { return instance; } }
        /// <summary>
        /// All registered objects & scripts.
        /// </summary>
        internal Dictionary<Type, List<object>> Objects { get { return objects; } }
        /// <summary>
        /// The current Game State.
        /// </summary>
        internal StateEnumerable GameState { get { return gameState; } }

        internal const string GameName = "GameName";
        internal const string GameVersion = "0.0.1.0.0";

        private int NextID = 0;
        private object _key = new();
        private static Core instance = new();
        private StateEnumerable gameState;
        private Dictionary<Type, List<object>> objects = new();

        /// <summary>
        /// !!! NEVER use this. You will no longer be in sync with the rest of the scripts. !!!
        /// </summary>
        internal Core()
        {
            Register(this);
            gameState = StateEnumerable.Running;
        }

        #region Registration, states, and others
        /// <summary>
        /// Registers the given object with the Core.
        /// </summary>
        /// <param name="obj">The object to register.</param>
        internal void Register(object obj)
        {
            Type type = obj.GetType();
            Objects[type].Add(obj);
            if (IsRegisteredScript(obj))
            {
                IRegisteredScript i_obj = obj as IRegisteredScript;
                i_obj.ID = NextID;
                NextID += 1;

                i_obj.StateChange += HandleStateChange;

            }
        }
        /// <summary>
        /// Handles state changes in <see cref="IRegisteredScript"/>s.
        /// </summary>
        /// <param name="args"><see cref="RSEventArgs"/> for the state change.</param>
        internal void HandleStateChange(RSEventArgs args) { }
        /// <summary>
        /// Resets the core's object key. 
        /// </summary>
        internal void ResetKey()
        {
            _key = new();
        }
        #endregion
        #region Checks
        /// <summary>
        /// Checks if the given object is a registered script (<see cref="IRegisteredScript"/>).
        /// </summary>
        /// <param name="obj">Object to check.</param>
        /// <returns></returns>
        internal bool IsRegisteredScript(object obj)
        {
            return obj.GetType().GetInterfaces().Contains(typeof(IRegisteredScript));
        }
        /// <summary>
        /// Checks if the given object is registered with the core.
        /// </summary>
        /// <param name="obj">Object to check.</param>
        /// <returns>True if the object is registered, False if not.</returns>
        internal bool IsRegistered(object obj)
        {
            Type type = obj.GetType();
            if (Objects.ContainsKey(type))
            {
                return Objects[type].Contains(obj);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Verify the Core by the given key.
        /// </summary>
        /// <param name="key">Key to use for verification.</param>
        /// <returns>Verified (True) or unverified (False).</returns>
        internal bool VerfiyCore(object key)
        {
            return key == _key;
        }
        #endregion
        #region Find && Get
        /// <summary>
        /// Finds the object with the given ID.
        /// </summary>
        /// <param name="id">Target ID</param>
        /// <returns>The object if found, null if not.</returns>
        internal object GetObjectById(int id)
        {
            foreach (object obj in Objects.Values)
            {
                if (((IRegisteredScript)obj).ID == id) { return obj; }
            }
            return null;
        }
        /// <summary>
        /// Gets all objects of the type {T}.
        /// </summary>
        /// <typeparam name="T">Type of objects to get.</typeparam>
        /// <returns>A <see cref="List{T}"/> if found, null if not.</returns>
        internal List<T>? GetObjectsByType<T>()
        {
            if (Objects.ContainsKey(typeof(T)))
            {
                return null;
            }
            List<T> output = new();
            foreach (T obj in Objects[typeof(T)])
            {
                output.Add(obj);
            }
            return output;
        }
        #endregion
        #region Communication
        /// <summary>
        /// Send a message to another script.
        /// </summary>
        /// <param name="reciever">The target script.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="sender"><code>this</code></param>
        internal void Send(object reciever, object message, object sender)
        {
            if (IsRegisteredScript(reciever) && IsRegistered(reciever))
            {
                ((IRegisteredScript)reciever).OnMessage(message, sender);
            }
        }
        #endregion
    }
}