using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Slider xSlider;
    public Slider ySlider;
    public Text xText;
    public Text yText;
    public ObjectController objectController;
    public MazeGenerator mazeGenerator;
    public GameObject prefab;

    public float x {
        set {
            xPosition = (int) value;
            xText.text = xPosition.ToString();
        }
    }

    public float y {
        set {
            yPosition = (int) value;
            yText.text = yPosition.ToString();
        }
    }

    private int xPosition;
    private int yPosition;

    void Start()
    {
        xSlider.maxValue = mazeGenerator.width - 1;
        ySlider.maxValue = mazeGenerator.height - 1;
        x = xSlider.value;
        y = ySlider.value;
    }

    public void spawnObject()
    {
        objectController.spawnObject(prefab, new ObjectController.Position(xPosition, yPosition));
    }

}
