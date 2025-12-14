# Setup

To use this, copy the folder `ApiLib` anywhere into your mod. (I suggest `Libraries/ApiLib`)

It is only needed once per Mod, no matter if you use it to provide your own API, or to access the API of another mod that uses it.

# Example

In the Example folder, you see the minimal code you will need to provide your own API with the help of this Library.

Simply copy the 3 folders `client`, `main`, and `shared` into your mod, rename some stuff, and instruct other modders to 
- copy `shared` and `client` from your mod into`Libraries/<YourModName>/`,
- and additionally, to copy this Library into their Libraries folder as well. (only once)
