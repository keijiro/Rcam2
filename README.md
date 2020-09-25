Rcam2
=====

![gif](https://i.imgur.com/vdjkRG1.gif)
![gif](https://i.imgur.com/zUxXjbz.gif)

![gif](https://i.imgur.com/sqCRth4.gif)
![gif](https://i.imgur.com/t7tEp61.gif)

<!--4567890123456789012345678901234567890123456789012345678901234567890123456-->

Rcam2 is my second attempt at making a real-time volumetric/AR VFX system with
[Unity] (the first attempt is [Rcam]). This time I used iPad Pro with a LiDAR
scanner.

[Unity]: https://unity.com/
[Rcam]: https://github.com/keijiro/Rcam

Rcam2 uses an iPad device as a remote camera/controller for the visualizer
running on a desktop computer. It uses [NDI] to send a video stream and
metadata (camera position, control data, etc.), so it doesn't require special
hardware but only a network connection.

[NDI]: https://www.ndi.tv/

I used Rcam2 in a Boiler Room stream on 24th September 2020. You can see the
[recorded video] on YouTube.

[recorded video]: https://youtu.be/ANVNNxid2to

System Requirements
-------------------

- HDRP-compatible desktop system
- iPad Pro with LiDAR scanner (2020 model)

How To Run
----------

1. Build and run the RcamController project on the iPad device.
1. Play the RcamVisualizer project on the desktop system.
