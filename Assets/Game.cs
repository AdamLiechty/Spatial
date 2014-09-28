using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public Orienter Orienter;

	private IDictionary<int, Level> levels;
	private int currentLevel;

	// Use this for initialization
	void Start () {
		this.levels = LoadLevels ();

		currentLevel = 1;
		this.Orienter.LoadLevel (currentLevel.ToString(), this.levels[currentLevel].TargetRotation);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.Orienter.IsNearTarget()) {
			currentLevel++;
			this.Orienter.LoadLevel (currentLevel.ToString(), this.levels[currentLevel].TargetRotation);
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
				var targetRotation = new Quaternion(
					float.Parse (split[1]),
					float.Parse (split[2]),
					float.Parse (split[3]),
					float.Parse (split[4]));

				levels.Add(level, new Level() { TargetRotation = targetRotation });
			}
		}

		return levels;
	}

	private struct Level
	{
		public Quaternion TargetRotation { get; set; }
	}
}
