//  CreateSceneGroupArea.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;

namespace SceneManageWindow
{
	/// <summary>
	/// シーングループを作成する領域
	/// </summary>
	public class CreateSceneGroupArea : AreaBase
	{
		#region define

		private const string CREATE_SCENE_GROUP_AREA_TITLE = "Create Scene Group";

		#endregion define


		#region variables

		private string _sceneGroupName;

		private Action<string> _onSceneGroupCreated;

		#endregion variables


		#region methods

		public CreateSceneGroupArea( Action<string> onSceneGroupCreated ) : base( CREATE_SCENE_GROUP_AREA_TITLE )
		{
			this._onSceneGroupCreated = onSceneGroupCreated;
			this._sceneGroupName = "";
		}

		protected override void DrawAreaDetail( GUIStyle style )
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label( "Name", GUILayout.Width( 40f ) );
				_sceneGroupName = EditorGUILayout.TextField( _sceneGroupName, EditorStyles.textField );
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				EditorGUI.BeginDisabledGroup( string.IsNullOrEmpty( _sceneGroupName ) ); 
				{
					if( GUILayout.Button( "Create Scene Group" ) && _onSceneGroupCreated != null ) {
						_onSceneGroupCreated( _sceneGroupName );
						GUI.FocusControl( "" );
						_sceneGroupName = "";
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			GUILayout.EndHorizontal();
		}

		#endregion methods
	}
}