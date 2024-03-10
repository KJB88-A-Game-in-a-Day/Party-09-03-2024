# Party (A-Game-in-a-Day) 09-03-2024 (~11.5 hours)
Theme: Party 09/03/2024 (a Game a Day Challenge).

Bump Party! Bump your friends in competitive party game! (Currently WIP, remote players stay stuck in bumped state due to state not being updated across network).

CONTROLS: WASD to move, Space Bar to dash. 3 lives per player.

CONCEPT: A simple PvP party game where you have to bump your friends and avoid being bumped!

IMPL: Using Mirror for networking, it was quick to get up and running. I used a MessageBroker and FSM to handle behaviours and comms up and down the stack from controller to state. Surprisingly fast go get going!

LESSONS: This one was actually pretty good, despite it not working at the end of the challenge. Mirror is a great technology to use for networking. Very straightforward. This challenge highlighted some technical considerations for working with Mirror.
