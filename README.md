# Spaceship Simulator

A simple spaceship flight simulator created for the course
"Vývoj počítačových her pro učitele" (Game Development for Teachers)
at Charles University, Faculty of Mathematics and Physics.

## About

The game simulates realistic spaceship control in a way that stays simple
and approachable. You pilot a mining spaceship whose job is to extract
materials from asteroids.

Your goal is to fill the ship's cargo hold with mined materials —
while keeping the ship in one piece. Space is unforgiving, so
survive and mine!

The game is designed to be played with a controller.

## Controls
   Input (GamePad) | Input (Keyboard + Mouse) | Action |
 |---|---|---|
 | Left Joystick | WSAD | Rotate the ship |
 | Right Joystick | Mouse Movement | Rotate the camera |
 | Right Trigger | Space | Forward thrust |
 | Left Trigger | Shift | Reverse thrust |
 | D-Pad Left | Q | Toggle left engine |
 | D-Pad Right | E | Toggle right engine |
 | A (Cross on PlayStation) | Left Mouse Button | Activate |
 | B (Circle) | Left Mouse Button | Mining |
 | X (Square) | X | Spin brake |

## Instalation

Currently, the game is only playable, if you download the source and launch it yourself.

Many of the source files (textures, meshes and sounds) are very large and require [GitLFS](https://git-lfs.com/) to be pulled properly (only pointers to those files are stored). After installing it, set it up for your user account with
```
git lfs install
```

When installed, you can pull the LFS files using
```
git lfs pull
```
