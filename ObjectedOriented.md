# Objected Oriented

## Grokking the low level design interview using OOD principles

### Knowledges

OOP principles

- Encapsulation
- Abstration
- Inheritance: inherit all the non-private properties/methods
  - types: single inheritance, multiple inheritance (more than 1 base class), multi-level inheritance, hierarchical inheritance, hybrid inheritance
- Polymorphism: many forms.
  - types: dynamic: method overriding (same name, var, return between two classes), static: method + operator overload(same name different var with in a same class)

UML:

- things
- relationships
- diagrams
  - structural
  - Behavioral: case diagrams, activity diagrams, sequence diagrams

Relationships

1. association
2. generalization: inheritance
3. include
4. extend

Object Association

1. simple association
2. composition
3. aggregation: objects can exist independently

Dependency

Sequence diagram

- lifeline
- activation bars
- messages

Activity diagram

SOLID

- Single Responsibility Principle
- Open/Closed Principle: open for extension but closed for modification. Use inherit not directly modify
- Liskov Substitution princile: superclass should be replaceable by subclasses
- Interface Segregation Principle: fine-grained interfaces. Have group of used interfaces not fat interfaces with useless methods
- Dependency Inversion Principle: high level modules should not dependent on low-level modules. Both should depend on abstrations

Design patterns

- creational
- structual
- behavioral
- achitectrual

Creational design patterns

- factory
- constructor
- singleton
- builder
- prototype
- abstract

structual design patterns

- decroator
- facade
- adapter
- bridge: ?
- composite
- flyweight: use context with cache to share across different compoents
  - intrinsic
  - extrinsic
- proxy

behavioral design pattern

- chain of responsibitlity: a request either handled or passed
- command: ?
- iterator
- mediator: central authority
- observer: notification
- vistor: ?
- interpreter pattern
- template pattern
- memento pattern
- state pattern
- strategy pattern: use interface and override

Architectural design patterns

- MVC
- MVP
- MVVM

Real problem

1. Collecting requirements
2. scope down the problem
3. SOLID: easy to extend

Model the problem: establish the classes and their relationships

### Parking lot

1. payment flexibility
2. parking spot type
3. vehicle types
4. pricing

Requirements

1. park 40k cars
2. types of spots: hanicapped, compact, large, motorcycle
3. multiple entrance and exit
4. types of cars: car, truck, van, motorcycle
5. a display board shows free parking spots
6. customers collect a ticket from the entrance, pay at exit
7. pay with either automated exit panel or pay the parking agent at exit
8. payment is calculated at an hourly rate.
9. payment made using either a credit/debit card or cash

Actors

1. customer: take/scan/pay ticket
2. parking agent
3. admin: add/remove/update a spot, agent, entry/exit panels. Modify hourly rate, view/update account (customer)
4. system: giving details of parking spot. assigning parking spots to vehicles

### Elevator System

Consideration

1. how many elevators
2. wait time and running cost
3. how passengers see the status of elevator

Design pattern

1. strategy
2. state
3. delegation

Requirements

1. 15 floors, 3 elevators
2. move up or down or in idle state
3. door open in idle state
4. elevator passes through each floor
5. a button to call elevator car, up and down
6. panel inside elevator have buttons to go to every floor, and open/close doors
7. display in and out shows the current floor and the direction
8. inside display shows capacity

Actors

1. passengers
2. system

Use cases

1. press panel button
2. move/stop elevator
3. dispatcher algorithm
4. display
5. open/close door
6. request for elevator
7. fllor request
8. call emergency

Dispatching algorithms

1. First Come First Serve: 4 states: elevator idle; elevator moving towards passenger and same direction; moving towards and wrong direction; moving away. Use a queue to track which passenger comes first
2. Shortest Seek Time First: passenger who is closest to the elevator gets it
3. SCAN: always go up to the top then go down to bottom
4. LOOK: advanced SCAN

### Library management system

Use cases

1. book info: # of copies of a book, position, name + attributes
2. renewal extend borrow
3. fine
4. book reservation
5. barcode scanner

Design pattern

- facotry
- delegation: delegate a task from one class to another
- observer: notify after a book is available

### Amazon locker service

Considerations

1. Locker size
2. Locker selection: not double booked
3. Lock status: time constraint for package kept in the locker
4. return an item using a locker
5. code

Design patterns

1. strategy
2. repository

actors

1. customer
2. delivery guy
3. system

### Vending Machine

Points

1. states of the vending machine: no money state, money inserted state, dispense state
2. receive, caculate, return money
3. racks

