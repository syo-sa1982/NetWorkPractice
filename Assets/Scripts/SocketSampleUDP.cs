using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class SocketSampleUDP : MonoBehaviour 
{
	// 接続先のIPアドレス.
	private string			m_address = "";
	
	// 接続先のポート番号.
	private const int 		m_port = 50765;

	// 通信用変数
	private Socket			m_socket = null;

	// 状態.
	private State			m_state;

	// 状態定義
	private enum State
	{
		SelectHost = 0,
		CreateListener,
		ReceiveMessage,
		CloseListener,
		SendMessage,
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

	void SendMessage()
	{
		Debug.Log("[UDP]Start communication.");

		m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Hello, this is client.");
		
	}


}
