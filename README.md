# Ultimate Arcade Unity Game SDK

This is the Unity SDK for integrating games (servers and frontends) into the Ultimate Arcade.

The following diagram shows how the SDK is expected to be used, both on the client side (blue) and on the server side (yellow).

![arcade and game interaction flow diagram](./interaction-flow.png)

There are several tokens (JWTs, but you can treat them as opaque strings) which prove to
other parts of the system, what actions can be performed.

* The **Match Token** gets passed from the Arcade Launcher to the game's website where it has to be passed into the SDK and grants permission to play exactly once on a specific server
* The **Server Token** is handled transparently by the SDK and is just listed for completeness. It ensure server and game token match up. 


## Using it

### Server Side

* Ensure you're listening with TCP (WebSockets) on the port that is set in the `PORT` environment variable


## Installation

### Using Git

- In the Unity Editor go to `Window > Package Manager`
- Click the + at the top left of the Package Manager window
- Select `Add package from git URL...`
- Paste the following URL `https://github.com/UltimateTournament/ArcadeUnitySDK.git` and click `Add`

### Updating

If you installed the SDK using the Git URL you can simply open the package manager and re-paste the Git URL.
This should force a download of the latest code.


## Developing

When adding/updating protobuf files, remember to 
* keep the C#-specific properties (namespace)
* fix relative paths to match this repo
* update the .csproj file for additions/deletions

## Delivering the Game

* ensure to include a Dockerfile in your shipment. An example that should work with most games is included in this repo.
