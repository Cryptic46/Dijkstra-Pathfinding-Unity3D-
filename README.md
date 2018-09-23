# 2D-A-Pathfinding-In-Unity
A 2D implementation for pathfinding in Unity3D.

# 29-08-2018 - Successfully implemented cost search functionality (Dijkstra), with optimal priority queue
Still got a slight issue (possibly bug). Hit play and check Vector(0.5, 1.5, -0.5).

# 31-08-2018 - Agent is able to re-construct path from Destination to Source (agent position).
Bugs have been fixed.
This algorithm is based on One Source, One Destination. Still need to implement Many Sources, One Destination.

# 04-09-2018 - Implemented logic for single destination, many sources.
Will spend the next few days optimizing (where possible) and re-factoring.

# 23-09-2018 - Implemented A* Pathfinding
Project now has an A* implementation (demonstration is located at "A Star"/Scenes/AStar.unity) and Dijkstra (demonstration is located at Dijkstra/Scenes/SampleScene.unity)

# I am currently working on steering and flocking with the A* implementation