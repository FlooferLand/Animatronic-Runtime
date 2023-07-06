namespace Project;
using Godot;
using System;
using System.Collections.Generic;

public partial class AssetLoader : Node {
	public readonly Dictionary<String, FootstepSoundContainer> FootstepSounds = new();
	
	#region Private methods
	public override void _Ready() {
		LoadFootsteps();
	}
	#endregion

	private void LoadFootsteps() {
		const string footstepsPath = "res://sound/footsteps/";
		foreach (string surfaceType in DirAccess.GetDirectoriesAt(footstepsPath)) {
			var steps = new List<AudioStream>();
			var slips = new List<AudioStream>();
			
			string surfaceDir = footstepsPath.PathJoin(surfaceType);
			foreach (string filename in DirAccess.GetFilesAt(surfaceDir)) {
				if (filename.EndsWith(".import")) continue;

				var stream = GD.Load<AudioStream>(surfaceDir.PathJoin(filename));
				if (filename.StartsWith("step"))
					steps.Add(stream);
				else if (filename.StartsWith("slip"))
					slips.Add(stream);
			}
			// Adding the new sound
			FootstepSounds.Add(surfaceType, new FootstepSoundContainer(steps, slips));
		}
	}
}

public record FootstepSoundContainer(
	List<AudioStream> steps,
	List<AudioStream> slips
);