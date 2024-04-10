namespace Project;
using Godot;
using System;
using System.Collections.Generic;

// FIXME: UNFINISHED
// 3D line system made for cables, tubes, ropes, etc.

public record ConduitSegmentNodes(Node3D Parent,  Path3D Path, CsgPolygon3D Mesh);
public class ConduitSegment {
	public Vector3 Start, End;
	public ConduitSegmentNodes Nodes = null;
	public ConduitSegment(Vector3 start, Vector3 end) {
		Start = start;
		End = end;
	}
}

public partial class Conduit : Node3D {
	// Parameters
	[Export] public float CircleRadius = 0.03f;
	[Export] public float CircleResolution = 45;
	private ConduitSegment[] segments;

	private void Create() {
		foreach (var child in GetChildren()) {
			child.QueueFree();
		}
		foreach (var point in segments) {
			var parent = new Node3D();
			var path = new Path3D();
			path.Curve.AddPoint(point.Start);
			path.Curve.AddPoint(point.End);
			parent.AddChild(path);
			AddChild(parent);
			
			var mesh = new CsgPolygon3D();
			mesh.Mode = CsgPolygon3D.ModeEnum.Path;
			mesh.PathNode = path.GetPath();
			parent.AddChild(mesh);

			point.Nodes = new ConduitSegmentNodes(parent, path, mesh);
		}
	}

	#region Segment setters/getters
	public ConduitSegment GetSegment(int i) => segments[i];
	public void AddSegment(ConduitSegment segment) {
		
	}
	public void RemoveSegment(ConduitSegment segment) {
		
	}
	public void ChangeSegment(int i, Vector3 start, Vector3 end) {
		if (segments.Length < i) return;
		segments[i].Start = start;
		segments[i].End = end;
	}
	#endregion
	
	public override void _Process(double delta) {
		
	}

	/// Gets the profile for a circle based on resolution and radius
	private Vector2[] GetProfile() {
		var circle = new List<Vector2>();
		for (int degree = 0; degree < CircleResolution; degree++) {
			var x = CircleRadius * Mathf.Sin(Mathf.Pi * 2 * degree / CircleResolution);
			var y = CircleRadius * Mathf.Cos(Mathf.Pi * 2 * degree / CircleResolution);
			var coords = new Vector2(x, y);
			circle.Add(coords);
		}
		return circle.ToArray();
	}
}
