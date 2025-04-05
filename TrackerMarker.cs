using ArgusReduxCore;
using ArgusReduxCore.NetworkUDP;
using Godot;
using System;
using System.Linq;

public partial class TrackerMarker : MeshInstance3D
{
	public Tracker Tracked;

	public override void _Ready()
	{
		Tracked.OnUpdated += Updated;
	}

	private void Updated(SensorDataMessage msg)
	{
		if (msg.IMUData.Count > 0)
		{
			IMUSample IMU = msg.IMUData.Last();
			Position = new Vector3(IMU.Accel[0] / 1024, IMU.Accel[1] / 1024, IMU.Accel[2] / 1024);
		}
	}
}
