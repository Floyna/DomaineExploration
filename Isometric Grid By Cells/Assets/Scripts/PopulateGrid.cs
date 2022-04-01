using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    [SerializeField] GameObject prefabTxt;
    [SerializeField] Scrollbar scrollbar;

    int lastChar = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject goTxt = Instantiate(prefabTxt, transform) as GameObject;
            TextMeshProUGUI text = goTxt.GetComponentInChildren<TextMeshProUGUI>();
            switch (Random.Range(0, 3))
            {
                case 0:
                    text.text = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor." +
                                " Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.";
                    // Debug.Log("TextLenght: " + text.text.Length);
                    //Debug.Log("LastChar: " + lastChar);
                    break;
                case 1:
                    text.text = "Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, " +
                                "fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo.";
                    Debug.Log("LastChar: " + lastChar);
                    break;
                case 2:
                    text.text = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.";
                    // Debug.Log("LastChar: " + lastChar);
                    break;
                default:
                    break;
            }
            switch (Random.Range(0, 5))
            {
                case 0:
                    text.color = Color.black;
                    break;
                case 1:
                    text.color = Color.green;
                    break;
                case 2:
                    text.color = Color.red;
                    break;
                case 3:
                    text.color = Color.blue;
                    break;
                case 4:
                    text.color = Color.gray;
                    break;
                default:
                    break;
            }


            lastChar = text.text.Length;
            scrollbar.value = 0;
        }
    }
}
