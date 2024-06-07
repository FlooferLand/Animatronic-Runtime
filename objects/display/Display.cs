namespace Project;
using Godot;
using System.Collections.Generic;

// Made with the help of Chevifier's tutorial:
// https://youtu.be/q0-jtBFrVgE

[Tool]
public partial class Display : Node3D, IBaseInteractable, ILookDetector {
	// Nodes
	[GetNode("Viewport")]					private SubViewport viewport;
	[GetNode("Mesh")]						private MeshInstance3D mesh;
	[GetNode("InteractionArea")]			private Area3D interactionArea;
	[GetNode("InteractionArea/Collision")]	private CollisionShape3D interactionAreaCollider;
	[GetNode("ClickPlayer")]				private AudioStreamPlayer3D clickPlayer;
	private Control userInterface;
	private Control UserInterface {
		get => userInterface;
		set { userInterface = value; Setup(); }
	}
	
	// Export variables
	[Export] public bool Transparent = false;
	
	// Public variables
	public StandardMaterial3D Material;
	
	// Private variables
	private bool isPlayerLooking;
	private Vector3 lastMousePos3D = Vector3.Zero;
	private Vector2 lastMousePos2D = Vector2.Zero;
	private Vector2 holdStartPos = Vector2.Zero;
	
	// Methods
	private void Setup() {
		if (UserInterface == null || viewport == null) return;
		if (Material != null) {
			GD.Print("Useless call to Setup!");
			return;
		}
		
		// Setting up the viewport
		viewport.Size = (Vector2I) userInterface.Size;
		viewport.TransparentBg = Transparent;
		if (!Engine.IsEditorHint())
			userInterface.Reparent(viewport);

		// Setting up the texture
		var texture = viewport.GetTexture();
		
		// Setting up the material
		Material = new StandardMaterial3D();
		Material.ResourceLocalToScene = true;
		Material.AlbedoTexture = texture;
		Material.EmissionTexture = texture;
		Material.EmissionEnabled = true;
		if (Transparent && !Engine.IsEditorHint())
			Material.Transparency = BaseMaterial3D.TransparencyEnum.Max;
		
		// Setting up the mesh
		mesh.MaterialOverride = Material;
		mesh.Mesh = new QuadMesh();
		((QuadMesh)mesh.Mesh).Size = Utils.CalculateRatio(userInterface.Size);
		
		// Interaction mesh
		interactionAreaCollider.Shape = new ConvexPolygonShape3D();
		var arrayMesh = new ArrayMesh();
		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, mesh.Mesh.SurfaceGetArrays(0));
		((ConvexPolygonShape3D)interactionAreaCollider.Shape).Points = arrayMesh.GetFaces();
	}
    
	public void Interact(RayCast3D ray, InteractState state, InteractButton button) {
		var mousePos3D = ray.GetCollisionPoint();
		
		// Fixes scaling/translation issues if the area moves around (?)
		mousePos3D = interactionArea.GlobalTransform.AffineInverse() * mousePos3D;
		lastMousePos3D = mousePos3D;
		
		// Converting the 3D position into a 2D one
		var vMousePos = new Vector2(mousePos3D.X, -mousePos3D.Y);
		
		// Changing positions to the display/viewport space (https://youtu.be/q0-jtBFrVgE?t=595)
		var meshSize = ((QuadMesh)mesh.Mesh).Size;
		vMousePos += meshSize / 2;
		vMousePos /= meshSize;
		vMousePos *= viewport.Size;
		
		// TODO: Add in support for text input/focusing
		
        // Some early state stuff
        switch (state) {
	        case InteractState.Press:
		        holdStartPos = vMousePos;
		        clickPlayer.Play();
		        break;
	        case InteractState.Release:
		        holdStartPos = Vector2.Zero;
		        break;
        }
		
		// Creating a mouse event
		switch (state) {
			case InteractState.Press:
			case InteractState.Release:
				var buttonEvent = new InputEventMouseButton();
				buttonEvent.Position = vMousePos;
				buttonEvent.GlobalPosition = vMousePos;
				switch (button) {
					case InteractButton.Primary:
						buttonEvent.ButtonIndex = MouseButton.Left;
						break;
					case InteractButton.Secondary:
						buttonEvent.ButtonIndex = MouseButton.Right;
						break;
				}
				buttonEvent.Pressed = (state == InteractState.Press);
				viewport.PushInput(buttonEvent);
				break;
			case InteractState.Hold:
				// TODO: Implement InputEventMouseMotion
				break;
		}
		
		// Remembering the last mouse pos
		lastMousePos2D = vMousePos;
	}
	
	public void LookDetector(bool isLooking, RayCast3D ray) {
		isPlayerLooking = isLooking;
	}
	
	// Built-in methods
	public override void _Ready() {
		// Getting the UI node
		if (GetChildOrNull<Control>(GetChildCount() - 1) is {} child)
			UserInterface = child;

		ChildEnteredTree += _OnChildEnteredTree;
		ChildExitingTree += _OnChildExitedTree;
		
		// Annoying config warnings >:C
		UpdateConfigurationWarnings();
	}

	private void _OnChildEnteredTree(Node child) {
		if (child is Control control)
			UserInterface = control;
		UpdateConfigurationWarnings();
	}
	
	private void _OnChildExitedTree(Node _) {
		UserInterface = null;
		UpdateConfigurationWarnings();
	}

	public override string[] _GetConfigurationWarnings() {
		var warnings = new List<string>();
		
		if (GetChildOrNull<Control>(GetChildCount()-1) == null)
			warnings.Add("Must have a UI node as a child");
		
		return warnings.ToArray();
	}
}
