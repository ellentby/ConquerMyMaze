using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NCMB;

public enum UnitType{gound, wall, entrance, exit};

public class MazeMaker : MonoBehaviour {
	public GameObject UnitPrefab;
	public GameObject SurroundingPrefab;
	public GameObject Blueprint;
	public GameObject CreateButton;
	public Sprite blackSprite;
	public Sprite frameSprite;
	public Sprite enterSprite;

	int[] intMaze;

	int unitRowNumber = 10;//including surrounding
	float unitWidth = 0f;
	float left = 0f;
	float bottom = 0f;

	// Use this for initialization
	void Start () {
		initIntMaze ();
		createDefaultBlueprint ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void initIntMaze(){
		intMaze = new int[unitRowNumber * unitRowNumber];
		for (int i = 0; i < unitRowNumber * unitRowNumber; i++) {
			intMaze [i] = (int)UnitType.gound;
		}
	}

	void createDefaultBlueprint(){
		int i = 0;
		int j = 0;
		//surrounding-bottom
		for (j = 0; j < unitRowNumber; j++) {
			GameObject created = Instantiate (SurroundingPrefab);
			if (j == 1) {
				created.GetComponent<Image> ().sprite = enterSprite;
				intMaze [i * unitRowNumber + j] = (int)UnitType.entrance;
			} else {
				created.GetComponent<Image> ().sprite = blackSprite;
				intMaze [i * unitRowNumber + j] = (int)UnitType.wall;
			}
			created.transform.name = "surrounding-" + i + "-" + j;
			created.GetComponent<RectTransform> ().position = getUnitPosition(i,j);
			created.GetComponent<RectTransform> ().sizeDelta = new Vector2 (UnitWidth,UnitWidth);
			created.transform.parent = Blueprint.transform;
		}
		//surrounding-top
		i = unitRowNumber - 1;
		for (j = 0; j < unitRowNumber; j++) {
			GameObject created = Instantiate (SurroundingPrefab);
			if (j == unitRowNumber - 2) {
				created.GetComponent<Image> ().sprite = enterSprite;
				intMaze [i * unitRowNumber + j] = (int)UnitType.exit;
			} else {
				created.GetComponent<Image> ().sprite = blackSprite;
				intMaze [i * unitRowNumber + j] = (int)UnitType.wall;
			}
			created.transform.name = "surrounding-" + i + "-" + j;
			created.GetComponent<RectTransform> ().position = getUnitPosition(i,j);
			created.GetComponent<RectTransform> ().sizeDelta = new Vector2 (UnitWidth,UnitWidth);
			created.transform.parent = Blueprint.transform;
		}

		//surrounding-left
		j = 0;
		for (i = 0; i < unitRowNumber; i++) {
			intMaze [i * unitRowNumber + j] = (int)UnitType.wall;
			GameObject created = Instantiate (SurroundingPrefab);
			created.GetComponent<Image> ().sprite = blackSprite;
			created.transform.name = "surrounding-" + i + "-" + j;
			created.GetComponent<RectTransform> ().position = getUnitPosition(i,j);
			created.GetComponent<RectTransform> ().sizeDelta = new Vector2 (UnitWidth,UnitWidth);
			created.transform.parent = Blueprint.transform;
		}

		//surrounding-right
		j = unitRowNumber - 1;
		for (i = 0; i < unitRowNumber; i++) {
			intMaze [i * unitRowNumber + j] = (int)UnitType.wall;
			GameObject created = Instantiate (SurroundingPrefab);
			created.GetComponent<Image> ().sprite = blackSprite;
			created.transform.name = "surrounding-" + i + "-" + j;
			created.GetComponent<RectTransform> ().position = getUnitPosition(i,j);
			created.GetComponent<RectTransform> ().sizeDelta = new Vector2 (UnitWidth,UnitWidth);
			created.transform.parent = Blueprint.transform;
		}


		for (i = 1; i < unitRowNumber-1; i++) {
			for (j = 1; j < unitRowNumber-1; j++) {
				GameObject created = Instantiate (UnitPrefab);
				created.transform.name = "unit-" + i + "-" + j;
				created.GetComponent<RectTransform> ().position = getUnitPosition(i,j);
				created.GetComponent<RectTransform> ().sizeDelta = new Vector2 (UnitWidth,UnitWidth);
				created.transform.parent = Blueprint.transform;
			}
		}
	}

	Vector3 getUnitPosition(int row, int column){
		return new Vector3 (Left + column * unitWidth, Bottom + row * unitWidth, 0);
	}

	//init left, bottom, unitWidth;
	void initLocationInfo(){
		if (unitRowNumber == 0) {
			Debug.LogError ("maze width is not set!");
			return;
		}
		float ruler;
		if (Screen.width < Screen.height - (CreateButton.GetComponent<RectTransform>().position.y + CreateButton.GetComponent<RectTransform>().sizeDelta.y/2)) {
			ruler = Screen.width;
		} else {
			ruler = Screen.height - (CreateButton.GetComponent<RectTransform>().position.y + CreateButton.GetComponent<RectTransform>().sizeDelta.y/2);
		}
		unitWidth = ruler / (unitRowNumber + 1);
		bottom = Screen.height - unitWidth * unitRowNumber;
		left =  Screen.width/2 - unitWidth * (unitRowNumber/2-0.5f);
	}

	#region Click Event
	public void OnUnit(GameObject unit){
		string[] splits = unit.name.Split (new char[]{'-'});
		int row = int.Parse(splits [1]);
		int column = int.Parse (splits[2]);
		int index = row * unitRowNumber + column;

		if (unit.GetComponent<Image> ().sprite == blackSprite) {
			unit.GetComponent<Image> ().sprite = frameSprite;
			intMaze [index] = (int)UnitType.gound;
		} else if(unit.GetComponent<Image> ().sprite == frameSprite){
			unit.GetComponent<Image> ().sprite = blackSprite;
			intMaze [index] = (int)UnitType.wall;
		}
	}

	public  void OnCreate(){
		//saveMaze ();

		//保存したオブジェクトのobjectIdをもとに取得を行う
		NCMBObject obj2 = new NCMBObject ("Maze");
		obj2.ObjectId = "939mde6GWwbdzqtA";
		obj2.FetchAsync ((NCMBException e) => {        
			if (e != null) {
				//エラー処理
			} else {
				//成功時の処理
				Configuration.maze = obj2;
				Application.LoadLevel("Maze");
			}               
		});
	}
	#endregion

	void saveMaze(){
		NCMBObject obj = new NCMBObject ("Maze");
		obj.Add ("maker", "ellen");
		obj.Add ("width", unitRowNumber);
		obj.Add ("maze", intMaze);
		obj.SaveAsync ((NCMBException e) => {      
			if (e != null) {
				//エラー処理
				Debug.LogError("Save maze record failed");
			} else {
				//成功時の処理

			}                   
		});
	}

	#region getter setter
	float UnitWidth{
		get{
			if (unitWidth == 0) {
				initLocationInfo ();
			}
			return unitWidth;
		}
	}

	float Bottom{
		get{
			if (bottom == 0) {
				initLocationInfo ();
			}
			return bottom;
		}
	}

	float Left{
		get{
			if (left == 0) {
				initLocationInfo ();
			}
			return left;
		}
	}
	#endregion

	void printArray(UnitType[] array){
		string result = "";
		for (int i = 0; i < array.Length; i++) {
			result += (int)array [i];
		}
		Debug.Log (result);
	}
}

