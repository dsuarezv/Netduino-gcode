Netduino-GCode
==============

A GCode parser written in C# for the netduino platform. G-code (http://en.wikipedia.org/wiki/G-code) is a standard language used in automation (CNC, numerical control) to instruct machines the movements they need to perform in other to build a shape. 

== Components == 

The software is divided in several components: 

* The netduino application: this is the GCode parser that runs in the netduino and translates GCode to pulses suitable for stepper motor drivers. Currently it receives the GCode through the network connection of the netduino plus (through a listening server).
* A viewer application on the PC side feeds the GCode file to the netduino and visualizes progress of the operation. This is viewer in 3D of toolpaths in the gcode file. It was used to debug the parser and visualize the GCode paths while in development. Here is a screenshot: 

![Screenshot of the viewer in action](/screenshots/09%203D%20support.png)

It is also possible to connect to the netduino directly via telnet and paste the gcode over that connection. 

== Building == 

The project is a VS2010 express solution, that requires the netduino SDK to be present. 

There are several build configurations available. They build the netduino application or the PC software. Both applications share the same gcode parser code so that the viewer draws what the netduino will do (at least what refers to GCode, mechanically that's a different story).

== Configuring == 

Output pins and other parameters of the device are configured in the StepperBasic/CncDevice.cs class. Changing those parameters requires recompiling and redeploying for now. 

It is possible to calibrate the number of steps per mm without recompiling using a D92 command like this: 

    D92 X320 Y320 Z160

This means that the X axis needs 320 pulses to advance one mm, same for Y and Z needs half (160). 