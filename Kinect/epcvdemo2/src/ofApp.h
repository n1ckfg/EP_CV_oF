#pragma once

#include "ofMain.h"
#include "ofxCv.h"
#include "ofxGui.h"
#include "ofxOrbbecAstra.h"
#include "ofxOsc.h"

#define HOST "localhost"
#define PORT 7110

class ofApp : public ofBaseApp {
public:
	void setup();
	void update();
	void draw();
	void mousePressed(int x, int y, int button);
	  
	//ofVideoGrabber cam;	
	ofImage thresh;
    ofxCv::ContourFinder contourFinder;
	ofColor targetColor;
    
    ofxPanel gui;
    ofParameter<float> thresholdVal;
    ofParameter<bool> trackHs;
    ofParameter<bool> holes;
    
    ofxOrbbecAstra astra;
    ofVboMesh mesh;
    ofEasyCam cam;
    
    bool bDrawPointCloud = false;
    bool bPointCloudUseColor = false;
    bool bUseRegistration = false;
    
    ofxOscSender sender;
    void sendOsc(int index, float x, float y, float z);
    string oscAddress = "face";
    string hostName = "tracker";
    
};
