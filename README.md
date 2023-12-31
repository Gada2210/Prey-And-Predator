# Prey-And-Predator
**Introduction**

In this project, I have implemented a predator-prey simulation in Unity. The simulation consists of two types of agents: predators and prey. The predators chase the prey, and the prey tries to escape from the predators. The agents have a field of vision and can detect other agents and obstacles within this field. The agents also avoid obstacles by changing their direction of movement when they detect an obstacle in their path.

**Implementation**

The simulation is implemented using a class hierarchy in C#. The Agent class is the base class for both the Predator and Prey classes. The Agent class defines the basic behaviours of an agent, such as moving and checking for other agents or obstacles. The Predator and Prey classes override these behaviours to implement their specific behaviours.
The agents' behaviours are implemented using a finite state machine. The agents have two states: "chasing" and "escaping". The predators are always in the "chasing" state, and they change their direction of movement to chase the nearest prey. The prey is in the "escaping" state when they detect a predator within their field of vision, and they change their direction of movement to escape from the predator. When the prey do not detect any predators, they are in the "chasing" state, and they change their direction of movement to move randomly.
