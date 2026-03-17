using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Names
{
    public class UINameCreatorSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<UIComponent, NameComponent, AliveTagComponent> _buffer = new();

        private readonly UIHudView _uiHudView;

        public UINameCreatorSystem(UIHudView uiHudView)
        {
            _uiHudView = uiHudView;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var uiComponent = _buffer.Components1[i];
            var nameComponent = _buffer.Components2[i];
            var aliveTagComponent = _buffer.Components3[i];

            var id = $"{UIConstants.Nickname}_{entityId}";

            var nameExists = uiComponent.Elements.Contains(UIConstants.Nickname);

            if (nameExists)
            {
                return;
            }

            CreateLabel(id, nameComponent.Name);
            uiComponent.Elements.Add(UIConstants.Nickname);
        }


        private Label CreateLabel(string name, string text)
        {
            var root = _uiHudView.NameAsset.CloneTree();
            var label = root.Q<Label>("nickname");

            label.name = name;
            label.text = text;
            label.style.position = Position.Absolute;

            _uiHudView.WorldHudRoot.Add(label);
            return label;
        }
    }
}
