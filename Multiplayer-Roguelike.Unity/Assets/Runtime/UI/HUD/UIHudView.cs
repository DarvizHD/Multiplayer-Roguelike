using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.HUD
{
    public class UIHudView : MonoBehaviour
    {
        public VisualElement Root => _uiDocument.rootVisualElement;

        [SerializeField] private UIDocument _uiDocument;
    }
}
