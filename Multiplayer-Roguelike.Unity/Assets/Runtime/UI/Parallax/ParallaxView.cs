using UnityEngine.UIElements;

namespace Runtime.UI.Parallax
{
    public class ParallaxView
    {
        public readonly VisualElement Root;
        public readonly VisualElement Background;

        public ParallaxView(VisualElement root)
        {
            Root = root;
            Background = Root.Q<VisualElement>("background");
        }
    }
}
