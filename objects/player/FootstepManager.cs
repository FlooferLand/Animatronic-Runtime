namespace Project;
using Godot;
using System.Collections.Generic;

public partial class FootstepManager : Node3D {
	// Nodes
	[ExportGroup("Nodes")]
	[Export] private Player player;
	private BasicLoader loader;
	private RayCast3D raycast;
	private AudioStreamPlayer3D audio;
	private Timer timer;
	
	// Settings
	[ExportGroup("Settings")]
	[Export] public float VolumeBoost {
		get => volumeBoost;
		set { volumeBoost = value; audio.VolumeDb = (value * 0.25f); }
	}
	[Export] public float Frequency {
		get => frequency;
		set {frequency = value; timer.WaitTime = value; }
	}
	private float volumeBoost;
	private float frequency = 0.5f;
	
	// Signals
	[Signal] public delegate void StepEventHandler(float force);
	
	// Public variables
	public bool Slipping = false;
	public float InitialFrequency;
	public string CurrentMaterialName = "porcelain";

	// Private variables
	private AudioEffectLowPassFilter lowPassEffect;
	private float stepForce;

	#region Private methods
	public override void _Ready() {
		// Getting nodes
		loader = GetNode<BasicLoader>("/root/BasicLoader");
		raycast = GetNode<RayCast3D>("RayCast");
		audio = GetNode<AudioStreamPlayer3D>("AudioPlayer");
		timer = GetNode<Timer>("Timer");
		InitialFrequency = Frequency;
		
		// Getting audio effect
		lowPassEffect = AudioMixer.GetBus("PlayerSteps")
			.GetEffect<AudioEffectLowPassFilter>(0);
		
		// Initializing stuff
		timer.Timeout += OneShot;
		timer.WaitTime = Frequency;
		audio.VolumeDb = VolumeBoost;
	}
	#endregion
	
	#region Public methods
	public void Play() {
		timer.Start();
		Slipping = false;
		OneShot();
	}
	public void Stop() => timer.Stop();
	public bool IsPlaying() =>
		(!timer.IsStopped() || audio.Playing);

	public AudioStream GetFootstepSound() {
		FootstepSoundContainer soundContainer = loader.FootstepSounds[CurrentMaterialName];
		List<AudioStream> stream = (Slipping ? soundContainer.slips : soundContainer.steps);
		return Utils.ListRandom(stream);
	}
	
	// TODO: Add support for multiple sound materials
	public void OneShot() {
        // Immersion check
        if (player.Speed < 0.2f
        || !player.IsOnFloor()) {
	        timer.Stop();
	        return;
        }
		
		// Getting / Emitting stuff
		stepForce = 1f + VolumeBoost;
		EmitSignal(nameof(Step), stepForce);
		
		// Playing the audio
		audio.Stream = GetFootstepSound();
		audio.Play();
	}

	public void Jump() {
		EmitSignal(nameof(Step), player.JumpHeight / 2f);
	}
	#endregion
	
	#region Private methods
	public override void _Process(double delta) {
        // Making the footstep lowpass filter shift around based on the step force
        lowPassEffect.CutoffHz = Mathf.Lerp(
	        lowPassEffect.CutoffHz,
	        stepForce * 5000f,
	        4f * (float) delta
	    );
	}
	#endregion
}
