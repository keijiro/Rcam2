Rcam2
=====

![gif](https://i.imgur.com/vdjkRG1.gif)
![gif](https://i.imgur.com/zUxXjbz.gif)

![gif](https://i.imgur.com/sqCRth4.gif)
![gif](https://i.imgur.com/t7tEp61.gif)

**Rcam2** is my second attempt at a real-time volumetric AR VFX system with
[Unity] (the first attempt is [Rcam]). This time I used iPad Pro with a LiDAR
scanner.

[Unity]: https://unity.com/
[Rcam]: https://github.com/keijiro/Rcam

The Rcam2 system consists of two software components: **RcamController** and
**RcamVisualizer**. RcamController runs on an iPad device and sends a video
stream and metadata (camera position, control data, etc.) to RcamVisualizer,
which runs on a desktop computer and renders VFX. It uses [NDI] to communicate
between these two components, so it doesn't require any special hardware but
only a network connection

[NDI]: https://www.ndi.tv/

I used Rcam2 in a [Boiler Room] stream on 24th September 2020. You can watch
the [recorded video] on Vimeo.

[Boiler Room]: https://boilerroom.tv/
[recorded video]: https://vimeo.com/462592995

System Requirements
-------------------

#### RcamController

- Unity 2020.1.6
- iPad Pro with LiDAR scanner (2020 model)
- iOS 14 and Xcode 12
- NDI SDK 4.5

#### RcamVisualizer

- Unity 2020.1.6
- HDRP-compatible desktop system

How To Run
----------

1. Build and run the RcamController project on the iPad device.
1. Play the RcamVisualizer project on the desktop system.
1. Select a controller from the dropdown list. You can hide the UI by clicking
   an empty area of the screen.

TIPS
----

- Although Rcam2 works with a wireless connection, it's recommended to use a
  wired ethernet connection for better latency and framerate. NDI works in a
  zero-configuration fashion, so you can directly connect a device and a
  computer without a router/switch.

- Holding an iPad device for a long time could be painful in a physical sense.
  You can relieve it by using [a camera handle].

[a camera handle]: https://twitter.com/_kzr/status/1309726929310765056

- Under a normal configuration with a wired connection, RcamController runs up
  to about two hours with a full charge.

- You can blur boundaries between rendered objects and real objects using the
  color grading and the depth-of-field effects. It's recommended to enable the
  "Film" and "DoF" toggles whenever these effects are acceptable.

FAQ
---

#### I can't find the iPad device on the dropdown list.

Try turning off the Windows firewall. It solves the problem in most cases.

#### I chose the right device from the dropdown list, but it's unresponsive.

If you're using a virtual network device, try turning it off. Note that WSL2
implicitly creates a virtual network device ([vEthernet]). You have to turn it
off to establish a connection correctly.

[vEthernet]: https://twitter.com/_kzr/status/1301722460421644289

#### Does it work with iPhone/iPad?

No. Rcam2 requires a LiDAR scanner. iPad Pro 2020 is the only device that
supports LiDAR at the moment.

#### Is it possible to make it run on an iPad Pro device as a standalone app?

It's technically possible but requires lots of changes. You might have to
remove some HD features (like the DoF effect) and reduce some
performance-related numbers like particle capacity counts in VFX.
You also have to tackle several device issues like thermal throttling,
battery life, etc.

Don't expect that I do anything in this direction. The controller-visualizer
design of Rcam2 is the most convenient way to avoid these problems.

<!--4567890123456789012345678901234567890123456789012345678901234567890123456-->
