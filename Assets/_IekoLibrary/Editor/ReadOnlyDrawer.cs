using UnityEngine;
using UnityEditor;


namespace IekoLibrary
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(_position, _property, _label, true);
			EditorGUI.EndDisabledGroup();
		}
	}
}
