# Some-Words-Game

> **This project is on indefinite hold due to changes in Unity's pricing.** I will be looking into porting over to Godot instead.

This is a fairly simple game where you move tiles around on a grid to create words (a bit like Scrabble, or some levels in Baba Is You). Levels require the player to create specific words or a specific number of words to progress. Some tiles have special rules that apply to them - for example, tiles that cannot be moved or tiles that must always be part of a complete word. If any rules are broken, the player must undo their action and try something else.

This entire thing is me learning how Unity 2D works. Pretty much everything is done by script and all assets are loaded in at runtime. There are no guarantees I am doing things the 'right way' here, and while I am trying to apply professional software development practices where I can, it might not be the correct 'unity way' of doing things.

## Finished

* Allow player to move cursor
* Allow player to pick up and drop tiles
* Allow player to push tiles
* Prevent cursor or tiles from leaving the boundaries
* Allow undo of every move
* Revamp the undo/redo system to allow nested commands and don't rely on actions performing validity checks
* Load list of words from dictionary
* Show valid words when created
* Use a better border graphic

## In Progress
* Create level selection ('world map'?)
* Use the `SpriteRenderer` and dynamic colouring instead of creating every single sprite variant by hand

## TODO

* Implement better sound system (use resources folder)
* Update sounds and properly credit them
* Make tile backgrounds more subtle
* Add new graphics for special tiles
* Better graphics for when cursor is currently 'holding' a tile
* Immovable tiles
* Tiles that must always be part of 1 word
* Tiles that must always be part of 2 words
* Require player to undo if any rules are broken (related to the 2 above points)
* Victory condition by creating a specific word
* Victory condition by creating X words
* Secret exits (multiple victory conditions)
* Create or import music
* Create level data

## Maybe

* In-game level editor
* Victory condition: include specific tiles in a word
* Additional 'game packs' selectable from a menu
* Special secret words
