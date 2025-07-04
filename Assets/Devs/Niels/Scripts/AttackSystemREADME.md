# Attack System Documentation

This attack system provides a flexible and reusable solution for both player and enemy attacks in your Unity project.

## Components Overview

### 1. Attack2.cs

The main attack system component that handles all attack logic.

### 2. PlayerAttackController.cs

Example controller for player attacks with input handling.

### 3. EnemyAttackController.cs

Example AI controller for enemy attacks with automatic targeting.

## Setup Instructions

### For Player:

1. Add the `Attack2` component to your player GameObject
2. **Make sure your player GameObject has the "Player" tag**
3. Configure the Attack2 settings in the inspector
4. The attack system will automatically integrate with your existing `PlayerActionsInput` script

### For Enemies:

1. Add the `Attack2` component to your enemy GameObject
2. Add the `EnemyAttackController` component to your enemy GameObject
3. Configure the Attack2 settings in the inspector
4. The enemy will automatically find the player using the "Player" tag

## Attack2 Component Settings

### Attack Settings

- **Attack Damage**: Amount of damage dealt
- **Attack Range**: Range of the attack
- **Attack Cooldown**: Time between attacks
- **Attack Duration**: How long the attack animation/effect lasts
- **Target Layers**: Which layers can be attacked

### Attack Types

1. **Melee**: Direct hit detection around the attacker
2. **Projectile**: Fires a projectile towards the target
3. **Area**: Area of effect attack around the attack point

### Projectile Settings (for Projectile attacks)

- **Projectile Prefab**: GameObject to spawn as projectile
- **Projectile Speed**: Speed of the projectile
- **Projectile Lifetime**: How long projectile exists

### Area Attack Settings (for Area attacks)

- **Area Radius**: Radius of the area effect
- **Area Effect Prefab**: Visual effect for area attacks

### Animation & Effects

- **Animator**: Reference to animator component
- **Attack Animation Trigger**: Name of animation trigger
- **Attack Sound**: Audio clip to play on attack
- **Hit Effect**: Effect spawned on hit
- **Attack Effect**: Effect spawned when attacking

## Usage Examples

### Basic Attack

```csharp
Attack2 attackSystem = GetComponent<Attack2>();
if (attackSystem.CanAttack)
{
    attackSystem.PerformAttack();
}
```

### Attack Specific Target

```csharp
attackSystem.PerformAttack(targetGameObject);
```

### Attack Towards Position

```csharp
attackSystem.PerformAttack(targetPosition);
```

### Check Attack Status

```csharp
bool canAttack = attackSystem.CanAttack;
bool isAttacking = attackSystem.IsAttacking;
```

### Modify Attack Properties

```csharp
attackSystem.SetAttackDamage(50f);
attackSystem.SetAttackRange(3f);
```

## Integration with Existing Systems

### Health System Integration

The attack system automatically works with your existing `Health` component. When an attack hits a target with a Health component, it will call `TakeDamage()`.

### Input System Integration

To integrate with your existing input system, call the attack methods from your input handlers:

```csharp
public void OnAttack(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        attackSystem.PerformAttack();
    }
}
```

### Enemy AI Integration

For enemies, you can call attacks from your AI state machines:

```csharp
// In enemy AI update
if (playerNearby && attackSystem.CanAttack)
{
    attackSystem.PerformAttack(playerPosition);
}
```

## Events System

The Attack2 component provides Unity Events that you can hook into:

- **OnAttackStart**: Called when attack begins
- **OnAttackEnd**: Called when attack ends
- **OnAttackHit**: Called when attack hits a target (passes the hit GameObject)

You can subscribe to these events in code or assign them in the inspector.

## Tips and Best Practices

1. **Layer Setup**: Set up proper layers for players, enemies, and environment to control what can be attacked
2. **Attack Point**: Create an empty child GameObject as the attack point for better control over attack origin
3. **Animation Integration**: Use animation events to trigger attacks at the right moment in attack animations
4. **Balancing**: Adjust damage, range, and cooldown values for balanced gameplay
5. **Effects**: Use particle systems and audio for better game feel

## Troubleshooting

### Attack Not Working

- Check if CanAttack returns true
- Verify target layers are set correctly
- Make sure Health component is on target objects

### No Damage Dealt

- Ensure target has a Health component
- Check if target layer is included in targetLayers mask
- Verify attackDamage is greater than 0

### Animation Issues

- Assign Animator component reference
- Create attack animation trigger in animator
- Make sure trigger name matches attackAnimationTrigger

## Extending the System

You can easily extend this system by:

1. Adding new attack types to the AttackType enum
2. Creating new attack methods in Attack2
3. Adding new properties for different attack behaviors
4. Creating specialized controllers for different character types
