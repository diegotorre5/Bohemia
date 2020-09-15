# Bohemia

Gameplay programmer test task: Fire-spreading simulation

The goal of this task is to create a simple system simulating the spreading of fire among plants while taking into account the direction and speed of the wind. For this task please use free personal version of Unity3D engine that can be downloaded at www.unity3d.com.

Deadline: one week, starting the moment this test task is sent to you

Result: both built app (Windows platform - .exe) as well as packed project source files

Note:

l you don’t have to use textures or particles as the visual experience quality is not part of the test

l base plants will be presented as green, burning plants as red and burnt ones as black boxes.

l for any values / instructions not specified - feel free to improvise

l performance matters and is part of the test, feel free to comment on your implementation choices and what impact they have performance-wise

Please follow these steps:

1) Create a new project in Unity3D engine and 3D scene within the project

2) Create a simple terrain (smooth hills) via Unity terrain system

3) Create a simple GUI (either by utilizing the “new” Unity GUI system or older script-based GUI) containing buttons / sliders:

l Generate - generates new random plants on the terrain (while clearing any existing)

l Clear - clears all the existing plants

l Simulate - plays / stops the simulation

l Fire - lights several randomly selected plants

l Mode - toggles between various mouse interaction modes:

 Add - LMB adds a plant at the clicking point

 Remove - LMB removes the plant under the mouse pointer

 Toggle Fire - LMB lights / extinguishes fire in the plant under the mouse pointer

l Wind speed (slider) - affects the speed of the fire spreading

l Wind direction (slider 0-360 + some sort of visual indication in the scene (arrow, line etc.)) - affects the direction of the fire spreading

l Quit - quits the app

+++++++Proposed Solution

In this Test I am spreading the fire in the direction of the wind and also in a certain radius. I have decided to split the burning process is three phases Ignition, Burning and Burned. Each phase time is different in each tree (random). Also the number of trees is randomly selected.

 All the inputs are being managed from the EvenSystem class, so the options that the user make are being set when the actual selection or button press is done. This may improve the performance a lot in case it was implemented by expecting the inputs changes in the Update method and ray casting.
All the Tree burning process is being managed by coroutines and nested coroutines. So each tree runs it's own burning process in its coroutine, so most of the performance that this game can consume by this "burning process" per tree is highly reduced by running it on coroutines since they are not running within the Update method. Also in this specific case coroutines are a preferred choice since they need to be controlled processes (paused and resumed) and also they will be stopped after the object is destroyed in contrast with sync threads.
Each time that an array has been used is set equals null to be disposed by the garbage collection process.
The radial spreading is being done by OverlapSphereNoneAlloc which allows store colliders in the radius generating no garbage.
All the trees are being parented with a Parent Object named "Bush" created on run-time. This allows to dispose all the memory used by all the trees and its processes (coroutines )at the time of clearing the full parent object when the user clicks on "Clear". 
