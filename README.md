# Bohemia

In this Test I am spreading the fire in the direction of the wind and also in a certain radius. I have decided to split the burning process is three phases Ignition, Burning and Burned. Each phase time is different in each tree (random). Also the number of trees is randomly selected.

 All the inputs are being managed from the EvenSystem class, so the options that the user make are being set when the actual selection or button press is done. This may improve the performance a lot in case it was implemented by expecting the inputs changes in the Update method and ray casting.
All the Tree burning process is being managed by coroutines and nested coroutines. So each tree runs it's own burning process in its coroutine, so most of the performance that this game can consume by this "burning process" per tree is highly reduced by running it on coroutines since they are not running within the Update method. Also in this specific case coroutines are a preferred choice since they need to be controlled processes (paused and resumed) and also they will be stopped after the object is destroyed in contrast with sync threads.
Each time that an array has been used is set equals null to be disposed by the garbage collection process.
The radial spreading is being done by OverlapSphereNoneAlloc which allows store colliders in the radius generating no garbage.
All the trees are being parented with a Parent Object named "Bush" created on run-time. This allows to dispose all the memory used by all the trees and its processes (coroutines )at the time of clearing the full parent object when the user clicks on "Clear". 
