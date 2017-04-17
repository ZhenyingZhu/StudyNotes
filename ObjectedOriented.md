## The Advanced Object-Oriented Technology
https://www.coursera.org/learn/aoo/home/welcome

### Chapter 1
Intro

#### Video 1

#### Video 2
[System definition](https://en.wikipedia.org/wiki/Systems_theory)
-  An organized entity made up of interrelated and interdependent parts

#### Video 3
Structure

#### Video 4
Model

#### Video 5
Engineering Model
- Abstract
- Understandable
- Simulatable
- Precise
- Predictable: can use to predict the behavior of the system
- Cheap
- Realizeable

Cannot be proved. Can only be tested

#### Video 6
Different OOD model:
- Peter Coad
- Grady Booch
- Ivar Jacobson

Workflow
1. Needs model
2. Analysis model
3. Design model

#### Video 7
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

#### Video 8
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

#### Video 9
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

#### Video 10, 11
Why and how to extend UML

BACS(or Backus?) UML Diagram

Class diagram

UML Profile: the extended UML. Can be downloaded from OMG

SysML

#### Video 12, 13
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

#### Video 14
MDA:
- procedural technology: Pascal, C
- object tecnology: Smalltalk, c++
- component technology: packages, framworks, patterns
- model technology: meta-models, xml, uml, MOF, XMI, XSLT

#### Video 15
[Eclipse modeling framework](http://www.eclipse.org/modeling/emf/)

[QVT](http://www.omg.org/spec/QVT/)

[JMF](http://www.oracle.com/technetwork/articles/javase/index-jsp-140239.html)

### Chapter 2

#### Video 1
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

#### Video 2
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

#### Video 3
Modeling process
1. Needs model
  - system border
  - participant
  - use case
2. basic model
  - objects
  - characteristics of objects
  - relation between objects
3. Aid model

#### Video 4
Books OOA and OOD written by Coad and Yourdon

Model-driven architecture (MDA) raised by Object Management Group (OMG)

From Platform-independent model (PIM) to Platform-specific model (PSM)

#### Video 5, 6
Use case analysis: can use a tree to define use cases and their sub use cases. Root is participant
- include relation: the sub use case always happen
- extend relation: not always happen
- generic relation

#### Video 7

