//  SceneGroupInfo.cs
//
//  Created by Sonoichi.

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SceneManageWindow
{
	/// <summary>
	/// シーングループ単体の情報
	/// </summary>
	[Serializable]
	public class SceneGroupInfo
	{
		#region variables

		[SerializeField]
		private string _name = "";

		[SerializeField]
		private List<string> _paths = new List<string>();

		public bool IsFoldout = true;

		public string Name{ get { return _name; } }

		public ReadOnlyCollection<string> Paths{ get { return _paths.AsReadOnly(); } }

		#endregion variables


		#region methods

		public SceneGroupInfo( string name, List<string> paths, bool isFoldout = true )
		{
			this._name = name;
			this._paths = paths;
			this.IsFoldout = isFoldout;
		}

		public bool AddScene( string path )
		{
			if( string.IsNullOrEmpty( path ) || _paths.Contains( path ) ) {
				return false;
			}

			_paths.Add( path );
			return true;
		}

		public bool RemoveScece( string path )
		{
			return _paths.Remove( path );
		}

		public void ReplacePath( string prePath, string newPath )
		{
			for( int i = 0 ; i < _paths.Count ; i++ ) {
				if( string.Compare( prePath, _paths[ i ] ) == 0 ) {
					_paths[ i ] = newPath;
					return;
				}
			}
		}

		#endregion methods
	}
}