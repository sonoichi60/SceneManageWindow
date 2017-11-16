//  FoldoutableViewBase.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;
using System;

/// <summary>
/// 折りたたみ可能なビューのベース
/// </summary>
public abstract class FoldoutableViewBase
{
	#region define

	private const float MARGIN = 5f;

	#endregion define


	#region variables

	private string _title;

	private bool _isFoldout;

	#endregion varaibles


	#region methods

	public FoldoutableViewBase( string title, bool initIsFoldout )
	{
		this._title = title;
		this._isFoldout = initIsFoldout;
	}

	public void Draw()
	{
		GUILayout.BeginVertical();
		{
			var foldoutStyle = new GUIStyle( EditorStyles.foldout );
			foldoutStyle.alignment = TextAnchor.MiddleLeft;
			foldoutStyle.fontStyle = FontStyle.Bold;

			var newIsFoldout = EditorGUILayout.Foldout( _isFoldout, _title, foldoutStyle );
			if( _isFoldout != newIsFoldout ) {
				OnFoldoutToggleChanged( newIsFoldout );
				_isFoldout = newIsFoldout;
			}

			if( _isFoldout ) {
				GUILayout.BeginHorizontal();
				{
					GUILayoutUtility.GetRect( MARGIN, 1f, GUILayout.Width( MARGIN ) );
					GUILayout.BeginVertical();
					{
						DrawViewDetail();
					}
					GUILayout.EndVertical();
					GUILayoutUtility.GetRect( MARGIN, 1f, GUILayout.Width( MARGIN ) );
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Box( "", GUILayout.Width( EditorGUIUtility.currentViewWidth - 26f ), GUILayout.Height( 1 ) );
		}
		GUILayout.EndVertical();
	}

	protected virtual void DrawViewDetail()
	{
		GUILayout.Label( "No detail" );
	}

	protected void DrawInsideArea( Action onDraw )
	{
		GUILayout.BeginVertical( (GUIStyle)"ShurikenEffectBg", GUILayout.Height( 1f ) );
		{
			onDraw();
		}
		GUILayout.EndVertical();
	}

	protected virtual void OnFoldoutToggleChanged( bool isOn )
	{
	}

	#endregion methods
}
