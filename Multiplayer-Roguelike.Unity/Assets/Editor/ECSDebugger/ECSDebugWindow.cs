using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Runtime;
using Runtime.ECS.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.ECSDebugger
{
    public class EcsDebugWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        private EntryPoint _entryPoint;
        private readonly Dictionary<int, bool> _foldouts = new();

        [MenuItem("Tools/ECS Debugger")]
        public static void ShowWindow()
        {
            GetWindow<EcsDebugWindow>("ECS Debugger");
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Available only in Play Mode", MessageType.Info);
                {
                    return;
                }
            }

            _entryPoint = FindFirstObjectByType<EntryPoint>();

            if (!_entryPoint)
            {
                EditorGUILayout.HelpBox("EntryPoint not found in the scene", MessageType.Warning);
                {
                    return;
                }
            }

            var componentManager = _entryPoint.EcsWorld.ComponentManager;

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var entities = componentManager.GetAllEntities().ToList();

            EditorGUILayout.LabelField($"Total Entities: {entities.Count}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            ShowEntityHeader(entities, componentManager);

            EditorGUILayout.EndScrollView();

            Repaint();
        }

        private void ShowEntityHeader(List<int> entities, ComponentManager componentManager)
        {
            foreach (var entityId in entities)
            {
                _foldouts.TryAdd(entityId, false);

                _foldouts[entityId] = EditorGUILayout.Foldout(_foldouts[entityId], $"Entity {entityId}", true);

                if (!_foldouts[entityId])
                {
                    continue;
                }

                EditorGUI.indentLevel++;

                ShowComponentHeader(componentManager, entityId);

                EditorGUI.indentLevel--;
            }
        }

        private void ShowComponentHeader(ComponentManager componentManager, int entityId)
        {
            var componentTypes = componentManager.GetComponentTypes(entityId);

            foreach (var componentType in componentTypes)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(componentType.Name, EditorStyles.boldLabel);

                var component = componentManager.GetComponent(entityId, componentType);
                if (component != null)
                {
                    EditorGUI.indentLevel++;
                    ShowComponentFields(component);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }
        }

        private void ShowComponentFields(object component)
        {
            var fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(component);
                EditorGUILayout.LabelField($"{field.Name}: {(value is Object obj ? obj.name : value)}");
            }

            foreach (var property in properties)
            {
                var value = property.GetValue(component);
                EditorGUILayout.LabelField($"{property.Name}: {(value is Object obj ? obj.name : value)}");
            }
        }
    }
}
