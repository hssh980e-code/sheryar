# 3D Game Project - Core Mechanics

A Unity 3D game project with essential core mechanics for gameplay development.

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   └── PlayerController.cs      # Player movement and input
│   ├── Camera/
│   │   └── PlayerCamera.cs          # First-person camera control
│   ├── Combat/
│   │   ├── Health.cs                # Health/damage system
│   │   └── Weapon.cs                # Weapon and attack system
│   ├── AI/
│   │   └── SimpleEnemy.cs           # Enemy AI with patrol/chase
│   ├── UI/
│   │   └── HealthUI.cs              # Health bar UI
│   └── GameManager.cs               # Game state management
```

## 🎮 Core Mechanics Included

### 1. **PlayerController**
- WASD movement with smooth acceleration
- Left Shift to sprint
- Space to jump
- Ground detection system
- Smooth velocity limiting

**Setup:**
- Attach to Player GameObject
- Assign Ground Layer in Inspector
- Adjust walkSpeed, sprintSpeed, jumpForce as needed

### 2. **PlayerCamera**
- First-person mouse look
- Vertical look clamping (prevents over-rotation)
- Cursor locking (ESC to toggle)
- Configurable mouse sensitivity

**Setup:**
- Create a Camera child object under Player
- Attach this script to the Camera
- Adjust mouseSensitivity value

### 3. **Health System**
- Track player/enemy health
- Damage and healing methods
- Death event system
- Health percentage calculation

**Setup:**
- Attach to any GameObject that needs health
- Add as event listener for UI updates

### 4. **Weapon System**
- Raycast-based attacks
- Attack cooldown management
- Damage dealing
- Fire point customization

**Setup:**
- Attach to weapon GameObject or as child of player
- Assign hit layers for detection
- Call `Attack()` from input handler

### 5. **SimpleEnemy AI**
- Patrol waypoint system
- Player detection and chase
- Attack behavior with cooldown
- Gizmo visualization

**Setup:**
- Create Enemy GameObject with Rigidbody
- Attach Health and SimpleEnemy scripts
- Set patrol points in Inspector
- Adjust detection and chase speeds

### 6. **HealthUI**
- Real-time health bar display
- Color gradient (green → red)
- Health text display
- Event-driven updates

**Setup:**
- Create Canvas with Image for health bar
- Add TextMeshPro for health text
- Attach script to UI parent object

### 7. **GameManager**
- Singleton pattern for global access
- Pause/resume functionality
- Difficulty settings

**Setup:**
- Add single GameManager to scene
- Access via `GameManager.Instance`

## 🛠️ Quick Setup Guide

1. **Create Player:**
   - GameObject → 3D Object → Capsule (name: Player)
   - Add Rigidbody (Body Type: Dynamic, Freeze Rotation: X, Y, Z)
   - Add PlayerController script
   - Create Camera child object
   - Add PlayerCamera script to Camera

2. **Create Enemy:**
   - Duplicate Player or create new Capsule (name: Enemy)
   - Add Health script
   - Add SimpleEnemy script
   - Create patrol points (empty GameObjects)
   - Assign patrol points to SimpleEnemy

3. **Create UI:**
   - GameObject → UI → Panel
   - Add Image child (health bar)
   - Add TextMeshPro child (health text)
   - Attach HealthUI script

4. **Setup Layers:**
   - Create "Ground" layer for floors
   - Create "Enemy" layer
   - Update player controller and weapon with correct layers

## ⚙️ Input System

- **W/A/S/D** - Move
- **Mouse** - Look around
- **Space** - Jump
- **Left Shift** - Sprint
- **ESC** - Toggle cursor lock
- **P** - Pause game
- **Left Mouse** - Attack (implement in your PlayerInput handler)

## 🎯 Next Steps

- Create PlayerInput handler to manage weapon attacks
- Add animation system (Animator)
- Implement item/pickup system
- Add sound effects and music
- Create level design with obstacles
- Add more enemy types and behaviors
- Implement inventory/HUD system

## 📝 Notes

- All scripts use well-commented code for learning
- Physics-based movement for smooth gameplay
- Event system for UI and game state communication
- Easily expandable architecture for new features

---

**Created for 3D game development in Unity with C#**
