using UnityEngine;
using System.Collections;
using NCMB;

public class MazeGenerator : MonoBehaviour {
	public GameObject player;
	public GameObject WallPrefab;
	public GameObject Maze;
	public GameObject UnitLight;
	float left = 0;
	float bottom = 0;
	float unitWidth = 0;
	// Use this for initialization
	void Start () {
		unitWidth = WallPrefab.GetComponent<MeshFilter> ().mesh.bounds.size.x * WallPrefab.transform.localScale.x;
		generateMaze();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void generateMaze(){
		ArrayList maze = (ArrayList)Configuration.maze ["maze"];
		int width = int.Parse(Configuration.maze ["width"].ToString());
		for(int i=0; i<width; i++){
			for (int j = 0; j < width; j++) {
				int index = width * i + j;
				if (int.Parse (maze [index].ToString ()) == (int)UnitType.wall) {
					GameObject wallUnit = Instantiate (WallPrefab);
					wallUnit.transform.parent = Maze.transform;
					wallUnit.transform.position = new Vector3 (left + unitWidth * i, WallPrefab.transform.position.y, bottom + unitWidth * j);
					wallUnit.name = "wall-" + i + "-" + j;
				} else if (int.Parse (maze [index].ToString ()) == (int)UnitType.entrance) {
					player.transform.position = new Vector3 (left + unitWidth * i, WallPrefab.transform.position.y, bottom + unitWidth * j);
					GameObject wallUnit = Instantiate (WallPrefab);
					wallUnit.transform.parent = Maze.transform;
					wallUnit.transform.position = new Vector3 (left + unitWidth * (i-1), WallPrefab.transform.position.y, bottom + unitWidth * j);
					wallUnit.name = "entrance";
				}else if (int.Parse (maze [index].ToString ()) == (int)UnitType.gound) {
					GameObject light = Instantiate (UnitLight);
					light.transform.parent = Maze.transform;
					light.transform.position = new Vector3 (left + unitWidth * i, UnitLight.transform.position.y, bottom + unitWidth * j);
					light.name = "light-" + i + "-" + j;
				}
			}
		}
	}
}
