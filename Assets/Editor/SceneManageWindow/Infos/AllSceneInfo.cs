//  AllSceneInfo.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SceneManageWindow
{
	/// <summary>
	/// すべてのシーンの情報
	/// </summary>
	public class AllSceneInfo
	{
		#region variables

		private List<SceneInfo> _scenes;

		private Dictionary<string, int> _sceneIndexDict;

		private List<string> _sceneDirectoryNames;

		public int CurrentSceneIndex{ get; private set; }

		#if UNITY_2017
		public int StartSceneIndex{ get; private set; }
		#endif

		public ReadOnlyCollection<SceneInfo> Scenes{ get { return _scenes.AsReadOnly(); } }

		public ReadOnlyCollection<string> SceneDirectoryNames{ get { return _sceneDirectoryNames.AsReadOnly(); } }

		public int SceneCount{ get { return _scenes.Count; } }

		public SceneInfo CurrentScene{ get { return GetSceneInfo( CurrentSceneIndex ); } }

		#endregion variables


		#region methods

		public AllSceneInfo()
		{
			this._scenes = new List<SceneInfo>();
			var sceneDirectriesList = new List<string>();
			var guids = AssetDatabase.FindAssets( "t:SceneAsset" );
			for( int i = 0 ; i < guids.Length ; i++ ) {
				var path = AssetDatabase.GUIDToAssetPath( guids[ i ] );
				_scenes.Add( new SceneInfo( path ) );
				var directoryName = Path.GetDirectoryName( path );
				if( !sceneDirectriesList.Contains( directoryName ) ) {
					sceneDirectriesList.Add( directoryName );
				}
			}

			#if UNITY_2017
			this.StartSceneIndex = -1;
			#endif

			this._sceneIndexDict = new Dictionary<string, int>();
			this._sceneDirectoryNames = new List<string>();
			for( int i = 0 ; i < _scenes.Count ; i++ ) {
				var path = _scenes[ i ].Path;
				_sceneIndexDict.Add( path, i );
				var directoryName = Path.GetDirectoryName( path );
				if( !_sceneDirectoryNames.Contains( directoryName ) ) {
					_sceneDirectoryNames.Add( directoryName );
				}
			}

			var currentScenePath = EditorSceneManager.GetActiveScene().path;
			this.CurrentSceneIndex = GetSceneIndex( currentScenePath );
		}

		public int GetSceneIndex( string path )
		{
			int index = -1;
			return _sceneIndexDict.TryGetValue( path, out index ) ? index : -1;
		}

		public SceneInfo GetSceneInfo( int index )
		{
			return index < 0 || SceneCount <= index ? null : _scenes[ index ];
		}

		public bool LoadScene( int sceneIndex, OpenSceneMode mode = OpenSceneMode.Single )
		{
			var scene = GetSceneInfo( sceneIndex );
			if( scene == null ) {
				return false;
			}

			if( !scene.Load( mode ) ) {
				return false;
			}
			CurrentSceneIndex = sceneIndex;
			return true;
		}

		public bool LoadSubScene( int sceneIndex )
		{
			return LoadScene( sceneIndex, OpenSceneMode.Additive );
		}

		private string GetScenePath( string directoryName, string sceneName )
		{
			return string.Format( "{0}/{1}.unity", directoryName, sceneName );
		}

		private bool IsInvalidFileName( string fileName )
		{
			var invalidChars = Path.GetInvalidFileNameChars();
			return fileName.IndexOfAny( invalidChars ) >= 0;
		}

		private bool IsUsablePath( string path )
		{
			if( string.IsNullOrEmpty( path ) ) {
				return false;
			}

			for( int i = 0 ; i < _scenes.Count ; i++ ) {
				if( string.Compare( path, _scenes[ i ].Path ) == 0 ) {
					return false;
				}
			}
			return true;
		}

		public bool CreateScene( string directoryName, string sceneName )
		{
			if( IsInvalidFileName( sceneName ) ) {
				EditorUtility.DisplayDialog( "Error!", string.Format( "Invalid scene name : \"{0}\"", sceneName ), "OK" );
				return false;
			}

			var path = GetScenePath( directoryName, sceneName );
			if( !IsUsablePath( path ) ) {
				EditorUtility.DisplayDialog( "Error!", string.Format( "Invalid path : \"{0}\"", path ), "OK" );
				return false;
			}

			if( !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() ) {
				return false;
			}

			var newScene = EditorSceneManager.NewScene( NewSceneSetup.DefaultGameObjects );
			_scenes.Add( new SceneInfo( path ) );
			EditorSceneManager.SaveScene( newScene, path );
			return true;
		}

		public bool DuplicateScene( string scenePath, string directoryName, string sceneName )
		{
			if( IsInvalidFileName( sceneName ) ) {
				EditorUtility.DisplayDialog( "Error!", string.Format( "Invalid scene name. : \"{0}\"", sceneName ), "OK" );
				return false;
			}

			var path = GetScenePath( directoryName, sceneName );
			if( !IsUsablePath( path ) ) {
				EditorUtility.DisplayDialog( "Error!", string.Format( "Invalid path. : \"{0}\"", path ), "OK" );
				return false;
			}

			if( !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() ) {
				return false;
			}
			if( !AssetDatabase.CopyAsset( scenePath, path ) ) {
				return false;
			}
			_scenes.Add( new SceneInfo( path ) );
			return true;
		}

		public bool RemoveScene( int sceneIndex )
		{
			var removeSceneInfo = GetSceneInfo( sceneIndex );
			if( removeSceneInfo == null ) {
				return false;
			}

			if( !EditorUtility.DisplayDialog( "Confirm deletion of SceneAsset", string.Format( "Can I delete scene assets named \"{0}\"?", removeSceneInfo.Name ), "OK", "Cancel" ) ) {
				return false;
			}

			if( !AssetDatabase.DeleteAsset( removeSceneInfo.Path ) ) {
				return false;
			}

			_scenes.Remove( removeSceneInfo );
			if( CurrentSceneIndex == sceneIndex ) {
				CurrentSceneIndex = -1;
			}
			return true;
		}

		public bool RenameScene( int sceneIndex, string newName )
		{
			var sceneInfo = GetSceneInfo( sceneIndex );
			if( sceneInfo == null ) {
				return false;
			}

			return sceneInfo.Rename( newName );
		}

		#if UNITY_2017
		public void SetStartScene( string startScenePath )
		{
			StartSceneIndex = GetSceneIndex( startScenePath );
		}

		public void UnsetStartScene()
		{
			StartSceneIndex = -1;
		}
		#endif

		#endregion methods
	}
}