using ArgusGodotVisualizer;
using Godot;
using System;
using System.Collections.Generic;

public partial class ThroughputLabel : Label
{
	private readonly Queue<(double time, int bytes)> _receivedBytesHistory = new();
	private const double AveragingWindowSeconds = 5.0;
	private ulong _totalBytesInWindow = 0;

	public override void _Ready()
	{
		base._Ready();
		// The subscription is moved to _Ready to avoid subscribing on every frame.
		if (ArgusCoreWrapper.UDPNetwork != null)
		{
			ArgusCoreWrapper.UDPNetwork.OnPacketReceived += OnPacketReceived;
		}
	}

	public override void _ExitTree()
	{
		if (ArgusCoreWrapper.UDPNetwork != null)
		{
			ArgusCoreWrapper.UDPNetwork.OnPacketReceived -= OnPacketReceived;
		}
		base._ExitTree();
	}

	private void OnPacketReceived(ArgusReduxCore.INetworkMessage message, System.Net.IPEndPoint remoteEndPoint)
	{
		// Defer the execution to the main thread to ensure thread safety.
		Callable.From(() => UpdateWithNewPacket(message.Length)).CallDeferred();
	}

	private void UpdateWithNewPacket(int messageLength)
	{
		double currentTime = Time.GetTicksMsec() / 1000.0;
		var newEntry = (time: currentTime, bytes: messageLength);
		_receivedBytesHistory.Enqueue(newEntry);
		_totalBytesInWindow += (ulong)newEntry.bytes;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		double currentTime = Time.GetTicksMsec() / 1000.0;
		
		// Remove old entries from the queue
		while (_receivedBytesHistory.Count > 0 && (currentTime - _receivedBytesHistory.Peek().time) > AveragingWindowSeconds)
		{
			_totalBytesInWindow -= (ulong)_receivedBytesHistory.Dequeue().bytes;
		}

		// Calculate throughput
		double throughputBps = _totalBytesInWindow / AveragingWindowSeconds;

		// Update the label text
		Text = $"RX Throughput: {throughputBps:F2} B/s";
	}
}