Design patterns

1. state

Actors

1. customer
2. operator: collect cash
3. system

### Blackjack game

rules

1. A is 1 or 11
2. 10, J, Q, K = 10
3. hit or pat
4. blackjack: A + 10

Points

1. how many players?
2. can players play against each other?
3. how many points can the player and the dealer hit the card
4. what if dealer and player gets same points
5. how many cards at max player can take

Design patterns

1. iterator
2. state

Actors

1. player: resigns a game
2. dealer: block member

Classes

1. card
2. deck
3. Shoe: shuffle
4. hand: get score
5. player: player + dealer
6. controller

### Meeting scheduler

Expectations

1. room + capacity
2. book room
3. attendees' availability
4. meeting notification to the invitee
5. accept or reject the invite
6. a calendar

pattern

1. singleton

actors

1. scheduler
2. user
3. system

### Movie Ticket Booking

Expectations

1. seat selection: do not double booked
2. seat reservation timeout? First come first serve?
3. Price of the booking: seat type? discount?
4. multiple cinemas with multiple halls? same movie at different time?
5. search movie
6. users can select multiple searts
7. notification

Design pattern

1. strategy

Actors

1. Systems
2. Customer
3. ticket agent

### Car rental

Expectations

1. vehicle type
2. search by name + location
3. services: assign a driver, roadside assistant
4. reservation + cancellation
5. payment + fine with notification
6. vehicle logs
7. multiple branches + parking stalls

design pattern

1. decorate: discount, peak season, damaged fine, partially filled fuel tank fine

Actors

1. member
2. receptionist
3. worker
4. system

### ATM system

Design patterns

1. singleton
2. state

Requirements

1. get account while insert card
2. card reader, keypad, screen, cash dispenser, printer, network infrastructure
3. PIN auth
4. checking + saving
5. cancel transaction before confirmation
6. withdrawal limit

### Chess game

Requirements

1. King, queen, rooks, knights, bishops, pawns
2. player takes first turn
3. undo

Design pattern

1. singleton
2. command

Objects

1. box: piece, x, y
2. Piece

### Hotel management

Requirements

1. book room: not double booked
2. search room
3. payment: in advance, or JIT
4. price: room size, time, duration
5. booking cancelation
6. housekeeper, receiptionist, guest, server
7. standard, deluxe, family suite, business suite
8. room service
9. return key

Design patterm

1. strategy
2. singleton
3. factory

## Amazon online shopping

Requirements

1. Discoverability
2. cart and checkout
3. payment
4. product reviews and ratings
5. guest user can view
6. authN user can buy, sell
7. shipping: address, tracking
8. order can be cancelled before it is shipped
9. notification
10. order log
11. coupon

Design pattern

1. factory
2. strategy

### Stack overflow

Requirements

1. question: view and search, post and answer
2. comment on questions and answers
3. reputation
4. voting
5. bounty
6. notification

Design pattern

1. observer

### Restaurant management

Requirements

1. branch
2. cerate order
3. menu item
4. available table
5. table reservation
6. notification
7. pay bills
8. different table configs

Design pattern

1. singleton
2. factory

### Facebook

Requirements

1. privacy
2. profile
3. group
4. search for users, groups, pages
5. write a new post
6. friend request
7. like, share post
8. notification
9. alert
10. send + receive message

Design pattern

1. observation

### Online stock brokeerage system

Requirements

1. discoverability: search for stock
2. visibility
3. Multiplicity
4. trade in
5. watchlist
6. different lots of the same stock
7. notify user when a trade order is carried out
8. trade types: market, limit, stop-loss, stop-limit

Design pattern

1. singleton
2. observer

### Jigsaw puzzle

Requirements

1. Puzzle board
2. puzzle pieces: corner vs. edge vs. middle pieces
3. puzzle solver: check if two pieces can matching up
4. rotate piece

Design pattern

1. singleton

### Airline management

Requirements

1. search flight
2. flight reservation: multiple flights
3. payment
4. book
5. flight cancellation
6. seat
7. airports
8. pilots and crew members
9. itineraries
10. notification

Design pattern

1. singleton
2. observer

### Cricinfo

- players, coaches, teams
- stats
- tournament
- matches
- points table
- stadium

### LinkedIn

Requirements

1. profile
2. serach for a user/page
3. connection
4. follow
5. stats: # of connections, profile views, post impressions, search appearance
6. recommendations to other users
7. write a new post
8. react, share, comment on a post
9. message
10. notification
11. company page
12. open jobs
13. groups
14. follow vs. connect
15. apply job

