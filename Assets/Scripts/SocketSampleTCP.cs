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
		m_state = State.SelectHost;

		m_address = GetServerIPAddress();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (m_state) {
		case State.StartListener:
			StartListener();
			break;
		
		case State.AcceptClient:
			AcceptClient();
			break;

		case State.ServerCommunication:
			ServerCommunication();
			break;

		case State.StopListener:
			StopListener();
			break;

		case State.ClientCommunication:
			ClientProcess();
			break;

		default:
			break;

		}
	}

	// TCP待受開始
	void StartListener ()
	{
		Debug.Log("Start server communication.");
		// ソケット生成
		m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		// ポート番号割当
		m_listener.Bind(new IPEndPoint(IPAddress.Any, m_port));
		// 待受開始
		m_listener.Listen(1);
		m_state = State.AcceptClient;
		
	}

	// クライアントからの接続待ち
	void AcceptClient()
	{
		if (m_listener != null && m_listener.Poll(0, SelectMode.SelectRead)) {
			m_socket = m_listener.Accept();
			Debug.Log("[TCP]Connected from client.");
			m_isConnected = true;
		}
	}

	// クライアントからのメッセージ受信
	void ServerCommunication()
	{
		byte[] buffer = new byte[1400];
		int recvSize = m_socket.Receive(buffer, buffer.Length, SocketFlags.None);

		if (recvSize > 0) {
			string message = System.Text.Encoding.UTF8.GetString(buffer);
			Debug.Log(message);
			m_state = State.StopListener;
		}
	}

	// 待受終了
	void StopListener()
	{
		if (m_listener != null) {
			m_listener.Close();
			m_listener = null;
		}

		m_state = State.Endcommunication;

		Debug.Log("[TCP]End server communication.");
	}
	
	// クライアント側の処理
	void ClientProcess()
	{
		Debug.Log("[TCP]Start client communication.");

		// サーバーへ接続
		m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_socket.NoDelay = true;

		Debug.Log(m_address);
		m_socket.Connect(m_address, m_port);

		// メッセージ送信
		byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Hello, this is client.");
		m_socket.Send(buffer, buffer.Length, SocketFlags.None);

		// 切断
		m_socket.Shutdown(SocketShutdown.Both);
		m_socket.Close();

		Debug.Log("[TCP]End client communication.");
	}

	public string GetServerIPAddress()
	{
		string hostAddress = "";
		string hostname = Dns.GetHostName();

		IPAddress[] adList = Dns.GetHostAddresses(hostname);

		for (int i = 0; i < adList.Length; ++i) {
			string addr = adList[i].ToString();
			string [] c = addr.Split('.');

			if (c.Length == 4) {
				hostAddress = addr;
				break;
			}
		}

		return hostAddress;
	}


	void OnGUI()
	{
		if (m_state == State.SelectHost) {
			OnGUISelectHost();
		}
	}

	void OnGUISelectHost()
	{
		if (GUI.Button (new Rect (20,40, 150,20), "Launch server.")) {
			m_state = State.StartListener;
		}
		
		// クライアントを選択した時の接続するサーバのアドレスを入力します.
		m_address = GUI.TextField(new Rect(20, 100, 200, 20), m_address);
		if (GUI.Button (new Rect (20,70,150,20), "Connect to server")) {
			m_state = State.ClientCommunication;
		}	
	}
}
