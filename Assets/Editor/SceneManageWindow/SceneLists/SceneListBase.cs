//  SceneListBase.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// シーンの一覧表示のベース
	/// </summary>
	public abstract class SceneListBase
	{
		#region define

		protected readonly static Color StartSceneElementColor = new Color( 1f, 0.6f, 0.6f, 1f );
		protected readonly static Color CurrentSceneElementColor = new Color( 0.6f, 1f, 0.6f, 1f );

		#endregion define


		#region variables

		public readonly string HeaderText;

		protected readonly AllSceneInfo _allSceneInfo;

		private readonly List<int> _sceneIndexes;

		private bool _isFoldout;

		#endregion variables


		#region methods

		public SceneListBase( string headerText, List<int> sceneIndexes, AllSceneInfo allSceneInfo, bool isFoldout )
		{
			this.HeaderText = headerText;
			this._allSceneInfo = allSceneInfo;
			this._sceneIndexes = sceneIndexes;
			this._isFoldout = isFoldout;
		}

		public void Draw()
		{
			if( _allSceneInfo == null ) {
				return;
			}

			GUILayout.BeginVertical();
			{
				DrawHeader();

				if( _isFoldout ) {
					DrawElements();
				}
			}
			GUILayout.EndVertical();
		}

		private void DrawHeader()
		{
			var headerStyle = new GUIStyle( (GUIStyle)"toolbarbutton" ) {
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				margin = new RectOffset( 0, 0, 0, 0 ),
				padding = new RectOffset( 0, 0, 0, 0 ),
			};

			GUILayout.BeginHorizontal( headerStyle );
			{
				DrawHeaderDetail( headerStyle );
			}
			GUILayout.EndHorizontal();
		}

		protected virtual void DrawHeaderDetail( GUIStyle style )
		{
			var curEvent = Event.current;
			GUILayout.Space( 2f );
			var toggleRect = GUILayoutUtility.GetRect( 1f, EditorGUIUtility.singleLineHeight );
			if( curEvent.type == EventType.Repaint ) {
				EditorStyles.foldout.Draw( toggleRect, HeaderText, false, false, _isFoldout, false );
			}
			if( curEvent.type == EventType.MouseDown && toggleRect.Contains( curEvent.mousePosition ) ) {
				_isFoldout = !_isFoldout;
				OnFoldoutToggleChanged( _isFoldout );
				curEvent.Use();
			}
		}

		private void DrawElements()
		{
			var elementStyle = new GUIStyle( (GUIStyle)"LargeButtonMid" ) {
				fontSize = 10,
				margin = new RectOffset( 0, 0, 0, 0 ),
				padding = new RectOffset( 0, 0, 3, 3 ),
			};

			GUILayout.BeginVertical();
			{
				for( int i = 0 ; i < _sceneIndexes.Count ; i++ ) {
					DrawElement( _sceneIndexes[ i ], elementStyle );
				}
			}
			GUILayout.EndVertical();
		}

		private void DrawElement( int index, GUIStyle style )
		{
			var sceneInfo = _allSceneInfo.GetSceneInfo( index );
			if( sceneInfo == null ) {
				return;
			}

			GUILayout.BeginHorizontal();
			{
				DrawElementDetail( index, sceneInfo, style );
			}
			GUILayout.EndHorizontal();
		}

		protected virtual void DrawElementDetail( int index, SceneInfo sceneInfo, GUIStyle style )
		{
			var defaultColor = GUI.backgroundColor;
			GUI.backgroundColor = GetElementColor( index, defaultColor );
			GUILayout.Label( sceneInfo.Name, style );
			GUI.backgroundColor = defaultColor;
		}

		protected void DrawButton( string text, GUIStyle style, Action onClicked )
		{
			if( GUILayout.Button( text, style, GUILayout.Width( 60f ) ) && onClicked != null ) {
				onClicked();
			}
		}

		protected bool DrawCheckBox( bool isChecked, GUIStyle style, Action<bool> onValueChanged )
		{
			var newIsChecked = GUILayout.Toggle( isChecked, isChecked ? "●" : "", style, GUILayout.Width( 20f ) );
			if( isChecked != newIsChecked && onValueChanged != null ) {
				onValueChanged( newIsChecked );
			}
			return isChecked;
		}

		protected bool DrawToggle( bool isOn, string text, GUIStyle style, Action<bool> onValueChanged )
		{
			var newIsOn = GUILayout.Toggle( isOn, text, style, GUILayout.Width( 60f ) );
			if( isOn != newIsOn && onValueChanged != null ) {
				onValueChanged( newIsOn );
			}
			return newIsOn;
		}

		protected virtual Color GetElementColor( int index, Color defaultColor )
		{
			var isCurrentScene = index == _allSceneInfo.CurrentSceneIndex;

			#if UNITY_2017
			var isStartScene = index == _allSceneInfo.StartSceneIndex;
			return isCurrentScene ? CurrentSceneElementColor : ( isStartScene ? StartSceneElementColor : defaultColor );
			#else
		return isCurrentScene ? CurrentSceneElementColor : defaultColor;
			#endif
		}

		protected abstract void OnFoldoutToggleChanged( bool isOn );

		#endregion methods
	}
}