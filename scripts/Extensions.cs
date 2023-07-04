namespace Project;
using Godot;
using System.Diagnostics.Contracts;

public static class Extensions {
	#region Vector3 extensions
	// I could make extensions for Node3D/Character3D/etc (if .Position or .Velocity is being set)
	// Having a .PositionSetX would be shorter.
	[Pure] public static Vector3 WithX(this Vector3 self, float x) => new(x, self.Y, self.Z);
	[Pure] public static Vector3 WithY(this Vector3 self, float y) => new(self.X, y, self.Z);
	[Pure] public static Vector3 WithZ(this Vector3 self, float z) => new(self.X, self.Y, z);
	
	[Pure] public static Vector3 WithX(this Vector3 self, double x) => new((float)x, self.Y, self.Z);
	[Pure] public static Vector3 WithY(this Vector3 self, double y) => new(self.X, (float)y, self.Z);
	[Pure] public static Vector3 WithZ(this Vector3 self, double z) => new(self.X, self.Y, (float)z);

	/// Sums up the values of a Vector <i>(X + Y + Z)</i>,
	/// runs <c>Abs</c> and <c>Normalized</c> on them, and remaps the values to a <i>0 to 1</i> scale.
	public static float Combined(this Vector3 self) {
		var vector = self.Abs().Normalized();
		var combined = (vector.X + vector.Y + vector.Z);
		return combined / 3f;
	}
	#endregion
}
