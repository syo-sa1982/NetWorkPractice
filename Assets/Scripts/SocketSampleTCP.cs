using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class SocketSampleTCP : MonoBehaviour 
{

	// 接続先のポート番号.
	private string m_address = "";
	private const int m_port = 50765;
	private Socket m_listener = null;
	private Socket m_socket = null;
	private bool m_isConnected = false;
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
		// ソケット生成
		m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		// ポート番号割当
		m_listener.Bind(new IPEndPoint(IPAddress.Any, port));
		// 待受開始
		m_listener.Listen(1);
		m_state = State.AcceptClient;
		
	}

	void AcceptClient()
	{
		if (m_listener != null && m_listener.Poll(0, SelectMode.SelectRead)) {
			m_socket = m_listener.Accept();
			Debug.Log("[TCP]Connected from client.");
			m_isConnected = true;
		}
	}
	
	void ClientProcess()
	{
		// サーバーへ接続
		m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_socket.NoDelay = true;
		m_socket.Connect(m_address, m_port);

		// メッセージ送信
		byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Hello, this is client.");
		m_socket.Send(buffer, buffer.Length, SocketFlags.None);
	}
}
