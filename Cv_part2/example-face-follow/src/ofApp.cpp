#include "ofApp.h"

using namespace ofxCv;
using namespace cv;

void ofApp::setup() {
	ofSetVerticalSync(true);
	ofSetFrameRate(120);
	finder.setup("haarcascade_frontalface_default.xml");
	finder.setPreset(ObjectFinder::Fast);
	finder.getTracker().setSmoothingRate(.3);
	cam.setup(640, 480);
	sunglasses.load("sunglasses.png");
	ofEnableAlphaBlending();
    
    sender.setup(HOST, PORT);
}

void ofApp::update() {
	cam.update();
	if(cam.isFrameNew()) {
		finder.update(cam);
	}
}

void ofApp::draw() {
	cam.draw(0, 0);
	
	for(int i = 0; i < finder.size(); i++) {
		ofRectangle object = finder.getObjectSmoothed(i);
		sunglasses.setAnchorPercent(.5, .5);
		float scaleAmount = .85 * object.width / sunglasses.getWidth();
		ofPushMatrix();
		ofTranslate(object.x + object.width / 2., object.y + object.height * .42);
		ofScale(scaleAmount, scaleAmount);
		sunglasses.draw(0, 0);
		ofPopMatrix();
		ofPushMatrix();
		ofTranslate(object.getPosition());
		ofDrawBitmapStringHighlight(ofToString(finder.getLabel(i)), 0, 0);
		ofDrawLine(ofVec2f(), toOf(finder.getVelocity(i)) * 10);
		ofPopMatrix();
        
        sendOsc(i, object.x, object.y, 0);
	}
}

void ofApp::sendOsc(int index, float x, float y, float z) {
    ofxOscMessage m;
    m.setAddress("/" + oscAddress);
    m.addStringArg(hostName);
    m.addIntArg(index);
    m.addFloatArg(x);
    m.addFloatArg(y);
    m.addFloatArg(z);
    sender.sendMessage(m);
}
