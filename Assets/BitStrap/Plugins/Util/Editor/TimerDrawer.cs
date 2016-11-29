using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	[CustomPropertyDrawer( typeof( Timer ) )]
	public class TimerDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			PropertyDrawerHelper.LoadAttributeTooltip( this, label );
			var lengthProperty = property.GetMemberProperty<Timer>( t => t.Length );

			float progressWidth = 64.0f;

			var timer = SerializedPropertyHelper.GetValue( fieldInfo, property ) as Timer;
			if( timer == null )
				progressWidth = 0.0f;

			Rect propertyPosition = position.Left( -progressWidth );
			EditorGUI.PropertyField( propertyPosition, lengthProperty, label );
			GUI.Label( propertyPosition.Right( 52.0f ), "seconds", EditorStyles.centeredGreyMiniLabel );

			if( lengthProperty.floatValue < 0.0f )
			{
				lengthProperty.floatValue = 0.0f;
				property.serializedObject.ApplyModifiedProperties();
			}

			if( timer == null )
				return;

			string progressText = StringHelper.Format( "{0}%", Mathf.RoundToInt( timer.Progress * 100.0f ) );
			Rect progressPosition = position.Right( progressWidth );
			EditorGUI.ProgressBar( progressPosition, timer.Progress, progressText );
		}
	}
}
