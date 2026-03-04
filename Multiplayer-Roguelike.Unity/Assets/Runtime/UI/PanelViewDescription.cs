using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI
{
    [CreateAssetMenu(fileName = "PanelViewDescription", menuName = "ViewDescriptions/PanelViewDescription")]
    public class PanelViewDescription : ScriptableObject
    {
        public string Id => name;
        public VisualTreeAsset Asset;
    }
}
