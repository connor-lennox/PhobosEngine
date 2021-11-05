# PhobosEngine
A Component-Architecture game engine written on top of the MonoGame framework.

This project serves primarily as an extension of MonoGame, and provides wrappers around some functionality while also adding some utility for things like Keybindings, Controller inputs, etc.

While MonoGame itself supports 2d and 3d graphics, this engine in particular is geared towards 2d.


## Features

PhobosEngine has all of the features you would need to make (simple) 2d games:

- Camera-based rendering for large scenes
- Transform hierarchies allowing for complex scene objects
- A custom physics implementation, based on an efficient Spatial Hash and 2d Collider system
- Image, Font, and Sound loading
- Text Rendering via (FontStashSharp)[https://github.com/rds1983/FontStashSharp]
- An event-based Input system with optional polling support
- Object-level serialization into JSON format

That being said, there is currently no UI-editor for games. All of the game objects must be made strictly through code (or by loading a serialized scene).
