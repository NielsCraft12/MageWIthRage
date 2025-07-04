# Integrated Attack System Setup Guide

Your attack system is now fully integrated into your existing `PlayerActionsInput` script! Here's how to set it up:

## SETUP STEPS

### 1. Player Setup (Already Done in Code!)

✅ The attack system is now built into your `PlayerActionsInput` script
✅ It uses your existing input system automatically
✅ No extra components needed on the player!

### 2. Configure Attack Settings in Inspector

1. Select your Player GameObject
2. Find the "PlayerActionsInput" component
3. In the "Attack System" section, set:
   - **Attack Damage**: How much damage the player deals (default: 50)
   - **Attack Range**: How far the attack reaches (default: 2)
   - **Enemy Layer**: Which layers contain enemies (default: all layers)
   - **Attack Point**: Where the attack originates from (leave empty to use player position)

### 3. Set up Animation Events

1. Open your player attack animation in the Animation window
2. Add an Animation Event where the attack should deal damage (usually mid-animation)
   - Set Function to: `TriggerAttackDamage`
3. Add another Animation Event at the end of the attack animation
   - Set Function to: `EndAttack`

### 4. Slime Setup

1. Add the `SuperSimpleSlimeAttack` component to each slime
2. Set up slime attack animation events:
   - Add Animation Event to slime attack animation
   - Set Function to: `DamagePlayer`

## HOW IT WORKS NOW

1. **Player presses attack button** → Your existing input system triggers
2. **Attack animation plays** → Your existing animation system works
3. **Animation event calls `TriggerAttackDamage`** → Damages enemies in range
4. **Animation event calls `EndAttack`** → Allows attacking again

## WHAT'S INTEGRATED

✅ Uses your existing `PlayerActionsInput` script
✅ Uses your existing input system (`OnAttack`)
✅ Uses your existing animation system
✅ Uses your existing health system
✅ Works with your existing level manager (ability unlock check)
✅ Compatible with your existing `BonkPressed` and `BonkLvl1Pressed` system

## DEBUGGING

- Check the Console for attack debug messages
- Select the player to see the red attack range sphere in the Scene view
- Make sure animation events are properly set up
- Verify slimes have the `Health` component

## SLIME SETUP (Quick Version)

For each slime:

1. Add `SuperSimpleSlimeAttack` component
2. Set damage and cooldown
3. Add animation event to attack animation: `DamagePlayer`
4. Make sure slime has collider and `Health` component

That's it! Your attack system is now fully integrated into your existing code!
