# PowerCams

PowerShell cmdlet for discovering ONVIF IP cameras on your network and getting their RTSP stream info in one command!

## Install

```powershell
Install-Module PowerCams
```

## Requirements

- PowerShell 7+ (won't run on Windows PowerShell 5.1)
- .NET 8 runtime
- Same subnet as the cameras (discovery uses multicast)
- Need to make sure ONVIF is enabled on the camera. This can be a dedicated ONVIF user account or in some cases, ONVIF compliance is enabled out of the box.

## Usage

```powershell
Import-Module PowerCams
Find-OnvifCamera
```
It'll prompt for the camera username and password,then run the discovery.

### Example output

```
Manufacturer : AXIS
Model        : P3225-LV Mk II
Address      : 192.168.0.127
RtspUri      : rtsp://192.168.0.127/onvif-media/media.amp?profile=profile_1_h264&streamtype=unicast
```

Results are objects, so you can filter and pipe them:

```powershell
Find-OnvifCamera -Credential (Get-Credential) |
    Where-Object Manufacturer -eq 'AXIS' |
    Select-Object Model, RtspUri
```

## How it works

WS-Discovery finds the cameras, the ONVIF device service returns their details, and PowerCams resolves a ready-to-use RTSP URI for each.

## Built with

[OnvifDiscovery](https://www.nuget.org/packages/OnvifDiscovery), [Onvif.Core](https://www.nuget.org/packages/Onvif.Core), and [SharpRTSP](https://www.nuget.org/packages/SharpRTSP).
