using UnityEngine;
using UnityEngine.UIElements;


namespace Test_UIToolkit
{

    public class PopupSetup : MonoBehaviour
    {
        void OnEnable()
        {
            UIDocument ui = GetComponent<UIDocument>();
            VisualElement root = ui.rootVisualElement;

            PopupWindow popup = new PopupWindow();
            root.Add(popup);

            popup.confirmed += () => Debug.Log("Clicked Confirm");
            
            popup.cancelled += () => Debug.Log("Clicked Cancel");
            popup.cancelled += () => root.Remove(popup);
        }
    }

}