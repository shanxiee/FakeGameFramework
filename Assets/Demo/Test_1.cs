using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameFramework;

public class Test_1 : MonoBehaviour {


	// Use this for initialization
	void Start () {
		GameFrameworkLinkedList<int> int_list = new GameFrameworkLinkedList<int>();
		int_list.Add(10);
		int_list.Add(99);
		var mid_node =  int_list.Find(99);
		int_list.AddBefore(mid_node,0);

		foreach(var val in int_list){
			Debug.Log("show_val "+ val);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private static void Display(LinkedList<string> words, string test)
	{
		Debug.Log(test);
		foreach (string word in words)
		{
			Debug.Log(word + " ");
		}
	}

}
