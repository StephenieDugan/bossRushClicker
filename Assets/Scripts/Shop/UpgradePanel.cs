using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public Canvas canvas;
    public GameObject Scenery;
    public RectTransform Panel;
    public float canvasHeight;
    public float canvasHeightBottom; //for testing, i made it a variable to make sure for the frame of the phone.
    public Button OpenShopButton; // Reference to the button
    public Button CloseShopButton; // Reference to the button
    private float resultHeight1, resultHeight2; // Target heights when opening/closing
    private float actualHeight1, actualHeight2; // Current animated heights
    public int panelMovement;

    public void Start()
    {
        canvasHeight = canvas.gameObject.GetComponent<RectTransform>().rect.height + 500f;
        ClosePanel();

        // Ensure button calls OpenPanel() when clicked
        if (OpenShopButton != null)
            OpenShopButton.onClick.AddListener(OpenPanel);
        // Ensure button calls ClosePanel() when clicked
        if (CloseShopButton != null)
            CloseShopButton.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel()
    {
        resultHeight1 = 0; // canvasHeight / -3.5f; // change this value to make the shop reach up to this height on the screen, 0 brings the box to the middle of the screen
        resultHeight2 = resultHeight1 - 100f; // Optional offset effect
        actualHeight1 = -canvasHeight;  // Start from bottom
        actualHeight2 = -canvasHeight / 4;
        panelMovement = 1; // Start movement

        Debug.Log(resultHeight1); Debug.Log(resultHeight2); Debug.Log(actualHeight1); Debug.Log(actualHeight2); Debug.Log(panelMovement);

        //Panel.anchoredPosition = new Vector2(0, 0);

        // Ensure button calls OpenPanel() when clicked

        //-Scenery.sizeDelta = new Vector2(0, canvasHeight);
        //Scenery.anchoredPosition = new Vector2(0, 0);
    }

    public void ClosePanel()
    {
        resultHeight1 = -canvasHeight - 500f; // Start completely off-screen
        resultHeight2 = resultHeight1 - 100f; // Optional offset effect
        //actualHeight1 = canvasHeight / 4;
        //actualHeight2 = canvasHeight; // Start fully above the screen

        panelMovement = 2; // make sure the movement happens


        //Panel.anchoredPosition = new Vector2(0, -canvasHeight / 2);
        //Panel.anchoredPosition = new Vector2(0, 0);
        //-Scenery.sizeDelta = new Vector2(0, canvasHeight / 2);
        //Scenery.anchoredPosition = new Vector2(0, -canvasHeight / 4);


    }


    /*
    public void Update()
    {
        //var height = canvas.gameObject.GetComponent<RectTransform>().rect.height;
        //print(height);  //debug to figure out where the panel is
        
        if (panelMovement != 0)
        {
            if (panelMovement == 1)// Move up
            {
                //Debug.Log(actualHeight1);
                actualHeight1 += canvasHeight * Time.deltaTime; // Smooth transition
                actualHeight2 += (canvasHeight/2) * Time.deltaTime; // Slightly slower for a staggered effect
                if (actualHeight1 > resultHeight1)
                {
                    actualHeight1 = resultHeight1;
                    actualHeight2 = resultHeight2;
                    panelMovement = 0; // Stop movement
                }
            }else if (panelMovement == 2) // Move down (if needed)
            {
                actualHeight1 -= canvasHeight * Time.deltaTime;
                actualHeight2 -= (canvasHeight / 2) * Time.deltaTime;

                if (actualHeight1 <= resultHeight1)
                {
                    actualHeight1 = resultHeight1;
                    actualHeight2 = resultHeight2;
                    panelMovement = 0;
                }
            }
        
            Panel.anchoredPosition = new Vector2(0, actualHeight1);
            //Scenery.anchoredPosition = new Vector2(0, actualHeight2);
        }
    */
    public void Update()
    {
        if (panelMovement == 1) // Move Up (Open)
        {
            actualHeight1 = Mathf.Lerp(actualHeight1, resultHeight1, Time.deltaTime * 5f);
            if (Mathf.Abs(actualHeight1 - resultHeight1) < 1f)
            {
                actualHeight1 = resultHeight1;
                panelMovement = 0; // Stop moving
            }
        }
        else if (panelMovement == 2) // Move Down (Close)
        {
            actualHeight1 = Mathf.Lerp(actualHeight1, resultHeight1, Time.deltaTime * 5f);
            if (Mathf.Abs(actualHeight1 - resultHeight1) < 1f)
            {
                actualHeight1 = resultHeight1;
                panelMovement = 0; // Stop moving
            }
        }

        Panel.anchoredPosition = new Vector2(0, actualHeight1);
    }
}
