## Problems
1.Problem: off the ground, player don't stop moving
2.Problem: In original game, after get down from the vine to the ground, can't move for a short time, but now mine can move
3.immediately after get to the ground
4.Problem: if holds horizontalOnVine player will move to that side such as jump to catch the vine from left side which has to hold right arrow,
    player will move to right side, but if only press it carefully, player will get on the left side
5.***Problem: Climbing speed errors, climb up with one hand speed is not equal climbUpSpeed and down is not climbDownSpeed after flipping of Idle on vine
    it takes climbDualHandSpeed instead
6.***Problem: Idle state get to reach state immediately, can't flip left-right of climbIdle animation but get to reach state instead, but after get back to Idle state, can flip.
7.Problem: Don't go down when there's no any vines on another side
    (1 - Transit to two-handed)Problem +=, -= always apply
8.Problem: reaching out doesn't wait the normal flip to finish as stepby step but imdediately reach out 
9.Problem: Fix all the changing DK jr position when get to the vine, for ex, jump under or even from the side of the vine to get on it, sometimes the position is not perfect or even get off the vine
10.***Check pos on Vine, sometime does't on the vine because of player postion
11.***Reach first time only one and can't
12.***GetDistanceToGetCloseVine() and Reach out/back have to be the same values. how much Reach out, Reach back has to be the same
13.Animation fall from height and fall from vine are not the same
14.Get to another vine x,y pos
15.After getting on another vine, if hold the btn, player will move to another side of the vine immediately, for ex, from Two-Handed L holding RightArrow after Dual-Handed R to get to another vine player move to the R of the another vine immediately
//instead of L(only see it in slight time, player can't really tell it's go to L before R) before to R, in the original game player will see L side and longer than this situation
16.Fall from height check rb.velocity.y when get down from the vine  rb.velocity.y = -float, leads to dead
17.get to Dual-Handed after flip from Two_handed just on press
18.get on vine with only 1 vine and get to Dual-Handed, the hand is off
19.get off the vine, get from one shorter to longer and don't get off
20.DualHanded Right to two-handed L hands off and can't get back
21.Player-Movement: Flipping and DualHanded and Fall, DualHanded immediately instead of flip(while Two-Handed) to another side after pressing btn to flip, and sometime Get off the vine without stoping by DualHanded animation first immediately

23.Player-life: Player only stop first time the game start, after reloading scene player doesn't stop
24.Player-Movement: Player collides with platform 1st and can't move up on the vine (if collide with ground vertical won't receive input)
25.Score is not Don'tDestroyed or stored as PlayerPrefs 


## Solved
1.Solved: gravity after jump, when collide with vine player float up
2.Solved: fliping doesn't move sprite to left or right side of the vine, but only flip itself
--> solved by move position a little, look in FlipInput()
3.Solved: if OnGround and hold the x (jump), the jump animation will continue to play, and by getting on vine then get back to the ground
--> solved by add another bool animation check "StopJump"

//Code in Used for reaching when get on the vine every first time after get off the ground
4.Solved: Try to fix after get up on vine don't reach out, when player gets up on the left side and try to reach out to the left side
player can't reach out --> solved by 2 conditions below
5.Solved: reach out immediately when flip to another side 
solved by usising Input.GetKeyDown instead of horizontalOnVine, becasuse horizontalOnVine will make player reach out immediately after push the btn
horizontalOnVine == 1 || -1 which meets the condition 
6.Solved: reach first time R while there's no vine on right side, player doesn't move to R side so that the hand will be on vine, maybe because checker condition only checks
when Raycast hits the vine (Vector2.right -- there's no any vines on Vector2.right, so it doesn't meet condition) 
--> solved by adding more condition in GetDistanceToGetCloseVine() using hitReachRWithNoVine Raycast
7.Solved: -=, += always apply, means position will always change and if they don't + and - each other, the hand position will not be on the vine
8.Solved: Reach R checker (GetDistanceToGetCloseVine()) doesn't check
9.Solved: IsOnVine() || FoundAnotherVine() doesn't work (set condition wrong)
Fall from height uses Rigidbody, if on vine and use vertical down rb will negative and lesser than the dying fixed height variable, leads to dead which is wrong
10.Fall from height uses Rigidbody, if on vine and use vertical down rb will negative and lesser than the dying fixed height variable, leads to dead which is wrong
--> solved by changing rb.velocity to Vector2.zero; in OnTriggeredExit after get off the vine  
11.Jump doesn't check enemy to get point when jump side to side, but check after jump straight 
--> solved by using Physics2D.CircleCaastNonAlloc();

## Cuations
1.if use horizontalOnVine > 0 in Reaching first time player will get to Dual-Handed immediately


## TODO:
//8/31/2023
//make dk jr's hands are on the vine within the its boundary

//9/14/2023 
//To-do-list
//1.Fall from vine (!FoundAnotherVine() && push btn of the reach side again (L then L again))✅
//2.Player life
//3.Score

//9/18/2023 
//To-do-list
//1.Falling height check
//2.Score - jump above enemy to get point, enemy stacking points. fruit collide with enemy get points, fruit stacking points 

//9/19/2023 
//To-do-list 
//1.Falling height check ✅
//2.Score - jump above enemy to get point, enemy stacking points. fruit collide with enemy get points, fruit stacking points 
//3.jump get on vine with little touch of hand - use vineDualHandPos01.up or other direction to check✅ --> use vineCheckPosBody with rayDistanceOnBody instead
//4.check ground-vine mixed (1st secene 1st platform) bottom is on ground but still dual/two-handed, if Two-Handed or Dual-Handed && IsGrouded() && IsOnVine() && FoundAnotherVine() player will be Two-handed or Dual-Handed state

//9/21/2023
1.Score - jump above enemy to get point, enemy stacking points ❌. fruit collide with enemy get points ✅, fruit stacking points ✅
2.Player Life --> collide with enemy, water, and fall from height, --health. ✅
3.Score - Highscore store and loadscene 

//9/25/2023
1.Jump avoid enemy get point ✅
2.Bonus score
3.Scene loading 
