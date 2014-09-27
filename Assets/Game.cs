using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public Orienter Orienter;

	private IDictionary<int, Level> levels;

	// Use this for initialization
	void Start () {
		this.levels = LoadLevels ();

		this.Orienter.LoadLevel ("1", this.levels [1].Target);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.Orienter.IsNearTarget()) {
			this.Orienter.LoadLevel("1", this.levels[1].Target);
		}
	}

	private static IDictionary<int, Level> LoadLevels()
	{
		var levelData = Resources.Load<TextAsset> ("LevelData");
		var lines = levelData.text.Split ('\n');
		var levels = new Dictionary<int, Level> (lines.Length);
		foreach (var line in lines)
		{
			if (line.Length > 0)
			{
				var split = line.Split('\t');
				int level = int.Parse(split[0]);
				var target = new Matrix4x4()
				{
					m00 = float.Parse(split[1]),
					m01 = float.Parse(split[2]),
					m02 = float.Parse(split[3]),
					m03 = float.Parse(split[4]),
					m10 = float.Parse(split[5]),
					m11 = float.Parse(split[6]),
					m12 = float.Parse(split[7]),
					m13 = float.Parse(split[8]),
					m20 = float.Parse(split[9]),
					m21 = float.Parse(split[10]),
					m22 = float.Parse(split[11]),
					m23 = float.Parse(split[12]),
					m30 = float.Parse(split[13]),
					m31 = float.Parse(split[14]),
					m32 = float.Parse(split[15]),
					m33 = float.Parse(split[16]),
				};
				
				levels.Add(level, new Level() { Target = target });
			}
		}

		return levels;
	}

	private struct Level
	{
		public Matrix4x4 Target { get; set; }
	}
}
