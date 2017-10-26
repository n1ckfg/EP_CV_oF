#pragma once

#include "ofMain.h"
#include "ofxCv.h"
#include "ofxOsc.h"

#define HOST "localhost"
#define PORT 7110

class ofApp : public ofBaseApp {
    
    public:
        void setup();
        void update();
        void draw();
    
        ofVideoGrabber cam;
        ofxCv::ObjectFinder finder;
        ofImage sunglasses;
    
        ofxOscSender sender;
        void sendOsc(int index, float x, float y, float z);
        string oscAddress = "face";
        string hostName = "tracker";
    
};
