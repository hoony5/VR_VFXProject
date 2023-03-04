using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[CustomEditor(typeof(VFXInteractValueContainer))]
public class VFXInteractValueContainerEditor : Editor
{
    private VFXInteractValueContainer _container;
    private const int Capacity = 128;
    
    private List<VFXExposedProperty> _exposedProperties = new List<VFXExposedProperty>(Capacity);
    private string[] _exposedPropertyNames = Array.Empty<string>();
    private Type[] _exposedPropertyTypes = Array.Empty<Type>();
    
    private int _selectVFXPropertyIndex;
    private string _selectVFXPropertyName;
    private Type _selectVFXPropertyType;
    
    
    private static SerializedProperty _vfxProperty;
    private static SerializedProperty _interactItemsProperty;
    private static SerializedProperty _vfxProgressScenarioProperty;
    private static SerializedProperty _vfxProgressScenarioKeyProperty;

    private static GUIContent _vfxContent = new GUIContent("VFX");
    private static GUIContent _vfxProgressScenarioContent = new GUIContent("Progress Scenario");
    private static GUIContent _vfxProgressScenarioKeyContent = new GUIContent("Key for Add Scenario Item");
    private static GUIContent _interactItemsContent = new GUIContent("Interactable Items");
    private static GUIContent _wantedVFXProperty = new GUIContent("Selecting VFX Property is added to Array : interactItems");
    private void OnEnable()
    {
        _container = (VFXInteractValueContainer)target;
        _vfxProgressScenarioKeyProperty = serializedObject.FindProperty("scnarioKey");
        _vfxProperty = serializedObject.FindProperty("vfx");
        _interactItemsProperty = serializedObject.FindProperty("interactItems");
        _vfxProgressScenarioProperty = serializedObject.FindProperty("vfxProgressScenario");
    }

