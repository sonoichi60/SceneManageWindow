//  SceneGroupList.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// シーングループの一覧
	/// </summary>
	public class SceneGroupList: SceneListBase
	{
		#region variables

		private ScenesInBuildInfo _scenesInBuildInfo;

		private Action<string> _onListRemoved;

		private Action<string,int> _onElementRemoved;

		private Action<int, bool> _onStartToggleChanged;

		private Action<int, bool> _onBuildToggleChanged;

		private Action<int> _onElementLoaded;

		private Action<string, bool> _onFoldoutToggleChanged;

		#endregion variables


		#region methods

		public SceneGroupList( string sceneGroupName, bool isFoldout, List<int> sceneIndexes, AllSceneInfo allSceneInfo, ScenesInBuildInfo scenesInBuildInfo ) : base( sceneGroupName, sceneIndexes, allSceneInfo, isFoldout )
		{
			this._scenesInBuildInfo = scenesInBuildInfo;
		}

		public void RegistorCallback( Action<string> onListRemoved, Action<string, int> onElementRemoved, Action<int, bool> onStartToggleChanged, Action<int, bool> onBuildToggleChanged, Action<int> onElementLoaded, Action<string, bool> onFoldoutToggleChanged )
		{
			this._onListRemoved = onListRemoved;
			this._onElementRemoved = onElementRemoved;
			this._onStartToggleChanged = onStartToggleChanged;
			this._onBuildToggleChanged = onBuildToggleChanged;
			this._onElementLoaded = onElementLoaded;
			this._onFoldoutToggleChanged = onFoldoutToggleChanged;
		}

		protected override void DrawHeaderDetail( GUIStyle style )
		{
			if( _onListRemoved != null ) {
				DrawButton( "Remove", style, () => _onListRemoved( HeaderText ) );
			} 

			base.DrawHeaderDetail( style );
		}

		protected override void DrawElementDetail( int index, SceneInfo sceneInfo, GUIStyle style )
		{
			#if UNITY_2017
			var isStartScene = index == _allSceneInfo.StartSceneIndex;
			#endif

			var isCurrentScene = index == _allSceneInfo.CurrentSceneIndex;
			var isBuild = _scenesInBuildInfo.SceneInBuildIndexes.Contains( index );

			if( _onElementRemoved != null ) {
				DrawButton( "Remove", style, () => _onElementRemoved( HeaderText, index ) );
			}

			base.DrawElementDetail( index, sceneInfo, style );

			#if UNITY_2017
			if( _onStartToggleChanged != null ) {
				DrawToggle( isStartScene, isStartScene ? "Start" : "None", style, ( isOn ) => _onStartToggleChanged( index, isOn ) );
			}
			#endif

			if( _onBuildToggleChanged != null ) {
				DrawToggle( isBuild, isBuild ? "Build" : "NonBuild", style, ( isOn ) => _onBuildToggleChanged( index, isOn ) );
			}

			if( _onElementLoaded != null ) {
				EditorGUI.BeginDisabledGroup( EditorApplication.isPlaying || isCurrentScene );
				{
					DrawButton( "Load", style, () => _onElementLoaded( index ) );
				}
				EditorGUI.EndDisabledGroup();
			}
		}

		protected override void OnFoldoutToggleChanged( bool isOn )
		{
			if( _onFoldoutToggleChanged != null ) {
				_onFoldoutToggleChanged( HeaderText, isOn );
			}
		}

		#endregion methods
	}
}