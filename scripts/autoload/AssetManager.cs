namespace Project;
using Godot;
using System.Collections.Generic;

// Loads and manages assets via magic means
// Should use paths from the Paths static class
public partial class AssetManager : Node {
    public static readonly Dictionary<string, FootstepSoundContainer> FootstepSounds = new();
	
    #region Private methods
    public override void _Ready() {
        LoadFootsteps();
    }
    #endregion

    private void LoadFootsteps() {
        foreach (string surfaceType in DirAccess.GetDirectoriesAt(Paths.FootstepPath)) {
            var steps = new List<AudioStream>();
            var slips = new List<AudioStream>();
			
            string surfaceDir = Paths.FootstepPath.PathJoin(surfaceType);
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
    List<AudioStream> Steps,
    List<AudioStream> Slips
);