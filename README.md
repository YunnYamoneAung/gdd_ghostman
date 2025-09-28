## Team Members
Thukha Nyan - 6520076 May Thu Khin - 6611281 Yunn Yamone Aung - 6511158

## About Our Project - Ghostman
A Unity project that flips the classic Pac-Man gameplay:
Instead of playing as Pac-Man, you are the Ghost. Your mission is to catch Pac-Man before he clears the maze!

## Gameplay
You Are the Ghost (Player)
Control with Arrow Keys (↑ ↓ ← →) or (W A S D).
Catch Pac-Man before he eats all pellets.
Pac-Man (AI)
Moves automatically through the maze.
Avoids danger and hunts you when powered up.
Power Pellets
When Pac-Man eats one:
Ghost becomes frightened (slower + vulnerable).
If Pac-Man touches you → ghost respawns in Ghost House.
Timer bar shows how long frightened mode lasts.
Fruit Power-Ups
Occasionally appear in the maze.
Give ghost a temporary speed boost.
Lives
Pac-Man starts with 3 lives.
Each time you catch him → Pac-Man loses 1 life.
Ghost wins if Pac-Man loses all lives.
Win Conditions
Ghost Wins → Pac-Man runs out of lives.
Pac-Man Wins → Pac-Man eats all pellets.

## Tech Stack
Engine: Unity (2021.3 LTS or higher recommended)
Language: C#
Assets: Classic-style sprites & sounds

## How to Run
Clone or download the repo:
Open the project in Unity Hub.
Load the provided Gameplay Scene.
Press ▶️ Play to start.

## Project Structure
PacMan-Ghost-RoleFlip/
 ├── Assets/
 │   ├── Scripts/         # C# gameplay scripts │   ├── Sprites/         # Pac-Man & Ghost sprites │   ├── Prefabs/         # Game object prefabs │   ├── Scenes/          # Gameplay scene │   └── Audio/           # Sound effects & BGM ├── ProjectSettings/
 └── README.md


## Screenshots
![Screenshot 2025-09-28 214503](https://github.com/user-attachments/assets/3a4e214c-64de-4600-ad6c-f44efd04a59b)
![Screenshot 2025-09-28 214541](https://github.com/user-attachments/assets/4c27858a-89bb-41e1-aee6-79b3ca0e6c3e)
![Title](https://github.com/user-attachments/assets/6ee94dea-dbfe-4258-bcf1-947a190c007c)


## Credits
Original Pac-Man by Namco (1980)
Role-flip concept & Unity implementation
Assets adapted for educational/demonstration use
