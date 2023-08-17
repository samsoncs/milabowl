---
title: The Rules of Milabowl
date: 2023-01-30
---
## Table of Contents
1. [General Rules](#general-rules)
2. [Specific Rules](#specific-rules)
3. [Rule Revisions](#suggested-rule-revisions)
4. [Future Rules](#future-rules)
    
## General Rules

> The first rule of Milabowl: Bugs are features🐛!
>
> -- <cite>MilaCorp</cite>

The most important rule of Milabowl is that bugs are in fact features.
What does this entail? If for some reason the implementation of say the 
_Cap Defender_ rule has a bug which prevents points from being given
if your captain's name starts with the letter **C**, then that is the
correct interpretation of the rule. The only way to revert this is to
call a vote and win by **majority**.

For rules where it makes sense (typically a per player rule) the score 
recieved from a player will be multiplied by two if he is captain and/or 
captained by you.

In the current state of Milabowl it is always the players that end up
on your team that counts. For instance if you initially benched _Andreas_
from Fullham (who hasn't?) and _Foden_ ends up not playing, then _Andreas_
will be substituted in for _Foden_. When calculating bench fail, it is
always the bench at the end of the round that counts, and not who was 
benched going in to the round. This rule might be revised in the future.

## Specific Rules

#### Sixtynine (69)
Get **6.9** points if your team hauls exactly 69 FPL points for the gameweek.

#### Yellow Card (YC)
Get **1** point per player in your active team receiving a yellow card.
This score is multiplied if a player is captained, leaving a potential
for 2 points!

#### Red Card (RC)
Get **3** points per player in your active team receiving a red card.
This score is multiplied if a player is captained, leaving a potential
for 6 points!

#### Bench Fail (BF)
Get **-1** point per 5 points left on the bench (at the end of a round).

#### Cap Fail (CF)
Get **-1** point if your captain __at the end of round__ receives less
than 5 points.

#### Cap Def (CD)
Get **1** point if your captain __at the end of round__ is a defender.

#### Minus is Plus (MiP)
Get **1** point per negative points each player gets (at the end of a round).
This score is multiplied if a player is captained.

#### Increase Streak (IS)
Get **1** point if this weeks round score is more than last weeks.

#### Equal Streak (ES)
Get **6.9** points if your team has the same FPL score as last gameweek.

#### Gameweek Position (GW PS)
Recieve points based on your position in the Milabowl FPL league this gameweek.
The current implementation goes like this:
- Group players by FPL gameweek score
- Order groupings by ascending points
- All players in first grouping (last place) receives **0** points
- For each grouping, the points assigned increases by **0.5** points
 
At the time of writing the league consists of 9 players, leaving a maximum
potential of **4** points, if no players had the same FPL score.

#### Head 2 Head Meta (H2H M)
Get **2** points if you beat your opponent in FPL Head to Head match with
2 points or less a given gameweek.

#### Sixtynine Sub (69 Sub)
Get **2.69** points if at least one of your players played exactly 69 minutes
in a gameweek. This score is multiplied if your captain plays 69 minutes, 
leaving a potential for **5.38** points!

#### Trendy Bitch (Trnd)
Get **-1** point if you traded in the most traded in player in FPL overall 
for a given gameweek. Get **-1** point if you traded out the most traded out 
player in FPL overall for a given gameweek.

#### Missed Penalties (Pn)
Get **-3** points for every player that misses a penalty (-6 points if a player
is captain)

## Powerups

#### Green Shell (Chip: Freehit)

#### Red Shell (Chip: Tripple Captain)

#### Banana (Chip: Wildcard)

#### Mushroom (Chip: Benchboost)

## Bomb
The bomb explodes in 7 random gameweek (but can't explode in gw 1). The bomb 
starts in posession of last years loser. The bomb is passed on to the h2h loser
if a player wins H2H, or to the player above you if you use any powerup. When
the bomb explodes, the holder will get -3 points.

## Suggested Rule Revisions
These are rules that have proposed revisions, that will be voted on during 
next board meeting.

#### GW PS 
Gameweek Position rule should start at 4 points, and decrease with 0.5 per 
grouping, instead of increasing from 0. Optionally it should skip X places
per grouping of X players (depends on implementation difficulty). I.E: If
two people share first place they should recieve 4 points, next grouping 
would then be 3rd place, and should recive 3 points (not 3.5 points). The
last grouping should always recive 0 points.

#### Trendy Bitch
Trendy bitch should be based on most traded player out/in within Milabowl 
FPL league, and not overall FPL trades.

## Future Rules
Here are suggestions for new rules, that will be voted on during the next 
board meeting of Milabowl. All the point values are provisional and should
also be determined during said board meeting.

#### Sellout (-$)
Get **-2** if the players you traded out this round recieved more points than
the players you traded in.