//  ScenesInProjectList.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// プロジェクト内のシーンの一覧
	/// </summary>
	public class ScenesInProjectList : SceneListBase
	{
		#region variables

		private ScenesInBuildInfo _scenesInBuildInfo;

		private List<int> _seletingIndexes;

		private Action<int, bool> _onElementSelected;

		private Action<int, bool> _onStartToggleChanged;

		private Action<int, bool> _onBuildToggleChanged;

		private Action<int> _onElementdLoaded;

		private Action<string, bool> _onFoldoutToggleChanged;

		#endregion variables

		#region methods

		public ScenesInProjectList( string name, List<int> selectingIndexes, List<int> sceneIndexes, AllSceneInfo allSceneInfo, ScenesInBuildInfo scenesInBuildInfo, bool isFoldout ) : base( name, sceneIndexes, allSceneInfo, isFoldout )
		{
			this._seletingIndexes = selectingIndexes;
			this._scenesInBuildInfo = scenesInBuildInfo;
		}

		public void RegistorCallback( Action<int, bool> onElementSelected, Action<int, bool> onStartToggleChanged, Action<int, bool> onBuildToggleChanged, Action<int> onElementLoaded, Action<string, bool> onFoldoutToggleChanged )
		{
			this._onElementSelected = onElementSelected;
			this._onStartToggleChanged = onStartToggleChanged;
			this._onBuildToggleChanged = onBuildToggleChanged;
			this._onElementdLoaded = onElementLoaded;
			this._onFoldoutToggleChanged = onFoldoutToggleChanged;
		}

		protected override void DrawHeaderDetail( GUIStyle style )
		{
			base.DrawHeaderDetail( style );
		}

		protected override void DrawElementDetail( int index, SceneInfo sceneInfo, GUIStyle style )
		{
			#if UNITY_2017
			var isStartScene = index == _allSceneInfo.StartSceneIndex;
			#endif

			var isCurrentScene = index == _allSceneInfo.CurrentSceneIndex;
			var isSelected = _seletingIndexes.Contains( index );
			var isBuild = _scenesInBuildInfo.SceneInBuildIndexes.Contains( index );

			if( _onElementSelected != null ) {
				DrawCheckBox( isSelected, style, ( isOn ) => _onElementSelected( index, isOn ) );
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

			if( _onElementdLoaded != null ) {
				EditorGUI.BeginDisabledGroup( EditorApplication.isPlaying || isCurrentScene );
				{
					DrawButton( "Load", style, () => _onElementdLoaded( index ) );
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