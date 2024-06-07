using System.Collections.Generic;

namespace Project;
using Godot;

public partial class FootstepManager : Node {
    // Nodes
    [Export] private Player player;
    [GetNode("Audio")] private AudioStreamPlayer audio;
    [GetNode("Timer")] private Timer timer;
    
    // Signals
    [Signal] public delegate void StepEventHandler(float force);
    
    // Settings
    [ExportGroup("Settings")]
    [Export] public float VolumeBoost {
        get => volumeBoost;
        set {
            volumeBoost = value;
            if (audio != null) audio.VolumeDb = (value * 0.25f);
        }
    }
    [Export] public float Frequency {
        get => frequency;
        set {frequency = value; timer.WaitTime = value; }
    }
    private float volumeBoost;
    private float frequency = 0.5f;
    
    // Public variables
    public bool Slipping = false;
    public float InitialFrequency;
    public string CurrentMaterialName = "porcelain";

    // Private variables
    private AudioEffectLowPassFilter lowPassEffect;
    private float stepForce;
    private Vector3 lastVelocity;

    public override void _Ready() {
        InitialFrequency = Frequency;
		
        // Getting audio effect
        lowPassEffect = AudioMixer.GetBus("PlayerSteps")
            .GetEffect<AudioEffectLowPassFilter>(0);
		
        // Initializing stuff
        timer.Timeout += OneShot;
        timer.WaitTime = Frequency;
        audio.VolumeDb = VolumeBoost;
    }
    
    #region Public methods
    public void Play() {
        timer.Start();
        Slipping = false;
        OneShot();
    }
    public void Stop() {
        timer.Stop();
    }

    public bool IsPlaying() =>
        (!timer.IsStopped() || audio.Playing);

    public AudioStream GetFootstepSound() {
        FootstepSoundContainer soundContainer = AssetManager.FootstepSounds[CurrentMaterialName];
        List<AudioStream> stream = (Slipping ? soundContainer.Slips : soundContainer.Steps);
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
        
        // Player velocity
        float diff = (player.Velocity.Max(lastVelocity) - player.Velocity.Min(lastVelocity)).Length();
        lastVelocity = player.Velocity;
        Slipping = diff > 2.95f;
		
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
            stepForce * 6000f,
            10f * (float) delta
        );
    }
    #endregion
}
