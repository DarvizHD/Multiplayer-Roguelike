using System.Collections.Generic;
using System.Reflection;
using Runtime;
using Runtime.Ecs.Components;
using Runtime.Ecs.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.EcsDebugger
{
    public class EcsDebugWindow : EditorWindow
    {
        private readonly Dictionary<ushort, bool> _entityFoldouts = new();
        private EntryPoint _entryPoint;
        private Vector2 _scrollPos;

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
                return;
            }

            _entryPoint = FindFirstObjectByType<EntryPoint>();

            if (!_entryPoint)
            {
                EditorGUILayout.HelpBox("EntryPoint not found", MessageType.Warning);
                return;
            }

            var componentManager = EcsWorld.DebugInstance?.ComponentManager;

            if (componentManager == null)
            {
                return;
            }

            var entities = componentManager.GetAllEntities();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.LabelField($"Total Entities: {entities.Count}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            foreach (var entity in entities)
            {
                DrawEntity(componentManager, entity);
            }

            EditorGUILayout.EndScrollView();

            Repaint();
        }

        private void DrawEntity(ComponentManager componentManager, ushort entityId)
        {
            _entityFoldouts.TryAdd(entityId, false);

            _entityFoldouts[entityId] =
                EditorGUILayout.Foldout(_entityFoldouts[entityId], $"Entity {entityId}", true);

            if (!_entityFoldouts[entityId])
                return;

            EditorGUI.indentLevel++;

            DrawComponents(componentManager, entityId);

            EditorGUI.indentLevel--;
        }

        private void DrawComponents(ComponentManager componentManager, ushort entityId)
        {
            foreach (var component in componentManager.GetAllComponents(entityId))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                var type = component.GetType();

                EditorGUILayout.LabelField(type.Name, EditorStyles.boldLabel);

                EditorGUI.indentLevel++;

                DrawFields(component);

                EditorGUI.indentLevel--;

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }
        }

        private void DrawFields(object component)
        {
            var fields = component.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(component);

                EditorGUILayout.LabelField(
                    $"{field.Name}: {(value is Object obj ? obj.name : value)}");
            }
        }
    }
}
