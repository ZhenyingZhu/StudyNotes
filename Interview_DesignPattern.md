# Design pattern

[Intro](https://en.wikipedia.org/wiki/Software_design_pattern)

## Creational patterns

### Abstract factory

### Builder

### Dependency Injection

### Factory method

### Lazy initialization

### Multiton

### Object pool

### Prototype

### Resource acquisition is initialization (RAII)

### Singleton

## Structural patterns

### Adapter, Wrapper, or Translator

### Bridge

### Composite

### Decorator

### Delegation

### Extension object

### Facade

### Flyweight

### Front controller

### Marker

### Module

### Proxy

### Twin


### Messaging pattern

Use a communication protocol to establish a communication channel between two different parts of a message passing system

- reuqest-response pattern
- one-way pattern

SOAP(Simple Object Access protocol)

### Observer pattern

- subject maintains a list of dependents/observers
- subject notifies observers when state changes
- used to implement distributed event handling system
- a key part in the familiar MVC(model-view-controller) architectural pattern

### Pub/sub pattern

[src](https://en.wikipedia.org/wiki/Publish%E2%80%93subscribe_pattern)

A messaging pattern

publisher always send all classes of messages. subscriber only receive classes it interested to by filtering.

filtering:

- topic-based: published to logical channels. Publisher define classes
- content-based: subscriber classifying messages.
- hybrid of two: publisher post messages to a topic, subscribers register content-based subscriptions to one or more topics

topologies

- publisher post messages to an intermediary message broker or event bus
- subscribers register subscriptions with the broker
- broker perform the filtering. It have a store and forward function and prioritize messages

#### Event bus

One of publish/subscribe pattern.

[src](http://timnew.me/blog/2014/12/06/typical-eventbus-design-patterns/)
enable message to be delivered between components without requiring the components to register itself to others.

### Producer-consumer

[Producer-consumer problem in Python](http://agiliq.com/blog/2013/10/producer-consumer-problem-in-python/)

consumer:

- acquire condition
- if queue empty, condition wait, which release the lock in the condition, and block waiting. Else goto get item
- get condition notification, wake up
- when the condition is released by producer, continue
- get item
- release the condition

producer:

- acquire condition
- put item in the queue
- notify condition
- release condition

If need put a max size on the queue, make producer wait until consumer notify on the same condition

### Adapter pattern

[src](https://en.wikipedia.org/wiki/Adapter_pattern) Wrapper

Client contains an Adaptor. Adaptor contains a list of Adaptee. When Client wants to call a method of Adaptee but APIs are not same, it call Adaptor method which call Adaptee method.

### Command pattern

[src](https://en.wikipedia.org/wiki/Command_pattern)

- Invoker/executor: doesn't know concrete command, but Command interface
- Client: hold invoker and concrete commands objects
- Receiver
- Command interface: have ref to a receiver
- Concrete commands implement Command
