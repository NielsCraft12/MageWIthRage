# Super Simple Attack System Setup Guide

This is the simplest possible attack system for your Unity game. Follow these steps:

## PLAYER SETUP

1. **Add the attack script to your player:**

   - Select your Player GameObject
   - Add Component > Super Simple Attack
   - Set the Player Damage (default: 50)
   - Set the Attack Range (default: 2)
   - Drag your player's Animator to the Player Animator field

2. **Set up attack animation events:**
   - Open your player attack animation in the Animation window
   - Add an Animation Event where the attack should deal damage (usually mid-animation)
   - Set the Function to: `DealDamage`
   - Add another Animation Event at the end of the attack animation
   - Set the Function to: `EndAttack`

## SLIME SETUP

1. **Add the slime attack script to each slime:**

   - Select your Slime GameObject
   - Add Component > Super Simple Slime Attack
   - Set the Slime Damage (default: 10)
   - Set the Attack Cooldown (default: 2 seconds)
   - Drag the slime's Animator to the Slime Animator field
   - Set the Attack Trigger name (default: "Attack")

2. **Set up slime collision:**

   - Make sure your slime has a Collider component
   - Set the Collider as a Trigger if you want touch-to-attack
   - OR leave it as a normal collider for collision-based attacks

3. **Set up slime animation events:**

   - Open your slime attack animation in the Animation window
   - Add an Animation Event where the slime should deal damage
   - Set the Function to: `DamagePlayer`

4. **Make sure your slime has the Health component:**
   - Each slime needs the existing Health script attached
   - Set Max Health, Current Health, and Damage Amount

## TAGS SETUP

1. **Set up tags:**
   - Make sure your Player GameObject has the tag "Player"
   - Make sure your Slime GameObjects have the tag "Enemy" (or the script will detect Slime component)

## HOW IT WORKS

**Player Attack:**

1. Player presses attack button (uses your existing input system)
2. Attack animation plays
3. During animation, `DealDamage()` is called by animation event
4. Script finds all enemies within attack range and damages them
5. `EndAttack()` is called at animation end to allow attacking again

**Slime Attack:**

1. Slime touches or collides with player
2. Attack animation plays (if cooldown allows)
3. During animation, `DamagePlayer()` is called by animation event
4. Player takes damage
5. Slime goes on cooldown

## IMPORTANT NOTES

- This uses your existing PlayerActionsInput system
- This uses your existing Health system
- This uses your existing animation system
- Just add the animation events and you're done!
- Adjust damage values and ranges in the inspector
- The red wireframe sphere shows the attack range when you select the player

## DEBUGGING

- Check the console for debug messages when attacking
- Make sure animation events are properly set up
- Verify that tags are correct ("Player" and "Enemy")
- Ensure all required components are attached (Health, Animator, etc.)

That's it! The simplest attack system possible!
