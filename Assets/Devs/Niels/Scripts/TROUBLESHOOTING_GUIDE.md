# ATTACK SYSTEM TROUBLESHOOTING GUIDE

## PROBLEM 1: Player Dies When Killing Slime ✅ FIXED

**Issue**: The player attack was hitting everything in range, including the player itself.

**Solution**: Updated `TriggerAttackDamage()` to:

- Skip the player GameObject
- Only damage objects tagged as "Enemy" or with Slime component

## PROBLEM 2: Slime Does No Damage

### Quick Fix - Use the New SlimeAttack Script

1. **Add `SlimeAttack` component to each slime**
2. **Set up the slime collider**:

   - Make sure slime has a Collider component
   - Set the collider as a **Trigger** (check "Is Trigger")
   - Or leave as normal collider for collision-based damage

3. **Configure in Inspector**:
   - Set Damage (default: 10)
   - Set Attack Cooldown (default: 2 seconds)
   - Drag slime's Animator (optional)
   - Set Attack Trigger name (optional)

### Debugging Steps

1. **Check Console Messages**:

   - Look for "Slime trigger entered by: [name]"
   - Look for "Player entered slime attack range!"
   - Look for "Slime attacking player!"

2. **Check Player Tag**:

   - Make sure your Player GameObject has tag "Player"

3. **Check Slime Setup**:

   - Slime has `SlimeAttack` component
   - Slime has `Health` component
   - Slime has Collider (with "Is Trigger" checked)

4. **Check Player Setup**:
   - Player has `Health` component
   - Player has tag "Player"

### Alternative: Use Existing SuperSimpleSlimeAttack

If you prefer the original system:

1. Add `SuperSimpleSlimeAttack` component to slimes
2. Set up collider as trigger
3. Add animation event `DamagePlayer` to slime attack animation

## Testing Your Setup

1. **Player Attack Test**:

   - Attack near a slime
   - Check console for "Player attacked [slime name] for X damage!"
   - Slime should take damage, player should NOT

2. **Slime Attack Test**:
   - Walk into slime
   - Check console for slime debug messages
   - Player should take damage

## Common Issues

### Player Still Dies When Attacking

- Check if player has "Enemy" tag (it shouldn't)
- Make sure `enemyLayer` in PlayerActionsInput is set correctly

### Slime Still Doesn't Damage Player

- Check player has tag "Player" (case sensitive)
- Check slime collider is set as trigger
- Check console for debug messages
- Make sure player has Health component

### No Debug Messages

- Check slime has `SlimeAttack` component attached
- Check collider is large enough for player to enter
- Check player actually touches the slime

## Quick Setup Checklist

### Player Setup ✅

- [x] PlayerActionsInput component (with attack system)
- [x] Health component
- [x] Tag: "Player"

### Slime Setup

- [ ] SlimeAttack component
- [ ] Health component
- [ ] Collider with "Is Trigger" checked
- [ ] Tag: "Enemy" (optional but recommended)

### Testing

- [ ] Player can attack and damage slime
- [ ] Player doesn't damage itself
- [ ] Slime damages player on touch
- [ ] Console shows debug messages
