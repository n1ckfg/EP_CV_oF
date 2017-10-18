#include "ofApp.h"

using namespace cv;
using namespace ofxCv;

void ofApp::setup() {
	cam.setup(1920, 1080);
	thresh.allocate(1920, 1080, OF_IMAGE_GRAYSCALE);
}

void ofApp::update() {
	cam.update();
	if(cam.isFrameNew()) {
		convertColor(cam, thresh, CV_RGB2GRAY);
		if(ofGetMousePressed()) {
			autothreshold(thresh);
		} else {
			float thresholdValue = ofMap(mouseX, 0, ofGetWidth(), 0, 255);
			threshold(thresh, thresholdValue);
		}
		thresh.update();
	}
}

void ofApp::draw() {
	cam.draw(0, 0);
	thresh.draw(640, 0);
}
