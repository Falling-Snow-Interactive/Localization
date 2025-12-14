using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fsi.Localization
{
    [CustomPropertyDrawer(typeof(LocEntry))]
    public class LocEntryDrawer : PropertyDrawer
    {
        private const float Indent = 10f;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Box foldoutBox = new()
                             {
                                 style = { paddingLeft = Indent + property.depth * Indent },
                                 tooltip = property.tooltip,
                             };

            // Pull the InspectorName applied to the *field that owns this LocEntry*
            string inspectorName = fieldInfo?.GetCustomAttribute<InspectorNameAttribute>()?.displayName;
            string labelText = string.IsNullOrEmpty(inspectorName) ? property.displayName : inspectorName;

            SerializedProperty entryProp = property.FindPropertyRelative("entry");

            PropertyField entryField = new(entryProp)
                                       {
                                           label = $" {labelText}"
                                       };

            foldoutBox.Add(entryField);
            return foldoutBox;
        }
    }
}