using Framework;
using Framework.Structures;

public class Example : IRegisteredScript, IExistent
{
    public int ID { get; set; } = -1;   // Set the ID to -1 as a placeholder (from IRegisteredScript)
    public StateEnumerable State { get; set; } = StateEnumerable.None;  // Set the state to None as a placeholder (from IRegisteredScript)
    public event IRegisteredScript.StateChangeHandler StateChange = delegate { };   // Create the state change event (from IRegisteredScript)

    public double X { get; set; } = 0;  // Set the X to 0 as a placeholder (from IExistent)
    public double Y { get; set; } = 0;  // Set the Y to 0 as a placeholder (from IExistent) 
    public RCHLayerEnumerable RCHLayer { get; set; } = RCHLayerEnumerable.None; // Set the layer to none as a placeholder  (from IExistent)

    private Core core = Core.Instance; // The core instance
    private bool expectingResponse; // Wether to expect a response or not
    private Exception? currentException; // The current exception that's being handled

    public Example() // Or wherever your class is initialized
    {
        core.Register(this); // Register 'this'
    }

    public void OnMessage(Message message) // Action to be performed on a message
    {
        var (sender, reciever, content, type) = message; // Deconstruct the message

        Message output = new Message(new object(), MessageType.None, this, sender, true); // Prepare the output
        switch (type) // Switch 'type'
        {
            case MessageType.None:
                output.Type = MessageType.What; // Ask 'what?'
                break;
            case MessageType.ErrorQuery:
                if (currentException == null)
                {
                    output.Type = MessageType.What; // There isn't any exception, what do they want?
                } else
                {
                    output.Content = currentException; // Give the exception 
                    output.Type = MessageType.Response; // It's a response type (gives back data)
                }
                break;
            case MessageType.Response:
                if (expectingResponse)
                {
                    break; // Handle the data
                }
                break;
        }
        if (message.IsResponse) { /* also handle responses here */}
        core.Send(output); // send the output
    }
    public RCHResponse OnRaycastHit(RCHEventArgs args) // Action to be performed on a raycast hit
    {
        RCHResponse res = new(); // Generic handling for this
        return res; // return it
    }
}