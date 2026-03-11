using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.HUD
{
    public class UIHudView : MonoBehaviour
    {
        public VisualElement HudRoot => _hudDocument.rootVisualElement.Q<VisualElement>("content");
        public VisualElement WorldHudRoot => _hudDocument.rootVisualElement.Q<VisualElement>("content");

        [field: SerializeField] public VisualTreeAsset TeammateAsset { get; private set; }
        [field: SerializeField] public VisualTreeAsset NameAsset { get; private set; }
        [field: SerializeField] public VisualTreeAsset HealthAsset { get; private set; }

        [SerializeField] private UIDocument _hudDocument;
        [SerializeField] private UIDocument _worldHudDocument;
    }
}
