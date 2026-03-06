using UnityEngine;

namespace Runtime.UI
{
    [CreateAssetMenu(fileName = "WorldViewDescription", menuName = "ViewDescriptions/WorldViewDescription")]
    public class WorldViewDescription : ScriptableObject
    {
        public UIViewDescription UI;
    }
}
