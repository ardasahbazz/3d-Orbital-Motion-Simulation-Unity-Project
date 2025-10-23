# 3D Orbital Motion Simulation (Unity)

A physics-based 3D simulation built in Unity to model and visualise orbital motion between celestial bodies.  

Created as part of a Computer Science NEA project and designed for use in A-Level Astrophysics lessons.


---

## Overview

This project simulates realistic gravitational interactions between orbiting bodies such as planets, moons, and satellites.  
It uses Unity’s physics system and custom C# scripts to show how Newtonian mechanics work in three dimensions.

The aim is to provide an interactive and educational tool for exploring:
- Orbital mechanics  
- The relationship between mass, distance, and gravitational force  
- Stable and unstable orbits
---

## Images

More images and links to videos can be found in the NEA.pdf document.

<figure>
  <img src="https://github.com/user-attachments/assets/af9daec0-2c43-4c14-9969-96d904c74f0e" width="700" alt="Solar system view">
  <figcaption><b>Figure 1:</b> Close-up of a planet's elliptical orbit and velocity vectors.</figcaption>
</figure>

<figure>
  <img src="https://github.com/user-attachments/assets/a3c04513-1442-4964-98f9-cf67677b6a19" width="700" alt="Planetary orbit detail">
  <figcaption><b>Figure 2:</b> Star constellation overlay enabled.</figcaption>
</figure>

<figure>
  <img src="https://github.com/user-attachments/assets/04ed0afb-a51a-4ec2-85a0-69751ce6aee0" width="700" alt="Constellation view">
  <figcaption><b>Figure 3:</b> Main Menu.</figcaption>
</figure>

<figure>
  <img src="https://github.com/user-attachments/assets/9098651f-3da9-4a06-99dc-fe6b3e735471" width="700" alt="Asteroid belt simulation">
  <figcaption><b>Figure 4:</b> Options Menu.</figcaption>
</figure>

---

## Features

- **Custom gravity calculations:** Computes forces and trajectories between all bodies each frame using Newton’s law of gravitation  
- **3D visualisation:** Real-time rendering with Unity’s lighting, shaders, and camera system  
- **Adjustable parameters:** Mass, velocity, and position of each object can be configured before or during runtime  
- **Camera controls:** Rotate, zoom, and focus on any object  
- **Orbit trails:** Displays motion paths with dynamically scaled thickness for clarity  
- **Asteroid belts:** Procedurally generated inner and outer belts using deterministic seeding  
- **Star field and constellations:** Toggleable constellations and labelled stars for educational context  
- **Time control:** Speed up or slow down time for analysis  
- **Pause and resume:** Step through frames for close inspection  
 

---

## Controls

| Action | Input |
|--------|--------|
| Rotate camera | Right mouse + drag |
| Zoom | Scroll wheel |
| Pan | Middle mouse + drag |
| Focus on object | Left-click |
| Pause / Resume simulation | Spacebar |
| Toggle constellations | Number keys (1–5) |
| Reset camera view | R |
| Increase simulation speed | + |
| Decrease simulation speed | - |

---

## Technical Details

- Developed in **Unity (C#)** using object-oriented design  
- Implements **N-body physics** with double-precision calculations  
- Custom gravitational constant (G) and adjustable time step (Δt)  
- Optimised with pooled objects and lightweight update loops  
- Uses **Trail Renderer** and **Line Renderer** for efficient orbit visualisation  
- Built for flexibility and future feature expansion

---

## Author

**Arda Sahbaz**  
© 2024

