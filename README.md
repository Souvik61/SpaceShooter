# SpaceShooter
 A modified version of Unity Multiplayer Tutorial

This is a modified/remixed version of the 
 [2D Space Shooter Sample](https://github.com/Unity-Technologies/com.unity.multiplayer.samples.bitesize/tree/main/Basic/2DSpaceShooter) created by **Unity**.

### With added features like:
- Play in a LAN Party.
- Players can play from both Android/PC.
- Auto Local IP detection.
- Two types of connection modes.
- And a new spaceship.

### Instructions
The game can be played in 2 ways :<br>
#### Gamemode A :
- One player has to join as host.
- Other players has to join as client.
- The host device will have the advantage of the lowest latency.
#### Gamemode B :
- One device has to join as server.
- Other players has to join as client.
- The server device cannot play the game.

**Players can join from both PC and Android and must be on same LAN/WiFi.**

**Playable versions are in Assets/Scenes/MyScenes**

### Bugs
- If something does not work properly just restart.
- There is a bug where if it fails to connect it just shows a blank screen instead of a helpful message. Just restart the game and check your connection
to the WiFi/LAN.

## Screenshots
![Screenshot1](https://user-images.githubusercontent.com/35128994/236508796-76949217-ac58-4377-91f6-b1223108a26b.jpg)
![Screenshot2](https://user-images.githubusercontent.com/35128994/236508694-2c34cae7-0cec-4749-b213-f663888c73c7.jpg)
![Screenshot 2023-05-06 214602](https://user-images.githubusercontent.com/35128994/236635899-a7b3e538-a468-4701-8b8d-bee26c979c25.png)
![Screenshot_20230506-214650](https://user-images.githubusercontent.com/35128994/236635911-fe7feb0b-c686-4683-8270-34b98e718024.png)
![Screenshot_20230506-214704_SpaceShooter](https://user-images.githubusercontent.com/35128994/236635914-b3ec3e16-6066-4fed-bfa2-56d463a1c1ff.jpg)

### Feats
- Modified the original demo game to work with android.
- Programmed to automatically find the local ip of the device in the network for connection.
- Removed PostProcessing and Effects for improved performance in mobile devices.
- Changed original ship sprites/fire modes.
- Modified so that the same code base works with Android and PC.
- Improved UI and added help texts.
