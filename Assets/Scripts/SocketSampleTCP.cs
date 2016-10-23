using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class SocketSampleTCP : MonoBehaviour 
{
	private Socket m_listener = null;
	private State m_state;

	// 状態定義
	private enum State
	{
		SelectHost = 0,
		StartListener,
		AcceptClient,
		ServerCommunication,
		StopListener,
		ClientCommunication,
		Endcommunication,
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// TCP待受開始
	void StartListener (int port)
	{
		Debug.Log("Start server communication.");

		m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		m_listener.Bind(new IPEndPoint(IPAddress.Any, port));

		m_listener.Listen(1);

		m_state = State.AcceptClient;
		
	}
	
}
