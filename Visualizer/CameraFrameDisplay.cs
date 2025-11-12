using Godot;
using System;
using ArgusReduxCore;
using ArgusGodotVisualizer;

public partial class CameraFrameDisplay : TextureRect
{
	private Tracker _tracker = null;
	private ulong? trackerID = null;

	public override void _Ready()
	{
		ArgusCoreWrapper.TrackerManager.OnTrackerAdded += OnTrackerAdded;
	}

	private void OnTrackerAdded(Tracker tracker)
	{
		// Assuming we only care about the first tracker for this display.
		// You might want a more sophisticated way to choose a tracker.
		if (!trackerID.HasValue)
		{
			trackerID = tracker.ID;
        }
		if (_tracker.ID == trackerID)
		{
			_tracker.OnFrameReceived -= OnFrameReceived;
            _tracker = tracker;
			_tracker.OnFrameReceived += OnFrameReceived;
		}
	}

	private void OnFrameReceived(byte[] frameData)
	{
		CallDeferred(nameof(UpdateTexture), frameData);
	}

	private void UpdateTexture(byte[] frameData)
	{
		var image = new Image();
		var error = image.LoadJpgFromBuffer(frameData);
		if (error != Error.Ok)
		{
			GD.PrintErr($"Error loading JPG from buffer: {error}");
			return;
		}

		var texture = ImageTexture.CreateFromImage(image);
		this.Texture = texture;
	}
}
