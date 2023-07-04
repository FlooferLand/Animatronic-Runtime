namespace Project;
using Godot;
using System.Collections.Generic;

public static class Utils {
	public static RandomNumberGenerator random = new RandomNumberGenerator();

	public static T ListRandom<T>(List<T> list) =>
		list[random.RandiRange(0, list.Count-1)];

	public static Vector2 CalculateRatio(Vector2 res, float factor = 0.5f) {
		res.X = (res.X / res.Y) * factor;
		res.Y = factor;
		return res;
	}

	public static void ClearConsole() {
		GD.Print(new string('\n', 20));
	}
}
