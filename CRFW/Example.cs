using Framework;
using Framework.Structures;

public class ExampleClass : IRegisteredScript
{
    private Core core;



    public ExampleClass()
    {
        // This is whatever is called on initialization in the game engine. (OnAwake in unity, etc)

        // get core instance
        core = Core.Instance;
        // Register self with core.
        core.Register(this);

        _state = StateEnumerable.Running;
        
    }

    private int _id;
    int IRegisteredScript.ID
    {
        get { return _id; }
        set { _id = value; }
    }

    private StateEnumerable _state;
    StateEnumerable IRegisteredScript.State
    {
        get { return _state; }
        set { _state = value; _stateChange?.Invoke(new(_state)); } 
    }

    private event IRegisteredScript.StateChangeHandler _stateChange = delegate { };
    event IRegisteredScript.StateChangeHandler IRegisteredScript.StateChange
    {
        add { _stateChange += value; }
        remove { _stateChange -= value; }
    }

    void IRegisteredScript.OnMessage(object message, object sender)
    {
        Console.WriteLine(message);
        Console.WriteLine(sender);
    }
}