using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class OSCConnection : MonoBehaviour{
    public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host 
    public int ListenerPort = 5000; //the port you will be listening on
    Osc handler;
    UDPPacketIO udp;
    public bool signal = false;
	public float acc0, acc1, acc2;
    public float s0, s1, c1, blink;
	public float alpha, beta, conn;

    void Start() { // Use this for initialization
        udp = new UDPPacketIO(); //Initializes on start up to listen for messages
        udp.init(RemoteIP, ListenerPort);
        handler = new Osc();
        handler.init(udp);
        handler.SetAllMessageHandler(AllMessageHandler);
    }

    void Update() {
 
    }
    void OnDisable(){
        udp.Close();
    }
    public void AllMessageHandler(OscMessage oscMessage){//These functions are called when messages are received Access values via: oscMessage.Values[0], oscMessage.Values[1], etc Debug.Log(msgString);
		signal = true;
        string msgString = Osc.OscMessageToString(oscMessage); //the message and value combined
        string msgAddress = oscMessage.Address; //the message address
//        if (msgAddress == "/muse/elements/touching_forehead")
//            s0 = (int)oscMessage.Values[0]; // 0 = not touch
//        if (msgAddress == "/muse/elements/horseshoe") { 
//            s1 = (float)oscMessage.Values[0];
//        }
//        if (msgAddress == "/muse/elements/alpha_relative"){
//                c1 = (float)oscMessage.Values[0];
//        }

		if (msgAddress == "/muse/elements/blink")
			blink = (int)oscMessage.Values [0];

		if (msgAddress == "/muse/acc") {
			acc0 = (float)oscMessage.Values [0];
			acc1 = (float)oscMessage.Values [1];
			acc2 = (float)oscMessage.Values [2]; // left & right
		}

		if (msgAddress == "/muse/elements/alpha_relative") {
//			Debug.Log ("Alpha");
			alpha = (float)oscMessage.Values [0];
		}

		if (msgAddress == "/muse/elements/beta_relative") {
			beta = (float)oscMessage.Values [0];
		}

		if (msgAddress == "/muse/elements/is_good") {
			float conn0 = (float)oscMessage.Values[0];
			float conn1 = (float)oscMessage.Values[1];
			float conn2 = (float)oscMessage.Values[2];
			float conn3 = (float)oscMessage.Values[3];
			conn = conn0 + conn1 + conn2 + conn3;
		}
    }
}