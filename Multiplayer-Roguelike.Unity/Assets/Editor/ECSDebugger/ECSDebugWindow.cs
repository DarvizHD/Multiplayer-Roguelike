using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor.ECSDebugger
{
    public class EcsDebugWindow : EditorWindow
    {
        private Vector2 _scrollPos;
        private EntryPoint _entryPoint;
        private readonly Dictionary<int, bool> _foldouts = new ();

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
                EditorGUILayout.HelpBox("EntryPoint not found in the scene", MessageType.Warning);
                return;
            }

            var componentManager = _entryPoint.EcsWorld.ComponentManager;

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var entities = componentManager.GetAllEntities().ToList();

            EditorGUILayout.LabelField($"Total Entities: {entities.Count}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            foreach (var entityId in entities)
            {
                _foldouts.TryAdd(entityId, false);

                _foldouts[entityId] = EditorGUILayout.Foldout(_foldouts[entityId], $"Entity {entityId}", true);

                if (!_foldouts[entityId])
                {
                    continue;
                }
                
                EditorGUI.indentLevel++;

                var componentTypes = componentManager.GetComponentTypes(entityId);

                foreach (var componentType in componentTypes)
                {
                    EditorGUILayout.LabelField(componentType.Name, EditorStyles.boldLabel);

                    var component = componentManager.GetComponent(entityId, componentType);
                    if (component != null)
                    {
                        EditorGUI.indentLevel++;
                        ShowComponentFields(component);
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.Space();
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndScrollView();

            Repaint();
        }

        private void ShowComponentFields(object component)
        {
            var fields = component.GetType().GetFields();

            foreach (var field in fields)
            {
                var value = field.GetValue(component);
                EditorGUILayout.LabelField($"{field.Name}: {value}");
            }
        }
    }
}