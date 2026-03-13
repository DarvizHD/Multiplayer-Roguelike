using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Parallax
{
    public class ParallaxView
    {
        public readonly VisualElement Root;
        public readonly VisualElement Background;

        public ParallaxView(UIDocument root)
        {
            Root = root.rootVisualElement.Q<VisualElement>("menu-root");
            Background = Root.Q<VisualElement>("background");
        }
    }
}
