//  WindowBase.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;

/// <summary>
/// ウィンドウのベース
/// </summary>
public abstract class WindowBase : EditorWindow
{
	#region define

	private const float WINDOW_MIN_WIDTH = 200f;

	#endregion define


	#region variables

	private Vector2 _scrollPos;

	#endregion variables


	#region methods

	protected abstract void Initialize();

	protected virtual void DrawWindowDetail()
	{
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label( "Non detail." );
		}
		GUILayout.EndHorizontal();
	}

	#endregion methods


	#region unity events

	private void OnEnable()
	{
		Initialize();
	}

	private void OnGUI()
	{
		_scrollPos = GUILayout.BeginScrollView( _scrollPos, false, true );
		{
			GUILayout.BeginVertical( GUILayout.ExpandWidth( true ), GUILayout.MinWidth( WINDOW_MIN_WIDTH ) );
			{
				GUILayout.Space( 5f );
				DrawWindowDetail();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndScrollView();
	}

	#endregion unity events
}
