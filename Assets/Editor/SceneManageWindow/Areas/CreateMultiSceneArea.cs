//  CreateMultiSceneArea.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// マルチシーンを作成する領域
	/// </summary>
	public class CreateMultiSceneArea:AreaBase
	{
		#region define

		private const string CREATE_MULTI_SCENE_AREA_TITLE = "Create Multi-Scene";

		#endregion define


		#region variables

		private string _multiSceneName;

		private Action<string> _onMultiSceneCreated;

		#endregion variables


		#region methods

		public CreateMultiSceneArea( Action<string> onMultiSceneCreated ) : base( CREATE_MULTI_SCENE_AREA_TITLE )
		{
			this._onMultiSceneCreated = onMultiSceneCreated;
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label( "Name", GUILayout.Width( 40f ) );
				_multiSceneName = EditorGUILayout.TextField( _multiSceneName, EditorStyles.textField );
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				EditorGUI.BeginDisabledGroup( string.IsNullOrEmpty( _multiSceneName ) ); 
				{
					if( GUILayout.Button( "Create Multi-Scene" ) && _onMultiSceneCreated != null ) {
						_onMultiSceneCreated( _multiSceneName );
						GUI.FocusControl( "" );
						_multiSceneName = "";
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
		}

		#endregion methods
	}
}