using ArgusReduxCore;
using Godot;
using System;
using System.Collections.Generic;

public partial class TrackerVisualizer : Node
{
	[Export]
	public PackedScene TrackerPrefab = null;

	private TrackerManager tm;
	private List<TrackerMarker> markers = new();

	public override void _Ready()
	{
		tm = new TrackerManager();
		tm.OnTrackerAdded += TrackerAdded;
	}

	private void TrackerAdded(Tracker tracker)
	{
		TrackerMarker marker = TrackerPrefab.Instantiate<TrackerMarker>();
		marker.Tracked = tracker;
		GetTree().CurrentScene.AddChild(marker);

		markers.Add(marker);
	}
}
