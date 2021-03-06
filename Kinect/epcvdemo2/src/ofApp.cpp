#include "ofApp.h"

using namespace ofxCv;
using namespace cv;

void ofApp::setup() {
	//cam.setup(1280, 720);
    
    thresh.allocate(640, 480, OF_IMAGE_GRAYSCALE);
    
    contourFinder.setMinAreaRadius(10);
	contourFinder.setMaxAreaRadius(150);
	//contourFinder.setInvert(true); // find black instead of white
    
    gui.setup();
    gui.add(thresholdVal.set("Threshold", 128, 0, 255));
    gui.add(trackHs.set("Track Hue/Saturation", false));
    gui.add(holes.set("Holes", false));
    
    astra.setup();
    astra.enableRegistration(bUseRegistration);
    astra.initDepthStream();
    
    sender.setup(HOST, PORT);
}

void ofApp::update() {
	//cam.update();
    astra.update();
    //if(cam.isFrameNew()) {
    if (astra.isFrameNew()) {
        //convertColor(cam, thresh, CV_RGB2GRAY);
        thresh.clone(astra.getDepthImage());
        float thresholdValue = ofMap(mouseX, 0, ofGetWidth(), 0, 255);
        threshold(thresh, thresholdValue);
        thresh.update();
        
        //contourFinder.setTargetColor(targetColor, trackHs ? TRACK_COLOR_HS : TRACK_COLOR_RGB);
        contourFinder.setThreshold(thresholdVal);
        contourFinder.setFindHoles(holes);
        contourFinder.findContours(thresh);
    }
}

void ofApp::draw() {
	ofSetColor(255);
	//cam.draw(0, 0);
    thresh.draw(0,0);
	
	ofSetLineWidth(2);
	contourFinder.draw();
	
	ofNoFill();
	int n = contourFinder.size();
	for(int i = 0; i < n; i++) {
		// smallest rectangle that fits the contour
		ofSetColor(cyanPrint);
		ofPolyline minAreaRect = toOf(contourFinder.getMinAreaRect(i));
		minAreaRect.draw();
		
		// ellipse that best fits the contour
		ofSetColor(magentaPrint);
		cv::RotatedRect ellipse = contourFinder.getFitEllipse(i);
		ofPushMatrix();
		ofVec2f ellipseCenter = toOf(ellipse.center);
		ofVec2f ellipseSize = toOf(ellipse.size);
		ofTranslate(ellipseCenter.x, ellipseCenter.y);
		ofRotate(ellipse.angle);
		ofDrawEllipse(0, 0, ellipseSize.x, ellipseSize.y);
		ofPopMatrix();
		
		// minimum area circle that encloses the contour
		ofSetColor(cyanPrint);
		float circleRadius;
		ofVec2f circleCenter = toOf(contourFinder.getMinEnclosingCircle(i, circleRadius));
		ofDrawCircle(circleCenter, circleRadius);
		
		// convex hull of the contour
		ofSetColor(yellowPrint);
		ofPolyline convexHull = toOf(contourFinder.getConvexHull(i));
		convexHull.draw();
		
		// defects of the convex hull
		vector<cv::Vec4i> defects = contourFinder.getConvexityDefects(i);
		for(int j = 0; j < defects.size(); j++) {
			ofDrawLine(defects[j][0], defects[j][1], defects[j][2], defects[j][3]);
		}
		
		// some different styles of contour centers
		ofVec2f centroid = toOf(contourFinder.getCentroid(i));
		ofVec2f average = toOf(contourFinder.getAverage(i));
		ofVec2f center = toOf(contourFinder.getCenter(i));
		ofSetColor(cyanPrint);
		ofDrawCircle(centroid, 1);
		ofSetColor(magentaPrint);
		ofDrawCircle(average, 1);
		ofSetColor(yellowPrint);
		ofDrawCircle(center, 1);
		
		// you can also get the area and perimeter using ofPolyline:
		// ofPolyline::getArea() and ofPolyline::getPerimeter()
		double area = contourFinder.getContourArea(i);
		double length = contourFinder.getArcLength(i);		
		
		// balance is useful for detecting when a shape has an "arm" sticking out
		// if balance.length() is small, the shape is more symmetric: like I, O, X...
		// if balance.length() is large, the shape is less symmetric: like L, P, F...
		ofVec2f balance = toOf(contourFinder.getBalance(i));
		ofPushMatrix();
		ofTranslate(centroid.x, centroid.y);
		ofScale(5, 5);
		ofDrawLine(0, 0, balance.x, balance.y);
		ofPopMatrix();
        
        if(contourFinder.getHole(i)) {
            ofDrawBitmapStringHighlight("hole", center.x, center.y);
        }
        
        sendOsc(i, centroid.x, centroid.y, 0);
	}

    /*
    gui.draw();
    
	ofTranslate(8, 90);
	ofFill();
	ofSetColor(0);
	ofDrawRectangle(-3, -3, 64+6, 64+6);
	ofSetColor(targetColor);
	ofDrawRectangle(0, 0, 64, 64);
    */
}

void ofApp::mousePressed(int x, int y, int button) {
	//targetColor = cam.getPixels().getColor(x, y);
    
    ofSaveFrame();
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