    public override void OnInspectorGUI()
    {
       // base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_vfxProperty, _vfxContent);
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("For Add VFX Interaction Item Area");
        if (GUILayout.Button("Get Exposed Properties"))
        {
            _container._loadSuccess = GetExposedPropertiesInfo();
        }

        if (_container._loadSuccess)
        {
            // for re-focusing data
            if (_exposedPropertyNames.Length == 0)
                _ = GetExposedPropertiesInfo();
            
            DrawVFXValueContainer();
            EditorGUILayout.Space(10);
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_interactItemsProperty, _interactItemsContent);
        Rect horizontalLine = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(horizontalLine.x, horizontalLine.y, horizontalLine.width, 2), Color.green);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("For Add Progress Scenario Item Area");
        EditorGUILayout.PropertyField(_vfxProgressScenarioProperty, _vfxProgressScenarioContent);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_vfxProgressScenarioKeyProperty, _vfxProgressScenarioKeyContent);
        EditorGUILayout.Space(10);
        AddScenarioProperty();
        serializedObject.ApplyModifiedProperties();
    }

    private bool GetExposedPropertiesInfo()
    {
        if (_container.vfx is null)
        {
            Debug.Log($"There is no VFX");
            return false;
        }
        
        // read exposed properties
        _container.vfx.visualEffectAsset.GetExposedProperties(_exposedProperties);

        if (_exposedProperties.Count is 0)
        {
            Debug.Log($"There is no Exposed Properties.");
            return false;
        }

        _exposedPropertyNames = new string[_exposedProperties.Count];
        _exposedPropertyTypes = new Type[_exposedProperties.Count];
        
        for (var i = 0; i < _exposedProperties.Count; i++)
        {
            VFXExposedProperty current = _exposedProperties[i];
            _exposedPropertyNames[i] = current.name;
            _exposedPropertyTypes[i] = current.type;
        }

        return true;
    }

    private void DrawVFXValueContainer()
    {
        EditorGUILayout.Space(10);
        // select VFX Property -> add array
        _selectVFXPropertyIndex = EditorGUILayout.Popup(_wantedVFXProperty, _selectVFXPropertyIndex, _exposedPropertyNames);
        
        if (_exposedPropertyNames.Length <= _selectVFXPropertyIndex) return;
        if (_exposedPropertyTypes.Length <= _selectVFXPropertyIndex) return;
        
        _selectVFXPropertyName = _exposedPropertyNames?[_selectVFXPropertyIndex];
        _selectVFXPropertyType = _exposedPropertyTypes?[_selectVFXPropertyIndex];

        EditorGUILayout.BeginHorizontal();
        AddInteractionProperty();
        EditorGUILayout.Space(10);
        if (GUILayout.Button("Remove Interaction All Values"))
        {
            _container.interactItems.Clear();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddInteractionProperty()
    {
        if (GUILayout.Button("Add VFX Property"))
        {
            ExposedProperty exposedProperty = _selectVFXPropertyName;
            VFXInteractValue interactValue = new VFXInteractValue();

            if (_selectVFXPropertyType == typeof(int))
            {
                interactValue.vfxValue = new VFXInt(exposedProperty, 0);
                interactValue.propertyType = VFXPropertyType.Int;
            }
            
            if(_selectVFXPropertyType == typeof(float))
            {
                interactValue.vfxValue = new VFXFloat(exposedProperty, 0);
                interactValue.propertyType = VFXPropertyType.Float;
            }
            
            if(_selectVFXPropertyType == typeof(AnimationCurve))
            {
                interactValue.vfxValue = new VFXCurve(exposedProperty, null);
                interactValue.propertyType = VFXPropertyType.Curve;
            }
            
            if(_selectVFXPropertyType == typeof(Vector2))
            {
                interactValue.vfxValue = new VFXVector2(exposedProperty, Vector2.zero);
                interactValue.propertyType = VFXPropertyType.Vector2;
            }
            
            if(_selectVFXPropertyType == typeof(Vector3))
            {
                interactValue.vfxValue = new VFXVector3(exposedProperty, Vector3.zero);
                interactValue.propertyType = VFXPropertyType.Vector3;
            }
            
            if(_selectVFXPropertyType == typeof(bool))
            {
                interactValue.vfxValue = new VFXBool(exposedProperty, false);
                interactValue.propertyType = VFXPropertyType.Bool;
            }
            
            if(_selectVFXPropertyType == typeof(Gradient))
            {
                interactValue.vfxValue = new VFXGradient(exposedProperty, null);
                interactValue.propertyType = VFXPropertyType.Gradient;
            }

            interactValue.displayName = _selectVFXPropertyName;
            
            if (_container.interactItems.Contains(interactValue))
                _container.interactItems.Remove(interactValue); 
            
            _container.interactItems.Add(interactValue);
        }
    }

    private void AddScenarioProperty()
    {
        if (GUILayout.Button("Add Scenario Property"))
        {
            if (!_vfxProgressScenarioProperty.objectReferenceValue) return;
            
            ExposedProperty exposedProperty = _selectVFXPropertyName;
            VFXInteractValue interactValue = new VFXInteractValue();

            if (_selectVFXPropertyType == typeof(int))
            {
                interactValue.vfxValue = new VFXInt(exposedProperty, 0);
                interactValue.propertyType = VFXPropertyType.Int;
            }

            if (_selectVFXPropertyType == typeof(float))
            {
                interactValue.vfxValue = new VFXFloat(exposedProperty, 0);
                interactValue.propertyType = VFXPropertyType.Float;
            }

            if (_selectVFXPropertyType == typeof(AnimationCurve))
            {
                interactValue.vfxValue = new VFXCurve(exposedProperty, null);
                interactValue.propertyType = VFXPropertyType.Curve;
            }

            if (_selectVFXPropertyType == typeof(Vector2))
            {
                interactValue.vfxValue = new VFXVector2(exposedProperty, Vector2.zero);
                interactValue.propertyType = VFXPropertyType.Vector2;
            }

            if (_selectVFXPropertyType == typeof(Vector3))
            {
                interactValue.vfxValue = new VFXVector3(exposedProperty, Vector3.zero);
                interactValue.propertyType = VFXPropertyType.Vector3;
            }

            if (_selectVFXPropertyType == typeof(bool))
            {
                interactValue.vfxValue = new VFXBool(exposedProperty, false);
                interactValue.propertyType = VFXPropertyType.Bool;
            }

            if (_selectVFXPropertyType == typeof(Gradient))
            {
                interactValue.vfxValue = new VFXGradient(exposedProperty, null);
                interactValue.propertyType = VFXPropertyType.Gradient;
            }

            interactValue.displayName = _selectVFXPropertyName;
            
            VFXProgressScenario scenario = (VFXProgressScenario)_vfxProgressScenarioProperty.objectReferenceValue;
            VFXProgressItem item = scenario.progressItems.Find(i => i.index == _container.scnarioKey);
            if (item is null)
            {
                scenario.progressItems.Add(new VFXProgressItem(){ index = _container.scnarioKey, interactValues = new List<VFXInteractValue>() { interactValue } });
                return;
            }
            
            List<VFXInteractValue> list = item.interactValues;
            if (list != null && list.Contains(interactValue))
                list.Remove(interactValue);

            list.Add(interactValue);
        }
    }
}