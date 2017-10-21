#pragma once

#include "ofMain.h"
#include "ofxFaceTracker2.h"
#include "ofxOsc.h"

#define HOST "localhost"
#define PORT 7110

class ofApp : public ofBaseApp{

	public:
		void setup();
		void update();
        void draw();

        void sendOsc(int index, float x, float y, float z);
    
        ofVideoGrabber grabber;
        ofxFaceTracker2 tracker;
        ofxOscSender sender;
    
        string oscAddress = "face";
        string hostName = "tracker";
    
};
