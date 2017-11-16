//  AreaBase.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;

/// <summary>
/// 領域描画のベース
/// </summary>
public abstract class AreaBase
{
	#region variables

	public readonly string _title;

	#endregion variables


	#region methods

	public AreaBase( string title )
	{
		this._title = title;
	}

	public void Draw()
	{
		var areaStyle = new GUIStyle( (GUIStyle)"RL Background" ) {
			alignment = TextAnchor.MiddleCenter,
			margin = new RectOffset( 5, 5, 0, 5 ),
			padding = new RectOffset( 10, 10, 0, 10 ),
		};

		GUILayout.BeginVertical( areaStyle );
		{
			GUILayout.Label( _title, EditorStyles.boldLabel );
			DrawAreaDetail( areaStyle );
		}
		GUILayout.EndVertical();
	}

	protected abstract void DrawAreaDetail( GUIStyle style );

	protected string DrawTextFirld( string text )
	{
		return EditorGUILayout.TextField( text, EditorStyles.textField );
	}

	#endregion methods
}
