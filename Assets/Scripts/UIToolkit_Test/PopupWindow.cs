using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test_UIToolkit
{
    [UxmlElement]
    public partial class PopupWindow : VisualElement
    {
        public event Action confirmed;
        public event Action cancelled;

        //private const string syleResource = "PopupWindowStyleSheet";
        private const string ussPopup = "popup_window";
        private const string ussPopupContainer = "popup_container";
        private const string ussHorContainer = "horizontal_container";
        private const string ussPopupMsg = "popup_msg";
        private const string ussPopupButton = "popup_button";
        private const string ussButtonCencel = "button_cancel";
        private const string ussButtonConfirm = "button_confire";

        public PopupWindow()
        {
            //styleSheets.Add(Resources.Load<StyleSheet>(syleResource));
            AddToClassList(ussPopupContainer);

            VisualElement window = new VisualElement();
            window.AddToClassList(ussPopup);
            hierarchy.Add(window);

            // Text Section
            VisualElement horizontalContainerText = new VisualElement();
            horizontalContainerText.AddToClassList(ussHorContainer);
            window.Add(horizontalContainerText);

            Label msgLable = new Label();
            msgLable.text = "Do you really want the red pill?(빨간약 원해요?)";
            msgLable.AddToClassList(ussPopupMsg);
            horizontalContainerText.Add(msgLable);

            // Button Section
            VisualElement horizontalContainerButton = new VisualElement();
            horizontalContainerButton.AddToClassList(ussHorContainer);
            window.Add(horizontalContainerButton);

            Button confirmButton = new Button() { text = "CONFIRM(확인)" };
            confirmButton.AddToClassList(ussPopupButton);
            confirmButton.AddToClassList(ussButtonConfirm);
            horizontalContainerButton.Add(confirmButton);

            Button cancelButton = new Button() { text = "CANCEL(취소)" };
            cancelButton.AddToClassList(ussPopupButton);
            cancelButton.AddToClassList(ussButtonCencel);
            horizontalContainerButton.Add(cancelButton);

            confirmButton.clicked += OnConfirn;
            cancelButton.clicked += OnCancel;
        }

        private void OnConfirn()
        {
            confirmed?.Invoke();
        }

        private void OnCancel()
        {
            cancelled?.Invoke();
        }
    }
}