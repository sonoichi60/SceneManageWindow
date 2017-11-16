//  MultiScenesView.cs
//
//  Created by Sonoichi.

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// マルチシーンのビュー
	/// </summary>
	public class MultiScenesView : FoldoutableViewBase
	{
		#region define

		private const string MULTI_SCENES_VIEW_TITLE = "Multi-Scenes";

		#endregion define


		#region variables

		private MySceneManager _sceneManager;

		private List<MultiSceneList> _multiSceneLists;

		#endregion variables


		#region methods

		#region methods initialize

		public MultiScenesView( MySceneManager sceneManager ) : base( MULTI_SCENES_VIEW_TITLE, sceneManager.OtherInfo.IsFoldoutOfMutiScenesView )
		{
			this._sceneManager = sceneManager;
			this._multiSceneLists = new List<MultiSceneList>();
			CreateList();
		}

		private void CreateList()
		{
			_multiSceneLists.Clear();
			for( int i = 0 ; i < _sceneManager.MultiSceneCount ; i++ ) {
				var multiSceneInfo = _sceneManager.MultiScenesInfo.MultiScenes[ i ];
				var subSceneIndexes = multiSceneInfo.SubScenePaths.Select( path => _sceneManager.GetSceneIndex( path ) ).Where( sceneIndex => sceneIndex != -1 ).ToList();
				var multiSceneList = new MultiSceneList( multiSceneInfo.HeaderText, multiSceneInfo.Name, subSceneIndexes, _sceneManager.AllSceneInfo, multiSceneInfo.IsFoldout );
				multiSceneList.RegistorCallback( OnMultiSceneRemoved, OnMultiSceneLoaded, OnSceneRemovedFromMultiScene, OnFoldoutToggleChanged );
				_multiSceneLists.Add( multiSceneList );
			}
		}

		#endregion methods initialize

		#region methods GUI

		protected override void DrawViewDetail()
		{
			if( _multiSceneLists.Count == 0 ) {
				DrawInsideArea( () => {
					GUILayout.Label( "No Multi-Scene." );
				} );
			}

			for( int i = 0 ; i < _multiSceneLists.Count ; i++ ) {
				_multiSceneLists[ i ].Draw();
			}

			GUILayout.Space( 5f );
		}

		#endregion methods GUI

		#region methods callback

		protected override void OnFoldoutToggleChanged( bool isOn )
		{
			_sceneManager.OtherInfo.IsFoldoutOfMutiScenesView = isOn;
			_sceneManager.Save();
		}

		private void OnMultiSceneRemoved( string multiSceneName )
		{
			if( _sceneManager.RemoveMultiScene( multiSceneName ) ) {
				CreateList();
			}
		}

		private void OnMultiSceneLoaded( string multiSceneName )
		{
			_sceneManager.LoadMultiScene( multiSceneName );
		}

		private void OnSceneRemovedFromMultiScene( string multiSceneName, int sceneIndex )
		{
			if( _sceneManager.RemoveSceneFromMultiScene( multiSceneName, sceneIndex ) ) {
				CreateList();
			}
		}

		private void OnFoldoutToggleChanged( string multiSceneName, bool isOn )
		{
			var multiSceneInfo = _sceneManager.GetMultiSceneInfo( multiSceneName );
			if( multiSceneInfo == null ) {
				return;
			}
			multiSceneInfo.IsFoldout = isOn;
			_sceneManager.Save();
		}

		#endregion methods callback

		#endregion methods
	}
}