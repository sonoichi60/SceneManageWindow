//  SceneGroupView.cs
//
//  Created by Sonoichi.

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// シーングループのビュー
	/// </summary>
	public class SceneGroupView : FoldoutableViewBase
	{
		#region define

		private const string SCENE_GROUP_VIEW_TITLE = "Scene Groups";

		#endregion define


		#region variables

		private MySceneManager _sceneManager;

		private List<SceneGroupList> _sceneGroupLists;

		#endregion variables


		#region methods

		#region methods initialize

		public SceneGroupView( MySceneManager sceneManager ) : base( SCENE_GROUP_VIEW_TITLE, sceneManager.OtherInfo.IsFoldoutOfSceneGroupsView )
		{
			this._sceneManager = sceneManager;
			this._sceneGroupLists = new List<SceneGroupList>();
			CreateList();
		}

		private void CreateList()
		{
			_sceneGroupLists.Clear();
			for( int i = 0 ; i < _sceneManager.SceneGroupCount ; i++ ) {
				var sceneGroupInfo = _sceneManager.SceneGroupsInfo.SceneGroups[ i ];
				var sceneIndexes = sceneGroupInfo.Paths.Select( path => _sceneManager.GetSceneIndex( path ) ).Where( sceneIndex => sceneIndex != -1 ).ToList();
				var sceneGroupList = new SceneGroupList( sceneGroupInfo.Name, sceneGroupInfo.IsFoldout, sceneIndexes, _sceneManager.AllSceneInfo, _sceneManager.ScenesInBuildInfo );
				sceneGroupList.RegistorCallback( OnSceneGroupRemoved, OnSceneRemovedFromSceneGroup, OnStartToggleChanged, OnBuildToggleChanged, OnSceneLoaded, OnFoldoutToggleChanged );
				_sceneGroupLists.Add( sceneGroupList );
			}
		}

		#endregion methods initialize

		#region methods GUI

		protected override void DrawViewDetail()
		{
			if( _sceneGroupLists.Count == 0 ) {
				DrawInsideArea( () => {
					GUILayout.Label( "No SceneGroup." );
				} );
			}

			for( int i = 0 ; i < _sceneGroupLists.Count ; i++ ) {
				_sceneGroupLists[ i ].Draw();
			}

			GUILayout.Space( 5f );
		}

		#endregion methods GUI

		#region methods callback

		protected override void OnFoldoutToggleChanged( bool isOn )
		{
			_sceneManager.OtherInfo.IsFoldoutOfSceneGroupsView = isOn;
			_sceneManager.Save();
		}

		private void OnSceneGroupRemoved( string sceneGroupName )
		{
			if( _sceneManager.RemoveSceneGroup( sceneGroupName ) ) {
				CreateList();
			}
		}

		private void OnSceneRemovedFromSceneGroup( string sceneGroupName, int sceneIndex )
		{
			if( _sceneManager.RemoveSceneFromSceneGroup( sceneGroupName, sceneIndex ) ) {
				CreateList();
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

		private void OnFoldoutToggleChanged( string sceneGroupName, bool isOn )
		{
			var sceneGroupInfo = _sceneManager.GetSceneGroupInfo( sceneGroupName );
			if( sceneGroupInfo == null ) {
				return;
			}
			sceneGroupInfo.IsFoldout = isOn;
			_sceneManager.Save();
		}

		#endregion methods callback

		#endregion methods
	}
}