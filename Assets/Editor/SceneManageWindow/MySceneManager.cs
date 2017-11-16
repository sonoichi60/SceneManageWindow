//  MySceneManager.cs
//
//  Created by Sonoichi.

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace SceneManageWindow
{
	/// <summary>
	/// シーン全般の管理
	/// </summary>
	public class MySceneManager
	{
		#region degine

		private const string SCENE_GROUPS_INFO_KEY = "SceneGroupsInfoKey";

		private const string MULTI_SCENES_INFO_KEY = "MultiScenesInfoKey";

		private const string OTHER_INFO_KEY = "OtherInfoKey";

		#endregion define


		#region variables

		public readonly AllSceneInfo AllSceneInfo;

		public readonly SceneGroupsInfo SceneGroupsInfo;

		public readonly MultiScenesInfo MultiScenesInfo;

		public readonly ScenesInBuildInfo ScenesInBuildInfo;

		public readonly OtherInfo OtherInfo;

		public int SceneCount { get { return this.AllSceneInfo.SceneCount; } }

		public int SceneGroupCount { get { return this.SceneGroupsInfo.SceneGroupCount; } }

		public int MultiSceneCount { get { return this.MultiScenesInfo.MultiSceneCount; } }

		#endregion variables


		#region methods

		public MySceneManager()
		{
			this.OtherInfo = LoadInstance<OtherInfo>( OTHER_INFO_KEY );
			this.SceneGroupsInfo = LoadInstance<SceneGroupsInfo>( SCENE_GROUPS_INFO_KEY );
			this.MultiScenesInfo = LoadInstance<MultiScenesInfo>( MULTI_SCENES_INFO_KEY );

			this.AllSceneInfo = new AllSceneInfo();
			this.ScenesInBuildInfo = new ScenesInBuildInfo( this.AllSceneInfo );
			this.OtherInfo.Init( AllSceneInfo.SceneDirectoryNames.ToList() );

			var paths = this.AllSceneInfo.Scenes.Select( scene => scene.Path ).ToList();
			this.SceneGroupsInfo.Clean( paths );
			this.MultiScenesInfo.Clean( paths );

			#if UNITY_2017
			var startSceneIndex = GetSceneIndex( OtherInfo.StartScenePath );
			SetStartScene( startSceneIndex );
			#endif
		}

		public void Save()
		{
			SaveInstance<OtherInfo>( OtherInfo, OTHER_INFO_KEY );
			SaveInstance<SceneGroupsInfo>( this.SceneGroupsInfo, SCENE_GROUPS_INFO_KEY );
			SaveInstance<MultiScenesInfo>( this.MultiScenesInfo, MULTI_SCENES_INFO_KEY );
		}

		private T LoadInstance<T>( string key ) where T : new()
		{
			var json = EditorUserSettings.GetConfigValue( key );
			var instance = JsonUtility.FromJson<T>( json );
			return instance != null ? instance : new T();
		}

		private void SaveInstance<T>( T instance, string key )
		{
			var json = JsonUtility.ToJson( instance );
			EditorUserSettings.SetConfigValue( key, json );
		}

		#region methdos getter

		public int GetSceneIndex( string path )
		{
			return AllSceneInfo.GetSceneIndex( path );
		}

		public SceneInfo GetSceneInfo( int index )
		{
			return AllSceneInfo.GetSceneInfo( index );
		}

		public SceneGroupInfo GetSceneGroupInfo( string sceneGroupName )
		{
			return SceneGroupsInfo.GetSceneGroupInfo( sceneGroupName );
		}

		public MultiSceneInfo GetMultiSceneInfo( string multiSceneName )
		{
			return MultiScenesInfo.GetMultiScene( multiSceneName );
		}

		#endregion methods getter

		#region methods scenes

		#if UNITY_2017
		public void SetStartScene( int sceneIndex )
		{
			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				UnsetStartScene();
				return;
			}

			var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>( sceneInfo.Path );
			if( sceneAsset == null ) {
				UnsetStartScene();
				return;
			}

			AllSceneInfo.SetStartScene( sceneInfo.Path );
			EditorSceneManager.playModeStartScene = sceneAsset;
			OtherInfo.StartScenePath = sceneInfo.Path;
			Save();
		}

		public void UnsetStartScene()
		{
			AllSceneInfo.UnsetStartScene();
			EditorSceneManager.playModeStartScene = null;
			OtherInfo.StartScenePath = "";
			Save();
		}
		#endif

		public bool LoadScene( int sceneIndex )
		{
			return AllSceneInfo.LoadScene( sceneIndex );
		}

		public bool CreateScene( string directoryName, string sceneName )
		{
			return AllSceneInfo.CreateScene( directoryName, sceneName );
		}

		public bool DuplicateScene( int sceneIndex, string directoryName, string sceneName )
		{
			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				return false;
			}

			return AllSceneInfo.DuplicateScene( sceneInfo.Path, directoryName, sceneName );
		}

		public bool RemoveScene( int sceneIndex )
		{
			return AllSceneInfo.RemoveScene( sceneIndex );
		}

		public bool RenameScene( int sceneIndex, string newName )
		{
			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				return false;
			}

			var prePath = sceneInfo.Path;

			if( !sceneInfo.Rename( newName ) ) {
				return false;
			}

			SceneGroupsInfo.RepalcePathInAllSceneGroup( prePath, sceneInfo.Path );
			MultiScenesInfo.ReplacePathInAllMultiScene( prePath, sceneInfo.Path );
			Save();
			return true;
		}

		public void SetSceneIsBuild( int sceneIndex, bool isBuild )
		{
			if( isBuild ) {
				ScenesInBuildInfo.AddSceneInBuildIndex( sceneIndex );
			}
			else {
				ScenesInBuildInfo.RemoveSceneInBuildIndex( sceneIndex );
			}
			Save();
		}

		#endregion methods scenes

		#region methods sceneGroups

		public bool CreateSceneGroup( string groupName, List<int> sceneIndexes )
		{
			if( sceneIndexes == null || sceneIndexes.Count == 0 ) {
				return false;
			}

			var paths = new List<string>();
			for( int i = 0 ; i < sceneIndexes.Count ; i++ ) {
				var sceneInfo = GetSceneInfo( sceneIndexes[ i ] );
				if( sceneInfo == null ) {
					continue;
				}
				paths.Add( sceneInfo.Path );
			}

			if( !SceneGroupsInfo.AddSceneGroup( groupName, paths ) ) {
				return false;
			}

			Save();
			return true;
		}

		public bool RemoveSceneGroup( string groupName )
		{
			if( !SceneGroupsInfo.RemoveSceneGroup( groupName ) ) {
				return false;
			}

			Save();
			return true;
		}

		public bool AddScenesToSceneGroup( string groupName, List<int> sceneIndexes )
		{
			if( sceneIndexes == null || sceneIndexes.Count == 0 ) {
				return false;
			}

			var sceneGroup = GetSceneGroupInfo( groupName );
			if( sceneGroup == null ) {
				return false;
			}

			bool success = false;
			for( int i = 0 ; i < sceneIndexes.Count ; i++ ) {
				var sceneInfo = GetSceneInfo( sceneIndexes[ i ] );
				if( sceneInfo == null ) {
					continue;
				}
				sceneGroup.AddScene( sceneInfo.Path );
				success = true;
			}

			if( success ) {
				Save();
			}

			return success;
		}

		public bool RemoveSceneFromSceneGroup( string sceneGroupName, int sceneIndex )
		{
			var sceneGroupInfo = GetSceneGroupInfo( sceneGroupName );
			if( sceneGroupInfo == null ) {
				return false;
			}

			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				return false;
			}

			if( !sceneGroupInfo.RemoveScece( sceneInfo.Path ) ) {
				return false;
			}

			if( sceneGroupInfo.Paths.Count == 0 ) {
				this.SceneGroupsInfo.RemoveSceneGroup( sceneGroupName );
			}
			Save();
			return true;
		}

		#endregion methods sceneGroups

		#region methods multiScenes

		public bool LoadMultiScene( string multiSceneName )
		{
			var multiSceneInfo = GetMultiSceneInfo( multiSceneName );
			if( multiSceneInfo == null ) {
				return false;
			}

			LoadScene( GetSceneIndex( multiSceneInfo.MainScenePath ) );
			for( int i = 0 ; i < multiSceneInfo.SubSceneCount ; i++ ) {
				LoadSubScene( GetSceneIndex( multiSceneInfo.SubScenePaths[ i ] ) );
			}

			return true;
		}

		private void LoadSubScene( int sceneIndex )
		{
			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				return;
			}

			if( !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() ) {
				return;
			}

			EditorSceneManager.OpenScene( sceneInfo.Path, OpenSceneMode.Additive );
		}

		public bool CreateMultiScene( string multiSceneName, List<int>sceneIndexes )
		{
			if( sceneIndexes == null || sceneIndexes.Count == 0 ) {
				return false;
			}

			var mainSceneInfo = GetSceneInfo( sceneIndexes[ 0 ] );
			if( mainSceneInfo == null ) {
				return false;
			}

			var subScenePaths = new List<string>();
			for( int i = 1 ; i < sceneIndexes.Count ; i++ ) {
				var subSceneInfo = GetSceneInfo( sceneIndexes[ i ] );
				if( subSceneInfo != null ) {
					subScenePaths.Add( subSceneInfo.Path );
				}
			}

			if( !MultiScenesInfo.AddMultiScene( multiSceneName, mainSceneInfo.Path, subScenePaths ) ) {
				return false;
			}

			Save();
			return true;
		}

		public bool RemoveMultiScene( string multiSceneName )
		{
			if( !MultiScenesInfo.RemoveMultiScene( multiSceneName ) ) {
				return false;
			}

			Save();
			return true;
		}

		public bool AddSceneToMultiScene( string multiSceneName, List<int> sceneIndexes )
		{
			if( sceneIndexes == null || sceneIndexes.Count == 0 ) {
				return false;
			}

			var multiSceneInfo = GetMultiSceneInfo( multiSceneName );
			if( multiSceneInfo == null ) {
				return false;
			}

			bool success = false;
			for( int i = 0 ; i < sceneIndexes.Count ; i++ ) {
				var sceneInfo = GetSceneInfo( sceneIndexes[ i ] );
				if( sceneInfo == null ) {
					continue;
				}
				multiSceneInfo.AddSubScene( sceneInfo.Path );
				success = true;
			}

			if( success ) {
				Save();
			}

			return success;
		}

		public bool RemoveSceneFromMultiScene( string multiSceneName, int sceneIndex )
		{
			var multiSceneInfo = GetMultiSceneInfo( multiSceneName );
			if( multiSceneInfo == null ) {
				return false;
			}

			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				return false;
			}

			return multiSceneInfo.RemoveSubScene( sceneInfo.Path );
		}

		#endregion methods multiScenes

		#endregion methods
	}
}