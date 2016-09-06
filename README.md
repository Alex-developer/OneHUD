
![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/OneHUD/assets/OneHUD Logo.png)

## Overview

OneHud is the server and dashboard component for my sim racing telemetry app.

The server provides data in real time for several games out of the box. Additional plugins can be devel;oped by 3rd party developers, full details for how to build plugins is provided.

The current supported games are

- iRacing
- Project Cars
- Project Cars 2
- Assetto Corsa
- Raceroom Experience

## Features

- 100% Open Source
- 100% HTML and Javascript client allowing it to run on any modern tablet.
- Supports pretty much any phone / tablet and modern desktop browser
- Built in pages
  - Dashboard
  - Timing Screen
  - Track recorder
  - Track map
  - Options
- The dashboards are fully customisable
  - Fully editable in a browser
  - Built in list of widgets
  - Full documentation for developers to build their own widgets
- Built in track recorder, for games that support it
- Auto program start
  - Allows your favourite apps to be started when a game is launched
- Support for my button box ! (See Below)

## Screenshots

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/OneHUD/assets/Screenshots/Misc/editor.png)

The Dashboard editor

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/OneHUD/assets/Screenshots/iPhone6/dash.png)

Dashboard running on an iPhone 6 - NOTE: This is a test dash, the real ones will look better !

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/OneHUD/assets/Screenshots/iPhone6/timing.png)

iRacing timing screen running on an iPhone 6

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/OneHUD/assets/Screenshots/Misc/trackrecorder.gif)

The track recorder (See videos below). NOTE: This is very much an Alpha version of the recorder.
The idea behind the track recorder is that you drive a few laps in the game and pick the best one to then use on the dashboards or trackmap. There are still a few issues, particularly on the first lap out of the pits but they will be sorted.

## Videos

[https://www.youtube.com/watch?v=80sd_RzdSD4](https://www.youtube.com/watch?v=80sd_RzdSD4)

Alpha version of the track recorder

## Button Box

Alongside OneHUD I am building a button box. Its fairly standard except for the iclusion of a Raspberry Pi which can display data from OneHUD or be used as a race computer.

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/Screenshots/Button%20Box/button%20box%20components.jpg)

The components for the button box

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/Screenshots/Button%20Box/button%20box%20prototype%20panel.jpg)

The prototype panel. I am using a think card to build various layout to decide which is the best

![alt tag](https://github.com/Alex-developer/OneHUD/blob/master/Screenshots/Button%20Box/buttonbox%20prototype.jpg)

The first prototype up and running

### Older videos - Previous versions

A video of the latest version showing the default iPhine dash running on lots of devices [https://www.youtube.com/watch?v=Fo_JXQd9oz4](https://www.youtube.com/watch?v=Fo_JXQd9oz4)

A video of the Alpha track map can be found at  [https://www.youtube.com/watch?v=h2GRYcLq-vY](https://www.youtube.com/watch?v=h2GRYcLq-vY/)

An early Alpha version of the dash running can be found at [https://www.youtube.com/watch?v=TkxlwAKm2Eo](https://www.youtube.com/watch?v=TkxlwAKm2Eo/)

The PC side of the app will also support the hardware dash I am building for iRacing / pCars (Again all opensource software and hardware). A crappy video is available at [https://www.youtube.com/watch?v=6MKJRQLznj8](https://www.youtube.com/watch?v=6MKJRQLznj8/)


# Credits

Chris Kinsman - For the new name (OneHUD) and graphics
Jim Britton - For CrewChief [https://github.com/mrbelowski] . This has saved me hours in working out the Project Cars UDP Data format. The UDP data structure in OneHud is based upon the code from CrewChief with some modifications
