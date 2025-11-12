using ArgusGodotVisualizer;
using Godot;
using System;

public partial class ThroughputLabel : Label
{
	public ulong TotalBytesReceived = 0;

	public override void _Process(double delta)
	{
		base._Process(delta);
		ArgusCoreWrapper.UDPNetwork.OnPacketReceived += ((ArgusReduxCore.INetworkMessage message, System.Net.IPEndPoint remoteEndPoint) => {
			Callable.From(() => UDPNetwork_OnPacketReceived(message, remoteEndPoint)).CallDeferred();
		});
	}

	private void UDPNetwork_OnPacketReceived(ArgusReduxCore.INetworkMessage message, System.Net.IPEndPoint remoteEndPoint)
    {
        TotalBytesReceived += message.Length;
        Text = $"RX Throughput: {TotalBytesReceived} bytes";
	}
}
