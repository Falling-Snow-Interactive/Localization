using UnityEditor;
using UnityEditor.UIElements;
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

			SerializedProperty entryProp = property.FindPropertyRelative("entry");
			PropertyField entryField = new(entryProp){ label = $" {property.displayName}" };
			foldoutBox.Add(entryField);
			
			return foldoutBox;
		}
	}
}