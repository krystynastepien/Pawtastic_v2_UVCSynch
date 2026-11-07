---
onenote-id: ce9e6c16-edba-475b-8923-4f2816fbc564:1
tags: [game-design, combat, instinct, yomi, csharp]
---

# Combat System

> Sources: Discord brainstorms (https://discord.com/channels/@me/669166014780669952), Reddit threads on RPS balance and Undertale-like combat, and a Game Developer article on Yomi.
> Core pitch: **"Yomi-based RPS system with weighted probabilities and stat-influenced prediction."** The cat's natural **Instinct** lets it read the enemy.

## Contents
1. [Design Goals & Inspiration](#1-design-goals--inspiration)
2. [Candidate Systems at a Glance](#2-candidate-systems-at-a-glance)
3. [System A: Zone Combat (Left / Center / Right)](#3-system-a-zone-combat-left--center--right)
4. [System B: Triangle Combat (Pounce / Swat / Hiss)](#4-system-b-triangle-combat-pounce--swat--hiss)
5. [Shared Systems](#5-shared-systems)
6. [Enemy Roster & AI](#6-enemy-roster--ai)
7. [Alternative / Supplementary Ideas](#7-alternative--supplementary-ideas)
8. [Implementation Notes (C#)](#8-implementation-notes-c)
9. [Open Questions](#9-open-questions)
10. [Reference Images](#10-reference-images)

---

## 1. Design Goals & Inspiration

**Goals**
- Turn-based, but it should **feel active**.
- Not pure dice: outcomes come from **reading the enemy** (prediction), not only luck.
- Stats shape *how well you can read*, not just how hard you hit.
- Cat theme in every action (pounce, swat, hiss, scratch, whiskers, ear twitches).

**Inspirations**
- *Legend of Zelda: Phantom Hourglass*: drawing spells.
- **Match-3** (early idea).
- **RPS balance in games**
  - Fire Emblem: swords > axes > spears > swords.
  - Pokémon: every element fits a big RPS model (fire/water/grass at its core).
  - Prediction and smart plays are the heart of the genre.
- **RPS-like concepts**: Cat & Mouse (power deficits overcome by playing to unique strengths), Tug of War (winning gives advantage, but fatigue matters), Tag (fastest or most capable wins).
- **Timing mini-games:** Shadow Hearts: Covenant's *Judgment Ring* (https://www.youtube.com/watch?v=_Uag_dGI5i4), Undertale's attack line, and Clair Obscur's action-point combos.

---

## 2. Candidate Systems at a Glance

| System | Core idea | Player skill | Complexity | Status |
|---|---|---|---|---|
| **A: Zone Combat** | Pick Left / Center / Right to attack or defend. Instinct widens your "buffer". | Prediction | Medium | Detailed |
| **B: Triangle Combat** | Secret simultaneous pick of Pounce / Swat / Hiss | Prediction + reading patterns | Low-Medium | Detailed |
| **Instinct Prediction** | See 3 possible enemy actions before committing (the umbrella idea behind A and B) | Prediction | Low | Concept |
| **Reflex Chain** | 3-5 s of real-time mini-games per turn | Reflexes | High | Concept |
| **Scratch Card** | Scratch panels in a time limit, avoid traps | Speed + risk | Medium | Concept |
| **Rhythm / Judge Ring** | Time inputs for "perfect" attacks | Timing | Medium | Supplement |

> **Suggested approach:** A and B are two versions of the same *prediction* idea. Pick one as the core loop. The Reflex Chain or timing mini-games can layer on top as attack execution.

---

## 3. System A: Zone Combat (Left / Center / Right)

### 3.1 Basic loop
- Standard turn-based 1v1 (or more).
- On your turn: **Attack**, **Defend**, or **Skill**.
- Attack and Defend each have **3 spots**: Left, Center, Right. The enemy also picks one.
- Guess right on defense and you can **parry and riposte** for bigger damage than a normal attack.
- Not a pure dice roll: it's RPS-like, and your stats improve your odds of guessing right.

### 3.2 Hidden roll (tutorial version)
The enemy "rolls" 0-100 each turn:

| Zone | Roll range |
|---|---|
| LEFT | 0-32 |
| CENTER | 33-66 |
| RIGHT | 67-100 |

- Ranges can change per enemy, e.g. LEFT 40% / CENTER 35% / RIGHT 25%.
- Visible hints can show it ("The enemy leans to the left", "High probability LEFT attack!").
- **Buffer from Instinct:** if the roll lands close enough to your chosen zone, it still counts. Example: you defend Center, the enemy rolls 38 (intends Left), and it's near enough that the block succeeds.

### 3.3 Instinct buffer zones

Base zones (no buffer):

```
LEFT:   [0 -------- 32]
CENTER:        [33 ----------- 66]
RIGHT:                    [67 ---------- 100]
```

| Instinct | Buffer | LEFT | CENTER | RIGHT | Extra |
|---|---|---|---|---|---|
| 0-20 | none | 0-32 | 33-66 | 67-100 | Pure 33/33/33 guessing |
| 21-40 | +5 | 0-37 | 28-71 | 62-100 | |
| 41-60 | +7 | 0-39 | 26-73 | 60-100 | |
| 61-80 | +10 | 0-42 | 23-76 | 57-100 | |
| 81-100 | +15 | 0-47 | 18-81 | 52-100 | See the exact enemy roll number |

**Worked example (Instinct 45, +7):**
- Enemy intends ATTACK and rolls **28**, which is in the LEFT range (0-32).
- Your CENTER zone extends to 26-73, so 28 falls in the overlap.
- If you chose CENTER, it **counts as a successful defense**.

### 3.4 Information shown to the player (by Instinct)

| Instinct | What you see |
|---|---|
| Low (10-30) | Vague bars / rough estimates only |
| Medium (31-60) | Accurate percentages (LEFT 40% / CENTER 35% / RIGHT 25%) |
| High (61-80) | Exact number ranges |
| Legendary (81-100) | The enemy's intended zone flashes briefly |

### 3.5 Outcome matrix

**Defend**

| Your choice | Enemy roll | Outcome |
|---|---|---|
| Same zone (exact) | Direct hit | ⭐ **Perfect Block**: Riposte (1.5x counter damage) |
| Same zone (buffer) | Within buffer | ✓ **Good Block**: can counter (1x damage) |
| Wrong zone | Outside buffer | ✗ **Hit**: take full damage |

**Attack**

| Your choice | Enemy | Outcome |
|---|---|---|
| Same zone (exact) | Enemy defends there | ✗ **Blocked**: no damage, wasted turn |
| Same zone (buffer) | Within enemy buffer | ○ **Partial Hit**: half damage |
| Different zone | Enemy elsewhere | ⭐ **Clean Hit**: full damage |

### 3.6 Skills (cost Energy ⚡)

Skills **bypass the zone system**. Energy regenerates slowly (+1 per turn).

| Skill | Cost | Effect |
|---|---|---|
| Fury Swipes | ⚡⚡ | Hits all 3 zones, guaranteed damage |
| Focused Pounce | ⚡⚡⚡ | Massive damage to one zone, enemy can't dodge |
| Perfect Dodge | ⚡⚡ | Auto-dodge the next enemy attack regardless of zone |
| Taunt | ⚡ | Forces the enemy to attack CENTER next turn |

### 3.7 Stats (System A)

| Stat | Effect |
|---|---|
| **Attack** | Base damage multiplier (50 = 100%, 75 = 150%) |
| **Defense** | Damage reduction when hit (50 = take 50% less) |
| **Speed** | Turn order (some enemies go first). Every 10 points makes enemy tells slightly slower, giving more time to decide |
| **Instinct** ⭐ | Buffer size and info clarity (see tables above) |

**Trade-off:** high Instinct makes defense easier but adds no damage, so builds should be balanced.

### 3.8 Multiple enemies

- **Option A: Sequential turns.** Enemy 1 attacks and you respond, then Enemy 2 attacks and you respond, then on your turn you choose one enemy to attack.
- **Option B: Choose focus.** You pick which enemy to engage. The others get free light damage on you, which rewards taking out threats quickly.

### 3.9 Zone AI behavior rules (general)
- Injured enemies favor their healthy side.
- Aggressive enemies prefer Center (direct approach).
- Defensive enemies prefer the sides (cautious).
- Previous moves influence the next move (patterns the player can learn).

---

## 4. System B: Triangle Combat (Pounce / Swat / Hiss)

### 4.1 Core rule
Both sides **secretly choose**, then **reveal simultaneously**.

```
POUNCE beats SWAT   (leap over the swipe)
SWAT   beats HISS   (smack the loud mouth)
HISS   beats POUNCE (startle mid-leap)
```

### 4.2 Outcomes

| Result | Example | Effect |
|---|---|---|
| **Win** | You POUNCE vs enemy SWAT | ⭐ Critical hit (1.5x damage), +1 Energy, **Momentum**: next attack +20% |
| **Lose** | You POUNCE vs enemy HISS | ✗ Startled: take 1.5x damage, lose momentum, briefly stunned (vulnerable next turn) |
| **Tie** | POUNCE vs POUNCE | ○ Attacks collide: both deal 0.5x, no bonuses, neutral state |

### 4.3 The three actions

|                                         | 🐾 **POUNCE**                                                              | 👋 **SWAT**                                                                                 | 😾 **HISS**                                                                                                   |
| --------------------------------------- | -------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| Role                                    | Aggressive, high risk / high reward                                        | Balanced, safe choice                                                                       | Defensive, utility                                                                                            |
| Cat behavior                            | Full-body leap, commits fully                                              | Quick reactive paw strike                                                                   | Intimidation, psychological warfare                                                                           |
| Base damage                             | 100% (highest)                                                             | 75%                                                                                         | 50% (lowest)                                                                                                  |
| Beats                                   | SWAT decisively                                                            | HISS reliably                                                                               | POUNCE (perfect counter to aggression)                                                                        |
| Loses to                                | HISS (startled mid-air)                                                    | POUNCE (leaped over)                                                                        | SWAT (smacked while hissing)                                                                                  |
| On win                                  | Crit bonuses                                                               | No special bonus                                                                            | Enemy **"shaken"**: next turn they deal -30% damage                                                           |
| Pros                                    | Beats SWAT, looks cool                                                     | Fast animation = harder to read, safe middle ground                                         | High utility, counters aggression                                                                             |
| Cons                                    | Long animation = predictable; vulnerable on tie                            | Boring? No bonuses                                                                          | Lowest damage, builds no momentum                                                                             |
| **Use when**                            | Enemy is defensive/passive; you have an HP lead; enemy used HISS last turn | Enemy is aggressive/loud (HISS users); you want consistent damage; you're learning patterns | Enemy is low HP and desperate (likely to POUNCE); you need to stall/heal; enemy has a momentum buff to remove |
| **Visual tell** (when an enemy does it) | Crouching low, butt wiggle, eyes locked on target                          | Weight shifting, one paw raised, focused alert posture                                      | Backing up, fur puffed, arched back                                                                           |

### 4.4 Instinct readout (UI)

```
╔════════════════════════════════════════════════╗
║         STRAY CAT (HP: 75/100)
║     [Enemy cat sprite - thinking pose]
║  "The stray's ears flatten..."
║  "Tail lashing aggressively..."
╠════════════════════════════════════════════════╣
║  Instinct Reading (Your Instinct: 52)
║
║  POUNCE: ████░░░░░░ 35%
║  SWAT:   ██████░░░░ 55%  ⚠️ LIKELY!
║  HISS:   ██░░░░░░░░ 10%
╠════════════════════════════════════════════════╣
║           CHOOSE YOUR MOVE:
║   [ POUNCE 🐾 ]   [ SWAT 👋 ]   [ HISS 😾 ]
║   Leap at enemy   Quick strike  Startle enemy
║
║  YOUR CAT (HP: 90/100)   Energy: ⚡⚡⚡○○
╚════════════════════════════════════════════════╝
```

| Instinct | Readout |
|---|---|
| Low (0-30) | Vague labels ("Likely", "Very Likely", "Unlikely") with rough bars |
| Medium (31-60) | Accurate percentages (e.g. 30 / 55 / 15) |
| High (61-80) | Percentages + recent history (e.g. "Used last turn ⚠️", "Never used") |

### 4.5 Skills (cost Energy ⚡)

Skills **bypass the triangle** and have guaranteed effects.

| Skill | Cost | Effect |
|---|---|---|
| Fury Swipes | ⚡⚡ | Hits regardless of enemy choice; 60% of Attack; can't be countered |
| Predator's Pounce | ⚡⚡⚡ | A POUNCE that HISS can't counter; 100% of Attack; builds momentum |
| Yowl | ⚡⚡ | Forces the enemy to use HISS next turn (fear), setting up your SWAT counter |
| Nine Lives *(once per battle)* | ⚡⚡⚡⚡ | If you would lose this round, **auto-win** instead; expensive, clutch |

**Energy:** +1 per turn, +1 extra on a win, +0 extra on a loss.

### 4.6 Combo: Hunting Mode
Win **3 rounds in a row** and:
- Next attack deals **2x damage**.
- The enemy is **panicked** (their Instinct against you drops).
- You get a **free skill use**.

### 4.7 Collar effects (ties into Inventory)

| Collar | Effect |
|---|---|
| Spiked Collar | Your SWAT also beats POUNCE (OP!) |
| Bell Collar | Enemy sees your choice 0.5 s early (disadvantage), but +20% damage |
| Lucky Collar | On a TIE, you win 70% of the time |

---

## 5. Shared Systems

### 5.1 Whisker Sense / Instinct (the "cat prediction" ability)
- Before each enemy turn, you see **3 possible actions** the enemy might take.
- Choose how to prepare: dodge left/right, block high/low, or interrupt.
- The enemy's actual choice is revealed *after* you commit.
- **Correct prediction → counter-attack bonus. Wrong → full damage.**
- **"Whisker Sense" stat** increases prediction accuracy.
- Some items reveal enemy patterns.
- Certain enemies have **tells** (ear twitch, tail lash).
- Optional extra layer: an RPS relationship such as *Pounce > Block > Swipe > Pounce*.

### 5.2 Energy & Action Points
- Energy powers Skills (regens +1 per turn).
- Clair Obscur-style idea: each skill has a **different combo input** and costs Action Points.
- **Defend** costs 1 AP, but a *successful* defend regains it.

### 5.3 Items in combat
- Quick drag-and-drop under time pressure (Reflex Chain idea).
- Items can reveal enemy patterns or add buffs (see Inventory.md).

---

## 6. Enemy Roster & AI

Each enemy has **tendencies**, so the player learns and exploits them (avoids pure randomness).

### 6.1 For System A (zones)

| Enemy | Archetype | Base L / C / R | Modifiers |
|---|---|---|---|
| **Rat** | Aggressive | 25 / 50 / 25 | HP < 50% → favors LEFT (protecting its right side). After attacking LEFT twice, won't attack LEFT again |
| **Crow** | Hit-and-run | 40 / 20 / 40 | If you attacked last turn → shifts away from that zone. If you defended last turn → targets that zone (punishes turtling) |
| **Raccoon** | Chaotic | 33 / 34 / 33 | Totally random (nightmare for high-Instinct builds), but **always telegraphs visually** (dumpster dives left = left attack) |
| **Boss: Feral Dog** | Pattern-based | n/a | **P1:** LEFT → CENTER → RIGHT, repeat. **P2 (HP < 50%):** random + occasional skill (hits all zones). **P3 (HP < 25%):** pure chaos, but slower (easier to react) |

### 6.2 For System B (triangle)

| Enemy | Archetype | Base P / S / H | Behavior |
|---|---|---|---|
| **Stray Cat** | Balanced, adaptive | 33 / 34 / 33 | HP > 70% → favors POUNCE. HP < 30% → favors HISS. After losing: 60% uses what beat you (revenge). After winning: 50% repeats the winning move (confident) |
| **Alley Rat** | Aggressive | 50 / 35 / 15 | Loves POUNCE. If you HISS twice in a row → 80% SWAT next (mad at your defense). HP < 40% → desperation POUNCE (all-in) |
| **Crow** | Tricky | 25 / 50 / 25 | Prefers SWAT (safe). Mirrors your last move 40% of the time. If you win 2 in a row → switches to what beats your last move (adapts) |
| **Raccoon** | Chaotic | 33 / 33 / 34 | Truly random each turn, but obvious tells (dumpster diving = POUNCE incoming). Instinct still helps you see the tell more clearly |
| **Boss: Feral Dog** | Pattern-based | n/a | **P1 (HP 100-60%):** SWAT → POUNCE → SWAT → HISS, repeat (learnable). **P2 (60-30%):** pattern breaks and becomes adaptive: counters your last move; if you win twice, uses a skill (bypasses the triangle). **P3 (30-0%):** desperate flurry: ~85% POUNCE spam, easy to counter with HISS, but he hits HARD |

---

## 7. Alternative / Supplementary Ideas

### 7.1 Reflex Chain System
*Turn-based but feels action-packed.* Each turn has **3-5 s of real-time action**:
1. **Choose action:** Attack / Defend / Item / Special.
2. **Execute mini-game**
   - **Attack:** tap indicators as they appear (rhythm-like, random pattern).
   - **Defend:** swipe toward incoming attacks to deflect.
   - **Items:** quick drag-and-drop under pressure.
3. **Combo:** landing **3+ attacks** enters combo state, which unlocks special moves (fury swipes, pounce barrage). Breaking the combo leaves a **vulnerability window**.

**Stats:** Speed = more indicators per turn. Attack = damage per successful input.

### 7.2 Scratch Card Battle *(unique)*
Turn-based with literal cat scratching:
- Each turn a **scratch card** with 9-12 panels appears.
- **Scratch/reveal quickly** (mouse drag or rapid click) within a time limit.

| Panel | Effect |
|---|---|
| ⚔️ Attack | Deal damage |
| 🛡️ Defense | Reduce incoming damage |
| ❤️ Health | Heal |
| 💎 Combo | 3 matching = special move |
| ☠️ Trap | Damage yourself, avoid! |

**Strategy:** scratch frantically for max reveals, or carefully avoid traps. Speed = longer scratch time. Defense = fewer trap panels.

### 7.3 Rhythm / Timing attacks
- Hit in a **specific rhythm** to land a perfect attack (Judgment Ring / Undertale line).
- Could be the *execution layer* on top of System A or B: choose the move, then time it for bonus damage.

---

## 8. Implementation Notes (C#)

**Zone resolution (System A)**

```csharp
public enum Zone { Left, Center, Right }

public static class ZoneMath
{
    // Base ranges: Left 0-32, Center 33-66, Right 67-100
    public static int GetBuffer(int instinct) => instinct switch
    {
        <= 20 => 0,
        <= 40 => 5,
        <= 60 => 7,
        <= 80 => 10,
        _     => 15
    };

    public static Zone RollToZone(int roll) =>
        roll <= 32 ? Zone.Left : roll <= 66 ? Zone.Center : Zone.Right;

    // True if the roll falls inside the player's chosen zone, extended by the buffer.
    public static bool IsBlocked(int roll, Zone chosen, int instinct)
    {
        int b = GetBuffer(instinct);
        var (lo, hi) = chosen switch
        {
            Zone.Left   => (0, 32 + b),
            Zone.Center => (33 - b, 66 + b),
            _           => (67 - b, 100)
        };
        return roll >= lo && roll <= hi;
    }
}
```

**Triangle resolution (System B)**

```csharp
public enum Move { Pounce, Swat, Hiss }

public static int Compare(Move a, Move b)
{
    if (a == b) return 0;
    bool aWins = (a == Move.Pounce && b == Move.Swat)
              || (a == Move.Swat   && b == Move.Hiss)
              || (a == Move.Hiss   && b == Move.Pounce);
    return aWins ? 1 : -1;
}
```

**Architecture tips**
- `EnemyBrain` returns a weighted distribution. A shared `WeightedPick(Dictionary<T,float>)` helper keeps the rules data-driven and lets you show percentages in the UI directly from the same numbers.
- Define each enemy in a `ScriptableObject`: base weights, modifier rules, tell animations, and phase thresholds for bosses.
- Keep a **move history** per battle (last N moves) for the pattern-based modifiers, revenge logic and high-Instinct "recent history" display.
- Use a `CombatState` machine: *Telegraph → PlayerChoose → Reveal → Resolve → Cleanup*.
- Apply Collar / item effects as modifiers (e.g. `IResolveModifier`) so you don't hard-code them into the core rules.

---

## 9. Open Questions

- [ ] **A or B?** The two systems share the idea but have different inputs. Which is the core, or are they different battle types (e.g. zones for normal fights, triangle for bosses)?
- [ ] Is the Instinct scale **0-100** everywhere? Tier thresholds differ slightly between notes (e.g. info tiers at 10-30/31-60/61-80 vs. 0-30).
- [ ] How much real-time action (Reflex Chain / rhythm) is wanted vs. a purely turn-based feel?
- [ ] How do **multiple enemies** work (sequential vs. focus choice)?
- [ ] Is "Nine Lives" a combat skill, an inventory charm (see Inventory.md), or both?
- [ ] Is Raccoon's "truly random but telegraphed" design fair to high-Instinct builds?
- [ ] Do ties feel good, or do they stall the pace?
- [ ] Which tells (ear twitch, tail lash, butt wiggle...) need real animation work?

---

## 10. Reference Images

![[Fight image.png]]
![[Fight image.jpg]]
![[Fight image 1.png]]
