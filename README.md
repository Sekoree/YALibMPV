# YALibMPV
Yet Another LibMPV Wrapper/PInvoke library

All methods, structs, enums and callbacks from client.h, render.h, render_gl.h and stream_cb.h mapped (I think)

I'm too lazy to test everything, I just need this for another project, if you feel like something could be better, feel free to yell at me in issues or do a pull request.

This is partially inspired by [NickVision.MPVSharp](https://github.com/NickvisionApps/MPVSharp) and [LibMPV](https://github.com/homov/LibMpv) 

Any similarities are purely cause, well, we all map the same library.

## Current state of this

- Linux is very untested
- OSX too (I dont have hgardware for that, but I wanted to include it)
- The most tested stuff is the Node stuff cause I wanted to see if I could do a loadfile from a bytearray (didnt work :/)
- The render methods are also untested as I just used the "wid"/Native emebedding so far in my testing
