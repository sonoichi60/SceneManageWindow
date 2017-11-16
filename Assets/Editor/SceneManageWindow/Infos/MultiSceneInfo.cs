//  MultiSceneInfo.cs
//
//  Created by Sonoichi.

using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SceneManageWindow
{
	/// <summary>
	/// マルチシーン単体の情報
	/// </summary>
	[Serializable]
	public class MultiSceneInfo
	{
		#region variables

		[SerializeField]
		private string _name = "";

		[SerializeField]
		private string _mainScenePath = "";

		[SerializeField]
		private List<string> _subScenePaths = new List<string>();

		[SerializeField]
		private bool _isFoldout = false;

		public string Name { get { return _name; } }

		public string MainScenePath { get { return _mainScenePath; } }

		public ReadOnlyCollection<string> SubScenePaths { get { return _subScenePaths.AsReadOnly(); } }

		public string HeaderText { get { return string.Format( "{0} ({1})", Name, Path.GetFileNameWithoutExtension( _mainScenePath ) ); } }

		public int SubSceneCount { get { return _subScenePaths.Count; } }

		public bool IsFoldout{ get { return _isFoldout; } set { _isFoldout = value; } }

		#endregion variables


		#region methods

		public MultiSceneInfo( string name, string mainScenePath, List<string> subScenePaths, bool isFoldout = true )
		{
			this._name = name;
			this._mainScenePath = mainScenePath;
			this._subScenePaths = subScenePaths;
			this.IsFoldout = isFoldout;
		}

		public void AddSubScene( string path )
		{
			if( string.IsNullOrEmpty( path ) || _subScenePaths.Contains( path ) ) {
				return;
			}

			_subScenePaths.Add( path );
		}

		public bool RemoveSubScene( string path )
		{
			return _subScenePaths.Remove( path );
		}

		public void ReplacePath( string prePath, string newPath )
		{
			if( string.Compare( _mainScenePath, prePath ) == 0 ) {
				_mainScenePath = newPath;
				return;
			}

			for( int i = 0 ; i < _subScenePaths.Count ; i++ ) {
				if( string.Compare( _subScenePaths[ i ], prePath ) == 0 ) {
					_subScenePaths[ i ] = newPath;
					return;
				}
			}
		}

		#endregion methods
	}
}