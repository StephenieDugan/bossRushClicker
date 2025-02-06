using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public Canvas canvas;
    public GameObject Scenery;
    public RectTransform Panel;
    public float canvasHeight;
    public float canvasHeightBottom; //for testing, i made it a variable to make sure for the frame of the phone.
    public float resultHeight1;
    public float resultHeight2;
    public float actualHeight1;
    public float actualHeight2;
    public int panelMovement;

    public void Start()
    {
        canvasHeight = canvas.gameObject.GetComponent<RectTransform>().rect.height;
        ClosePanel();
        
    }

    public void OpenPanel()
    {
        resultHeight1 = 0;
        resultHeight2 = 0;
        actualHeight1 = -canvasHeight / 2;
        actualHeight2 = -canvasHeight / 4;
        panelMovement = 1;

        Panel.anchoredPosition = new Vector2(0, 0);
        //-Scenery.sizeDelta = new Vector2(0, canvasHeight);
        //Scenery.anchoredPosition = new Vector2(0, 0);
    }

    public void ClosePanel()
    {
        resultHeight1 = -canvasHeight / 2;
        resultHeight2 = -canvasHeight / 4;
        actualHeight1 = 0;
        actualHeight2 = 0;

        Panel.anchoredPosition = new Vector2(0, -canvasHeight / 2);
        //-Scenery.sizeDelta = new Vector2(0, canvasHeight / 2);
        //Scenery.anchoredPosition = new Vector2(0, -canvasHeight / 4);
    }



    public void Update()
    {
        //var height = canvas.gameObject.GetComponent<RectTransform>().rect.height;
        //print(height);  //debug to figure out where the panel is

        if (panelMovement != 0)
        {
            if (panelMovement == 1)
            {
                print(actualHeight1);
                actualHeight1 += canvasHeight * Time.deltaTime;
                actualHeight2 += canvasHeight/2 * Time.deltaTime;
                if(actualHeight1 > resultHeight1)
                {
                    actualHeight1 = resultHeight1;
                    actualHeight2 = resultHeight2;
                    panelMovement = 0;
                }
            }else if (panelMovement == 2)
            {
                actualHeight1 -= canvasHeight * Time.deltaTime;
                actualHeight2 -= canvasHeight / 2 * Time.deltaTime;

                if (actualHeight1 < resultHeight1)
                {
                    actualHeight1 = resultHeight1;
                    actualHeight2 = resultHeight2;
                    panelMovement = 0;
                }
            }
            Panel.anchoredPosition = new Vector2(0, actualHeight1);
            //Scenery.anchoredPosition = new Vector2(0, actualHeight2);
        }
    }
}
