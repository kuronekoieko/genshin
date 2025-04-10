using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(B))]
public class MyDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 各フィールドの高さを合計
        int fieldCount = 2;
        return EditorGUIUtility.singleLineHeight * fieldCount + EditorGUIUtility.standardVerticalSpacing * (fieldCount - 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var isUseProp = property.FindPropertyRelative("isUse");
        var nameProp = property.FindPropertyRelative("name");

        if (isUseProp == null || nameProp == null)
        {
            Debug.LogError("プロパティが見つかりません");
            return;
        }

        // 横並び：最小限の間隔だけ入れる
        float padding = 2f; // ほんの少しだけ間隔を開ける
        float totalWidth = position.width;

        // isUse（true/false）は bool だから幅を少なめに
        float isUseWidth = 20f;
        float nameWidth = totalWidth - isUseWidth - padding;

        Rect isUseRect = new Rect(position.x, position.y, isUseWidth, EditorGUIUtility.singleLineHeight);
        Rect nameRect = new Rect(position.x + isUseWidth + padding, position.y, nameWidth, EditorGUIUtility.singleLineHeight);

        // フィールドを描画
        EditorGUI.PropertyField(isUseRect, isUseProp, GUIContent.none);
        EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);

        EditorGUI.EndProperty();
    }


}
