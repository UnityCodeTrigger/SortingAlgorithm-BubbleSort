using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingAlgorithmHandler : MonoBehaviour
{
    [Header("Settings")]
    [Range(15,50)]
    public int numberOfLines = 5;

    List<Transform> objects;
    List<float> objectsValues;
    int index = 0;
    int numberOfSteps;
    int numberOfSwaps;
    //Visualizer variables
    float time = 0;
    bool toggleAutomatic;
    bool toggleSound = true;
    //Audio
    AudioSource source;

    //Inicialize the scene
    private void Start()
    {
        objects = new List<Transform>();
        objectsValues = new List<float>();

        source = GetComponent<AudioSource>();

        GenerateLines();
    }
    //Create next step button
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 40, 75, 30), "Next step"))
        {
            toggleAutomatic = false;
            NextStep();
        }

        if (GUI.Button(new Rect(10, 90, 75, 30), "Generate"))
        {
            toggleAutomatic = false;
            numberOfSteps = 0;
            numberOfSwaps = 0;

            if (numberOfLines != transform.childCount)
                ResetScene();
            else
                HandleChangeLineValues();
            
        }

        toggleAutomatic = GUI.Toggle(new Rect(100, 40, 75, 30), toggleAutomatic, "Automatic");
        toggleSound = GUI.Toggle(new Rect(100, 90, 75, 30), toggleSound, "Sound");

        GUI.Label(new Rect(10, 10, 75, 30), "Steps:" + numberOfSteps);
        GUI.Label(new Rect(100, 10, 75, 30), "Swaps:" + numberOfSwaps);

       numberOfLines = (int)GUI.HorizontalSlider(new Rect(175, 10, 100, 30), numberOfLines, 15, 50);
    }
    //Automatic mode
    private void Update()
    {
        if (!toggleAutomatic)
            return;

        if (time <= 0)
        {
            time = 0.01f;
            NextStep();
        }
        else
            time -= Time.deltaTime;
    }

    //Generate starting lines
    void GenerateLines()
    {
        for (int i = 0; i < numberOfLines; i++)
        {
            CreateNewObject();
        }
    }
    //Create objects in scene
    void CreateNewObject()
    {
        //Create new gameObject in scene
        GameObject obj = new GameObject();
        obj.transform.parent = transform;
        //Change the position y to screen bottom, change the x position to screen edge
        obj.transform.position = new Vector3(
            -SortingAlgorithm.ScreenWorldSize().x + ((objects.Count + 0.5f) * ((SortingAlgorithm.ScreenWorldSize().x * 2) / numberOfLines)),    //Align to center
            -SortingAlgorithm.ScreenWorldSize().y,
            -10);

        obj.name = objects.Count.ToString();

        CreateNewLine(obj);

        objects.Add(obj.transform);
    }
    //Add LineRenderer component to generated objects (called in GenerateLines())
    void CreateNewLine(GameObject obj)
    {
        //Add line renderer and set random 
        LineRenderer line = obj.AddComponent<LineRenderer>();

        line.material = SortingAlgorithm.lineMaterial(Color.white);
        //Set size to .5f
        line.SetWidth(.5f, .5f);

        ChangeLinesValues(obj);
    }
    //Created for regenerate lines (Called in OnGUI generate button)
    public void HandleChangeLineValues()
    {
        objectsValues.Clear();
        for (int i = 0; i < numberOfLines; i++)
        {
            ChangeLinesValues(objects[i].gameObject);
        }
    }

    void ResetScene()
    {
        objects.Clear();
        objectsValues.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GenerateLines();
    }

    //Change line values not visuals
    void ChangeLinesValues(GameObject obj)
    {
        //Get random number
        float value = Random.Range(1f, SortingAlgorithm.ScreenWorldSize().y * 2);
        //Change values to lists
        objectsValues.Add(value);
        //Update line positions values
        UpdateLineValues(obj, value);
    }
    //Update visuals
    void UpdateLineValues(GameObject obj,float value)
    {
        LineRenderer line = obj.GetComponent<LineRenderer>();

        //Set Y size and position
        line.SetPosition(0, (Vector2)line.transform.position);
        line.SetPosition(1, new Vector2(line.transform.position.x, value - SortingAlgorithm.ScreenWorldSize().y));
    }
    //Visuals
    void HighlightLines(int i)
    {
        objects[i].GetComponent<LineRenderer>().material = SortingAlgorithm.lineMaterial(Color.green);
    }
    //Audio
    void PlaySound()
    {
        source.pitch = ((float)index / (float)objects.Count) + 1;
        source.PlayOneShot(source.clip);
    }

    public void NextStep()
    {
        BubbleSort();
    }
    //Swap a object to b object
    void SwapObjects(int aIndex, int bIndex)
    {
        Transform aObj = objects[aIndex];
        Transform bObj = objects[bIndex];

        float ia = objectsValues[aIndex];
        float ib = objectsValues[bIndex];
        //Cambia los valores (no mueve el objeto)
        objectsValues[bIndex] = ia;
        objectsValues[aIndex] = ib;

        UpdateLineValues(aObj.gameObject, objectsValues[aIndex]);
        UpdateLineValues(bObj.gameObject, objectsValues[bIndex]);

        if(toggleSound) PlaySound();
        numberOfSwaps++;
    }
    //Sort Algorithm
    void BubbleSort()
    {
        if (numberOfSteps > Mathf.Pow(objects.Count, 2))
            return;

        if (index < objects.Count - 1)
        {
            if (objectsValues[index] > objectsValues[index + 1])
                SwapObjects(index,index + 1);

            index++;
            numberOfSteps++;
        }
        else
            index = 0;
    }

}