## The Advanced Object-Oriented Technology

<https://www.coursera.org/learn/aoo/home/welcome>

### Chapter 1

Intro

#### Chapter 1 Video 1

#### Chapter 1 Video 2

[System definition](https://en.wikipedia.org/wiki/Systems_theory)

- An organized entity made up of interrelated and interdependent parts

#### Chapter 1 Video 3

Structure

#### Chapter 1 Video 4

Model

#### Chapter 1 Video 5

Engineering Model

- Abstract
- Understandable
- Simulatable
- Precise
- Predictable: can use to predict the behavior of the system
- Cheap
- Realizeable

Cannot be proved. Can only be tested

#### Chapter 1 Video 6

Different OOD model:

- Peter Coad
- Grady Booch
- Ivar Jacobson

Workflow

1. Needs model
2. Analysis model
3. Design model

#### Chapter 1 Video 7

Method = model + process

UML

Language:

- Syntax
- Semantics
- Pragmatics

Difference

- Concrect Syntax: use graph or xml to represent a model
- Abstract Syntax: use Class, Association to reprensent a model
- Semantics: the real meaning

#### Chapter 1 Video 8

Methods:

- Structure: Data flow + preceeding
- E-R: Relation between static entities
- Finite Status Machine: event driven. Good for concurent, or complicate behavior
- Rules: Good for game theory
  - uniform game tree
  - Backtracking
- formalize: Flowchart
  - Hoare logic
  - petri net
  - Process Algebra

#### Chapter 1 Video 9

OOD design paradigm

- Real world
- Needs analysis
- System Analysis
- System Design
- OO languange
- non-OO language
- Assembly language
- Computer

Principles [src](http://bijian1013.iteye.com/blog/2282565)

- Encapsulation
- Message passing
- Delegation
- Dynamic binding
- Instantiation
- Generic and polymorphism
- Relation

Aspect-oriented programming

- Aspect
- Join point
- Advice
- Pointcut
- Introduction
- Target object
- Weaving

Service-Oreinted Architecture，Component-Based Development

#### Chapter 1 Video 10, 11

Why and how to extend UML

BACS(or Backus?) UML Diagram

Class diagram

UML Profile: the extended UML. Can be downloaded from OMG

SysML

#### Chapter 1 Video 12, 13

Meta: control, describe

Backus Naur Form: `::= |`

Meta model: 4 layers

- M0: Objects
- M1: Model
- M2: Meta-Model

UML OCL: to describe classes with context relation

UML extension method:

- UML 1.x
  - Stereotypes
  - Constraints
  - Tagged Values
- UML 2.0
  - Profile
  - Meta model

#### Chapter 1 Video 14

MDA:

- procedural technology: Pascal, C
- object tecnology: Smalltalk, c++
- component technology: packages, framworks, patterns
- model technology: meta-models, xml, uml, MOF, XMI, XSLT

#### Chapter 1 Video 15

[Eclipse modeling framework](http://www.eclipse.org/modeling/emf/)

[QVT](http://www.omg.org/spec/QVT/)

[JMF](http://www.oracle.com/technetwork/articles/javase/index-jsp-140239.html)

### Chapter 2

#### Chapter 2 Video 1

Modeling element

- Class
- Relation
- Properity
- Operation
- Inherit
- Aggration
- Message

Principle

- Abstract
- Classify
- Encapsulation
- Inherit
- Aggration
- Relation
- Message communication: objects can only use message to complicate with other objects
- Grading control: package
- Behavior analysis: all behaviors belones to some class

#### Chapter 2 Video 2

OOA: analysis

- Needs model
- Basic model: classes
  - Object layer
  - Characteristic layer
  - Relation layer
- Aid model
  - package graph
  - order graph
  - active graph
  - FSM

OOD: design

- problem domain
- UI
- Data API: persist
- Control driver

#### Chapter 2 Video 3

Modeling process

- Needs model
  - system border
  - participant
  - use case
- basic model
  - objects
  - characteristics of objects
  - relation between objects
- Aid model

#### Chapter 2 Video 4

Books OOA and OOD written by Coad and Yourdon

Model-driven architecture (MDA) raised by Object Management Group (OMG)

From Platform-independent model (PIM) to Platform-specific model (PSM)

#### Chapter 2 Video 5, 6

Use case analysis: can use a tree to define use cases and their sub use cases. Root is participant

- include relation: the sub use case always happen
- extend relation: not always happen
- generic relation

#### Chapter 2 Video 7, 8, 9

System duty vs problem domain

Relation includes

- aggration relation
- entirety-partial relation

Diagrams

- A class diagram
- A Sequence diagram
- An activation diagram

Sync or Async message

#### Chapter 2 Video 10, 11, Video 12

UI and Control driver design

- Identify passive class(send out message and control other classes)
- use process or thread

Control flow

I/O intensive vs CPU intensive

### Chapter 3

#### Chapter 3 Video 1, 2, 3

Object vs Subject

Object is an instance of a Class

Properties and Methods

#### Chapter 3 Video 4

Object based: cannot derive new classes

- Javascript
- OCL

#### Chapter 3 Video 5

encapsulate

- combine properties and operations together
- avoid outside objects directly modify inner properties
- information hidden: make static, unchangable services public (public, private, protected, friend)

Process oriented: only operations

ER diagram: only properties

#### Chapter 3 Video 6

Instantiation

Abstract Data Type (ADT): used by Ada and CLU

#### Chapter 3 Video 7, 8

Message passing

- The explaination of the message is owned by receiver
- function override, polymorphism, code reuse all need message passing mechanism

There is a delay to pass message to another object

#### Chapter 3 Video 9, 10, 11

generics and polymorphism

Generic

- inherit

polymorphism

- override
- interface reuse

override: replace an implementation with specific cases; is polymorphism, use dynamic link. Use virtual table to implement. Expensive

overload: different functions with a same name; not polymorphism, use static link

overwrite: put something else in the place, like rewrite a file. not related here.

function signature vs prototype

#### Chapter 3 Video 12

Association and Aggration

Association

- The relationship between objects
- Dependency relation is not association

Aggration

- Entire-partial relation

### Chapter 4

#### Chapter 4 Video 1, 2

Book chapter 2

Function decomposition methods

- Structure design: data flow diagram
- Information modeling: E-R diagram. Entity and relation. Both of them are defined by properties

#### Chapter 4 Video 3

Problem domain

System responsibilities

#### Chapter 4 Video 4, 5, 6, 7

OOD methods

- Booch
- Coad-Yourdon
- Firesmith
- Jacobson (OOSE)
- Martin-Odell
- Rumbaugh (OMT)
- Seidewitz-Stark
- Shlaer-Mellor
- Wirfs-Borck

Booch

- Class diagram
- Object diagram
- Module diagram
- Process diagram
- State transfer diagram
- Interactive diagram

Coad Yourdon 5 layers

- Subject layer
- Class and object layer
- Structure layer
- Property layer
- Service layer

4 components

- Human interface component
- Problem domain component
- Task management component
- Data management component

Jacobson focus on use case

3 objects

- entity object
- interface object
- control object

Rumbaugh 3 model

- function model
- Object model
- Dynamic model

#### Chapter 4 Video 8, 9, 10, 11

Book Chapter 3

specification vs standard

UML1 parts

- Summary
- Semantics
- Notation guide
- Example profiles
- model interchange
- Object constranit language

4 layers

- Meta-metamodel
- Metamodel
- Model
- User object

#### Chapter 4 Video 12

Static Structure Diagram

- Class Diagram
- Object Diagram
- Use case Diagram

Interaction Diagram

- Sequence Diagram
- Collaboration Diagram
- State chart Diagram
- Activity Diagram

Implementation Diagram

- Component Diagram
- Deployment Diagram

Extension mechanism

- Constraint
- Comment
- Tagged value
- Stereotype

#### Chapter 4 Video 13, 14, 15

UML2 models

Diagram

- Structure
  - Class
- Composite Structure
- Component
- Deployment
- Object
- Package
- Behavior
  - Actuvity
- Interaction
  - Sequence
  - Communication
  - Interaction Overview
  - Timeing
- Use case
- State machine

#### Chapter 4 Video 16

### Chapter 5

#### Chapter 5 Video 1

use case diagram

1. Define border of the system: how outer objects can interact with the system, don't think about how system provide those business functions
2. Find actors
3. Find usecase

From actor point of view

#### Chapter 5 Video 2, 3, 4, 5, 6

- Actor send request to system
- system ask actor to provide services

Actors can have generic relation between actors

Strategy to identify actors

- The human class that directly interact with the system
- Outer systems
- Devices, if the system we develop needs to deal with them
- External events, e.g. time event, that needs the system to responce

Use case: the description of how the actors use one function of the system. Is a special sequence of transaction

Actors can have association relation with use cases

One use case can associate with two actors. It doesn't mean two actors have generic relation. It means two actors both involve in this use case.

**Extend**: relation between use case and use case. A points to B means A is a generic use case, while B is some special cases of A. B has extension point to describe the special condition of it.

**Include**: Use case A points to B means A include the behavior of use case B.

**Generic**: The sub use case can overload some behavior of super use case

#### Chapter 5 Video 7

Use cases include both success cases and failure cases

Use case needs to describe 1. what are actors' cases, 2. what are system cases

#### Chapter 5 Video 8, 9

Group use cases into packages

Extreme programming (XP): One of the agile software development

#### Chapter 5 Video 10

eclipse GMF

### Chapter 6

#### Chapter 6 Video 1

Object vs Class

active object

Domain model diagram

Diagram of Implementation Classes

Object Diagram

#### Chapter 6 Video 2, 3

CRC card to find system responsibilities

- each card is a class
- first define responsibilities of a class
- if those responsibities cannot be done by this class only, create a new card as helper

Review and discard some useless objects

Properties of a class cannot relate to another class. Create a relation class.

#### Chapter 6 Video 4

**Generization**: is a

Power type & stereotype

#### Chapter 6 Video 5

**Association**: zero or one-to-zero or one, one-to-one, one-to-many, many to many(try to avoid many-to-many, use one-to-one instead)

**Direct Association**: From A can access B but from B cannot access A

#### Chapter 6 Video 6

If an association has properties, can use a class to describe it

Extended association types

- Qualifier: avoid many-to-many relation. break it into two one-to-many relation
- XOR: either relate to this or that
- Ordered

Two ways to implement association

- message passing
- database store args

#### Chapter 6 Video 7

**Analysis pattern** (Martin Fowler)

- Accountability: Party, organization hierarchies, Organization structure
- Observation and measurements
- Rederring to objects

#### Chapter 6 Video 8

**Aggregation**: has a

**Composition**: one of aggregation. All composites combine to be an entire part.

There are some case which can both use aggregation and generization. If one instance of the subclass can change into another class, use aggregation is better.

#### Chapter 6 Video 9

**Dependency**: all other relations

- access: message passing
- derive
- import
- refine
- trace
- use
- include
- extend

**Interface realization**:

Iterfaces between

- processes: OCL
- languages: IDL
- hosts: ejb, com, dcom

#### Chapter 6 Video 10

package diagram

import vs access

visibility:

- `+`: public
- `#`: protected
- `-`: private
- `~`: only elements in the same package can access

### Chapter 7

#### Chapter 7 Video 1

Interaction diagram

Intro

- good at describe control flow and data flow
- good at describe static activity
- not good at describe event driven, dynamic activity

Diagrams

- Sequence diagram
- Communication diagram
- ?
- ?

#### Chapter 7 Video 2

Sequence diagram

- object
- lifecycle line: contains both active state and hibernate state of an object
- activate
- message

Message

- Syncronized message: block the sender thread until get response
- Async message: in buffer
- return message

#### Chapter 7 Video 3

Inorder system

- Messages are sync messages
- each message only have one receiver

Concurrent system

#### Chapter 7 Video 4, 5

Async message: To receiver, the message is an event

UML define this kind of message as signal, which only has properties, but not methods

UML 2.0

- message occurrence
- execution occurrence
- interaction fragment
- combined fragment
- formal gates
- actual gates
- expression gates

#### Chapter 7 Video 6

- Communication diagram
- Interaction abstract diagram
- Timing diagram

### Chapter 8

#### Chapter 8 Video 1

State chart diagram

- State
- event
- action
- state transfer

Events

- interaction
- timing
- entity value changing

#### Chapter 8 Video 2

Represent a state:

- state name
- a pair of event and action

Pseudo state

Final state

#### Chapter 8 Video 3

Action vs event

#### Chapter 8 Video 4

Pseudo state

- Junction
- Choice

#### Chapter 8 Video 5

How to draw state flow chart

#### Chapter 8 Video 6

How to convert state flow chart to C++

#### Chapter 8 Video 7, 8

Advanced status

- Composition status
- Historical status
- Concurrent status: Orthogonality

#### Chapter 8 Video 9

Examples:

- TCP
- Elevator

### Chapter 9

## Defination

coarse-grained, fine-grained [Granularity](https://en.wikipedia.org/wiki/Granularity)

[SOLID](https://en.wikipedia.org/wiki/SOLID_(object-oriented_design))
