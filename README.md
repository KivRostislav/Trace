# Trace

Trace is a lightweight event manager that processes events in a separate thread.

## Installation

Use the package manager [nuget](https://www.nuget.org/) to install trace.

```bash
dotnet add package Trace
```

## Usage

### Event 

To begin with, you need to create a class that implements the IEvent interface, we will use it as an event that we will publish in our program.

```csharp
// CustomEvent.cs
using Trace;

namespace Tutorial;

public class CustomEvent : IEvent { }
```

This class can have any number of fields and methods. But do not abuse it, it can lead to a decrease in the speed of the program.

### Listener

Listeners are needed to handle events in the program. You need to create a listener for each type of event.

To create a listener, you need to create a class that implements interface IEventListener. 

```csharp
// CustomEventListener.cs
using Trace.Events;
using Trace.Listeners;

namespace Tutorial;

public class CustomEventListener : IEventListener
{
    // This field should indicate the type of event handled by the listener
    public Type EventType => typeof(CustomEvent);

    // A method that processes a message, the message is passed through method parameters.
    public Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Hello world!");
        return Task.CompletedTask;
    }

    // This method is called after successful event processing.
    public void OnComplete(IEvent @event) { }

    // This method is called after an unsuccessful event processing.
    public void OnError(Exception exception) { }
}
```

### Configure

After creating the listener and the event, you can proceed to the configuration of our hub.

``` csharp
// Program.cs
using Tutorial;
using Trace.Builders;
using Trace.Hub;

// We create an instance of the builder class.
DefaultEventHubBuilder builder = new();

// We register our listener.
builder.Register(new CustomEventListener());

// Let's build a hub.
IEventHub hub = builder.Build();

// And finally we publish our event.
hub.Publish(new CustomEvent());

Console.ReadLine();
```

After running this code in the console you will see this

```bash
Hello world!
```
## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
