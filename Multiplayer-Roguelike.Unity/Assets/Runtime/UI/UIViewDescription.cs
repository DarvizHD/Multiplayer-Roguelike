using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI
{
    [CreateAssetMenu(fileName = "UIViewDescription", menuName = "ViewDescriptions/UIViewDescription")]
    public class UIViewDescription : ScriptableObject
    {
        [SerializeField] private List<PanelViewDescription> _elements;
        public VisualTreeAsset UserAsset;

        public PanelViewDescription Get(string id)
        {
            return _elements.Find(x => x.Id == id);
        }
    }
}
