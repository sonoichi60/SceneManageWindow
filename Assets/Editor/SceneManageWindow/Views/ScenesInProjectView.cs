//  ScenesInProjectView.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// プロジェクト内のシーンのビュー
	/// </summary>
	public class ScenesInProjectView : FoldoutableViewBase
	{
		#region define

		private const string SCENES_IN_PROJECT_VIEW_TITLE = "Scenes In Project";

		#endregion define


		#region variables

		private MySceneManager _sceneManager;

		private List<ScenesInProjectList> _scenesInProjectLists;

		private List<int> _selectedSceneIndexes;

		private CreateSceneArea _createSceneArea;

		private DuplicateSceneArea _duplicateSceneArea;

		private RemoveSceneArea _removeSceneArea;

		private RenameSceneArea _renameSceneArea;

		private CreateSceneGroupArea _createSceneGroupArea;

		private AddSceneToSceneGroupArea _addSceneToSceneGroupArae;

		private CreateMultiSceneArea _createMultiSceneArea;

		private AddSceneToMultiSceneArea _addSceneToMultiSceneArea;

		private Action _onReloaded;

		#endregion variables


		#region methods

		#region methods initialize

		public ScenesInProjectView( MySceneManager sceneManager, Action onReloaded ) : base( SCENES_IN_PROJECT_VIEW_TITLE, sceneManager.OtherInfo.IsFoldoutOfScenesInProjectView )
		{
			this._sceneManager = sceneManager;
			this._onReloaded = onReloaded;
			this._scenesInProjectLists = new List<ScenesInProjectList>();
			this._selectedSceneIndexes = new List<int>();

			this._createSceneArea = new CreateSceneArea( sceneManager.AllSceneInfo.SceneDirectoryNames.ToArray(), OnSceneCreated );
			this._duplicateSceneArea = new DuplicateSceneArea( sceneManager.AllSceneInfo.SceneDirectoryNames.ToArray(), OnSceneDuplicated );
			this._removeSceneArea = new RemoveSceneArea( OnSceneRemoved );
			this._renameSceneArea = new RenameSceneArea( OnSceneRenamed );
			this._createSceneGroupArea = new CreateSceneGroupArea( OnSceneGroupCreated );
			this._addSceneToSceneGroupArae = new AddSceneToSceneGroupArea( _sceneManager.SceneGroupsInfo.GetSceneGroupNames(), OnScenesAddedToSceneGroup );
			this._createMultiSceneArea = new CreateMultiSceneArea( OnMultiSceneCreated );
			this._addSceneToMultiSceneArea = new AddSceneToMultiSceneArea( _sceneManager.MultiScenesInfo.GetMultiSceneNames(), OnSceneAddedToMultiScene );

			CreateList();
		}

		private void CreateList()
		{
			for( int i = 0 ; i < _sceneManager.AllSceneInfo.SceneDirectoryNames.Count ; i++ ) {
				var sceneIndexes = new List<int>();
				var directoryName = _sceneManager.AllSceneInfo.SceneDirectoryNames[ i ];
				for( int j = 0 ; j < _sceneManager.SceneCount ; j++ ) {
					var sceneInfo = _sceneManager.GetSceneInfo( j );
					if( sceneInfo != null && string.Compare( directoryName, sceneInfo.DirectoryName ) == 0 ) {
						sceneIndexes.Add( j );
					}
				}
				var listData = _sceneManager.OtherInfo.GetScenesInProjectListData( directoryName );
				_scenesInProjectLists.Add( new ScenesInProjectList( directoryName, _selectedSceneIndexes, sceneIndexes, _sceneManager.AllSceneInfo, _sceneManager.ScenesInBuildInfo, listData != null ? listData.IsFoldout : true ) );
			}

			for( int i = 0 ; i < _scenesInProjectLists.Count ; i++ ) {
				_scenesInProjectLists[ i ].RegistorCallback( OnSceneSelected, OnStartToggleChanged, OnBuildToggleChanged, OnSceneLoaded, OnFoldoutToggleChanged );
			}
		}

		#endregion methods initialize

		#region methods GUI

		protected override void DrawViewDetail()
		{
			if( _scenesInProjectLists.Count == 0 ) {
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label( "Scene not exits in project." );
				}
				GUILayout.EndHorizontal();
			}
			else {
				for( int i = 0 ; i < _scenesInProjectLists.Count ; i++ ) {
					_scenesInProjectLists[ i ].Draw();
				}
				DrawEditArea();
			}

			GUILayout.Space( 5f );
			if( GUILayout.Button( "Reload Scene List" ) ) {
				_onReloaded();
			}
			GUILayout.Space( 5f );
		}


		private void DrawEditArea()
		{
			GUILayout.Space( 5f );

			DrawInsideArea( () => {
			
				var labelStyle = new GUIStyle( EditorStyles.label );
				labelStyle.fontStyle = FontStyle.Bold;
				labelStyle.normal.textColor = Color.white;

				GUILayout.Label( "Selecting Scenes", labelStyle );

				labelStyle.fontStyle = FontStyle.Normal;
				if( _selectedSceneIndexes.Count == 0 ) {
					GUILayout.Label( "None.", labelStyle );
				}
				else {
					for( int i = 0 ; i < _selectedSceneIndexes.Count ; i++ ) {
						GUILayout.BeginHorizontal();
						{
							var scene = _sceneManager.GetSceneInfo( _selectedSceneIndexes[ i ] );
							GUILayout.Label( string.Format( "{0}: {1}", i, scene.Name ), labelStyle );
						}
						GUILayout.EndHorizontal();
					}
				}

				_createSceneArea.Draw();
				if( _selectedSceneIndexes.Count == 1 ) {
					_duplicateSceneArea.Draw();
					_removeSceneArea.Draw();
					_renameSceneArea.Draw();
				}
				if( _selectedSceneIndexes.Count >= 1 ) {
					_createSceneGroupArea.Draw();
					if( _sceneManager.SceneGroupCount > 0 ) {
						_addSceneToSceneGroupArae.Draw();
					}
				}
				if( _selectedSceneIndexes.Count >= 2 ) {
					_createMultiSceneArea.Draw();
				}
				if( _selectedSceneIndexes.Count >= 1 && _sceneManager.MultiSceneCount > 0 ) {
					_addSceneToMultiSceneArea.Draw();
				}
			} );
		}

		#endregion methods GUI

		#region methods callback

		protected override void OnFoldoutToggleChanged( bool isOn )
		{
			_sceneManager.OtherInfo.IsFoldoutOfScenesInProjectView = isOn;
			_sceneManager.Save();
		}

		private void OnReloaded()
		{
			if( _onReloaded != null ) {
				_onReloaded();
			}
		}

		#region methods callback sceneList

		private void OnSceneSelected( int sceneIndex, bool isOn )
		{
			if( isOn ) {
				_selectedSceneIndexes.Add( sceneIndex );
			}
			else {
				_selectedSceneIndexes.Remove( sceneIndex );
			}
		}

		private void OnStartToggleChanged( int sceneIndex, bool isOn )
		{
			#if UNITY_2017
			if( isOn ) {
				_sceneManager.SetStartScene( sceneIndex );
			}
			else {
				_sceneManager.UnsetStartScene();
			}
			#endif
		}

		private void OnBuildToggleChanged( int sceneIndex, bool isOn )
		{
			_sceneManager.SetSceneIsBuild( sceneIndex, isOn );
		}

		private void OnSceneLoaded( int sceneIndex )
		{
			_sceneManager.LoadScene( sceneIndex );
		}

		private void OnFoldoutToggleChanged( string headerText, bool isOn )
		{
			var listData = _sceneManager.OtherInfo.GetScenesInProjectListData( headerText );
			if( listData == null ) {
				return;
			}
			listData.IsFoldout = isOn;
			_sceneManager.Save();
		}

		#endregion methods callback sceneList

		#region methods callback editSceneArea

		private void OnSceneCreated( string directoryName, string sceneName )
		{
			EditorApplication.delayCall += () => {
				if( _sceneManager.CreateScene( directoryName, sceneName ) ) {
					OnReloaded();
				}
			};
		}

		private void OnSceneDuplicated( string directoryName, string sceneName )
		{
			if( _selectedSceneIndexes.Count == 0 ) {
				return;
			}

			EditorApplication.delayCall += () => {
				if( _sceneManager.DuplicateScene( _selectedSceneIndexes[ 0 ], directoryName, sceneName ) ) {
					OnReloaded();
				}
			};
		}

		private void OnSceneRemoved()
		{
			if( _selectedSceneIndexes.Count == 0 ) {
				return;
			}

			EditorApplication.delayCall += () => {
				if( _sceneManager.RemoveScene( _selectedSceneIndexes[ 0 ] ) ) {
					OnReloaded();
				}
			};
		}

		private void OnSceneRenamed( string newSceneName )
		{
			if( _selectedSceneIndexes.Count == 0 ) {
				return;
			}

			if( _sceneManager.RenameScene( _selectedSceneIndexes[ 0 ], newSceneName ) ) {
				OnReloaded();
			}
		}

		private void OnSceneGroupCreated( string sceneGroupName )
		{
			if( _sceneManager.CreateSceneGroup( sceneGroupName, _selectedSceneIndexes ) ) {
				OnReloaded();
			}
		}

		private void OnScenesAddedToSceneGroup( string sceneGroupName )
		{
			if( _sceneManager.AddScenesToSceneGroup( sceneGroupName, _selectedSceneIndexes ) ) {
				OnReloaded();
			}
		}

		private void OnMultiSceneCreated( string multiSceneName )
		{
			if( _sceneManager.CreateMultiScene( multiSceneName, _selectedSceneIndexes ) ) {
				OnReloaded();
			}
		}

		private void OnSceneAddedToMultiScene( string multiSceneName )
		{
			if( _sceneManager.AddSceneToMultiScene( multiSceneName, _selectedSceneIndexes ) ) {
				OnReloaded();
			}
		}

		#endregion methods callback editSceneArea

		#endregion methods callback

		#endregion methods
	}
}