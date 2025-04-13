using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SelectedArtMainGoblet))]
public class SelectedArtMainGobletDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 各フィールドの高さを合計
        int fieldCount = 1;
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
        float fieldWidth = (totalWidth - isUseWidth - padding * 2) / 2f;



        Rect isUseRect = new Rect(position.x, position.y, isUseWidth, EditorGUIUtility.singleLineHeight);
        Rect nameRect = new Rect(isUseRect.xMax + padding, position.y, fieldWidth, EditorGUIUtility.singleLineHeight);


        // フィールドを描画
        EditorGUI.PropertyField(isUseRect, isUseProp, GUIContent.none, false);
        EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none, false);

        EditorGUI.EndProperty();
    }


}