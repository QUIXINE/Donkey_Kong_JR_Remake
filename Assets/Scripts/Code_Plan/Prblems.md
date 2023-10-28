
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
17.Player Movement: get to Dual-Handed after flip from Two_handed just on press
18.get on vine with only 1 vine and get to Dual-Handed, the hand is off
19.get off the vine, get from one shorter to longer and don't get off
20.DualHanded Right to two-handed L hands off and can't get back
21.Player-Movement: Flipping and DualHanded and Fall, DualHanded immediately instead of flip(while Two-Handed) to another side after pressing btn to flip, and sometime Get off the vine without stoping by DualHanded animation first immediately
24.Player-Movement: Player collides with platform 1st and can't move up on the vine (if collide with ground vertical won't receive input), can it DualHanded?
25.Score is not Don'tDestroyed or stored as PlayerPrefs 
26.if GameState is not Don'tDestroyed, i have to add GameState obj every scenes. if Don'tDestroyed //put player in Update() because it will be missing after reload the scene
27.Player health: doesn’t get reset (get back to 3) after player die(-3 life) and start a new game, it’s DontDestroyOnLoad(). 
28.Score: bonus and lap amount don’t reset after game over.
29.if there's platform on another side, player can still flip to another side, but player shouldn't be able to do that  
30.Player-Movement: jump from platform to the vine DK will flip wrong side (if hard press DK will get to another side of vine immediately)
31.Player-Movement: When on platform dk get on vine from a little too far from vine
    - check the ray on body, hand, head
    - should I create more layer for floating platform, while player on platform define the short ray distance so that player wouldn't be too far to get on vine
32.Player-Movement: while Dualhanded gets to another vine which is not with valid distance
    1.check getclose() that relates with Dualhanded, the distance of a ray
    2.check get to another vine func if the another side vine is found
33.Player-Movement: Player doesn't get on the vine when the vine is ahead of PosBody while still in range of ability to get on vine 
34.Player-Movement: if there's On head Playuer won't be able to reach out animation but the state is changed to DualHanded as coded

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
12.Game State: Player only stop first time the game start, after reloading scene player doesn't stop
--> solved by putting the code in GameState script which is not Don'tDestroyed, instead of PlayerHealth script;
13.Player-Life: Collide with 2 enemies take 2 life
--> solved by changing player layer to Default from Player(enemy will collide with Player layer) after taking damage from one enemy;
14.Player-Movement: jump from platform to the vine DK flip wrong side which makes Dk is off the vine, and can't continue any action
    Try the plan below to solve (solved by using plan 2)
    1. if not on vine and on ground HorOnVine =0
    2. TwoHanded, maybe because after jump to the vine on TwoHanded L or R, the check pos on body only check in direction of -transform.right, but when the player far from vine and the check pos on body needs to check with transform.right without transform.right Dk hands will get off the vine with the wrong sided TwoHanded (this has a problem because now three's a problem when player move to another vine, DK will flip to the wrong side inmmediately - move from L vine to R vine, DK should be on L side first after jump to get on the vine that is on R but it's another way around right now)
15.Player Movement: Fall off the vine, player won't fall because gravity is 0 after Fall from vine condition in DualHandedState() works
--> solved by using CurrentState = PlayerState.Idle; in Get off and Fall from vine condition and set gravity in (CurrentState == PlayerState.Idle && !IsGroundedChecker() && isOnVine condition) in OnVineGravityCheck() 

## Cuations
1.if use horizontalOnVine > 0 in Reaching first time player will get to Dual-Handed immediately
2.if ScoreRankJson doesn't load when is not reached the Rank_Scene yet try change LoadHighScore() in ScoreRankTable script to static and call in GameState script or anywhere 


## TODO:
//8/31/2023
//make dk jr's hands are on the vine within the its boundary

//9/14/2023 
1.Fall from vine (!FoundAnotherVine() && push btn of the reach side again (L then L again))✅
2.Player life
3.Score

//9/18/2023 
1.Falling height check
2.Score - jump above enemy to get point, enemy stacking points. fruit collide with enemy get points, fruit stacking points 

//9/19/2023 
1.Falling height check ✅
2.Score - jump above enemy to get point, enemy stacking points. fruit collide with enemy get points, fruit stacking points 
3.jump get on vine with little touch of hand - use vineDualHandPos01.up or other direction to check✅ --> use vineCheckPosBody with rayDistanceOnBody instead
4.check ground-vine mixed (1st secene 1st platform) bottom is on ground but still dual/two-handed, if Two-Handed or Dual-Handed && IsGrouded() && IsOnVine() && FoundAnotherVine() player will be Two-handed or Dual-Handed state

//9/21/2023 
1.Score - jump above enemy to get point, enemy stacking points ❌. fruit collide with enemy get points ✅, fruit stacking points ✅
2.Player Life --> collide with enemy, water, and fall from height, --health. ✅
3.Score - Highscore store and loadscene 

//9/25/2023
1.Jump avoid enemy get point ✅
2.Bonus score
3.Scene loading 

//9/26/2023
1.Bonus score
2.Scene Loading
    - Game over 
        - every -1 life wait 4 sec. to load new scene✅
3.Game State - every time there is reloading scene Dh jr will not active for 1 sec then active ✅
4.Score board scene
    - find a way to do it

//10/19/2023
1.to solve player Dualhanded can't completely reach the vine from another side
    - check if how many Getclose() I really need
    - check the operator (+, -)
    - check the value or distance to get close to the vine 
    - check when back03, 04, 01
    - check what does each func is for

    now: pos.x -= GetDistanceToReachVineCloser(); stop this to see if I can only use ReachVineCloser() to make Dk get close to the vine

//10/23/2023, 10/24/2023, 10/25/2023
1.polish Player Movement
    DualHanded
        Reaching distance ✅ --> solved by using vineCheckPosDualHand02 to check if the hand is off instead //❌_Level02 Reaching distance is still not ok
        - What's wrong with DualHandPos01 checker? If there's nothing wrong it should detect and get closer to the vine.
        - If there's nothing wrong with DualHandPos01 try use DualHandPos02 to detect the vine with left trasform direction. Will there be a problem like error position?
        - check GetDistanceToReachVineCloser() isn't it used to get closer to the vine? if it is, why there's still a few distance farther from the vine
        - DK try to get close to the far vine. Should I use bodyCheckPos instead? if I use DH01CheckPos I have to make sure ray distance will not detect the far vine so that DK won’t get close to it.
    Get On Vine
        - problem 31
        - problem 33 relates w/ //Used hand instead of body (Bookmark)
    problem 24
    Polish IsOnVineChecker()
2.Polish Player Health  (Singleton), destroy when is at Menu Scene✅
3.polish Score (Score UI, Singleton)✅

//10/28/2023
1.polish Player Movement
    DualHanded
        Reaching distance 
        - What's wrong with DualHandPos01 checker? If there's nothing wrong it should detect and get closer to the vine.
        - If there's nothing wrong with DualHandPos01 try use DualHandPos02 to detect the vine with left trasform direction. Will there be a problem like error position?
        - check GetDistanceToReachVineCloser() isn't it used to get closer to the vine? if it is, why there's still a few distance farther from the vine
        - DK try to get close to the far vine. Should I use bodyCheckPos instead? if I use DH01CheckPos I have to make sure ray distance will not detect the far vine so that DK won’t get close to it.
        - still won't reach to the another vine properly
    Get On Vine
        - problem 31
        - problem 33 relates w/ //Used hand instead of body (Bookmark)
    problem 24
    Polish IsOnVineChecker()