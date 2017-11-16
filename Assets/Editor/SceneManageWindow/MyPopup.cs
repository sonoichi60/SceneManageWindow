//  MyPopup.cs
//
//  Created by Sonoichi.

using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// パスを表示できるポップアップ
/// </summary>
public class MyPopup : EditorWindow
{
	#region define

	private static readonly GUIStyle STYLE = new GUIStyle() {
		normal = new GUIStyleState() {
			textColor = Color.black,
			background = Texture2D.whiteTexture,
		},
		active = ( (GUIStyle)"LODSliderRangeSelected" ).normal,
		hover = ( (GUIStyle)"LODSliderRangeSelected" ).normal,
		padding = new RectOffset( 5, 5, 3, 3 ),
		margin = new RectOffset( 1, 1, 1, 1 ),
	};

	#endregion define


	#region variables

	static MyPopup _myPopup;

	private string[] _elements;

	#endregion variables


	#region methods

	private Action<int> _onSelected;

	public static void Open( Rect position, string[] elements, Action<int> onSelected )
	{
		if( _myPopup == null ) {
			_myPopup = CreateInstance<MyPopup>();
		}

		_myPopup._elements = elements;
		_myPopup._onSelected = onSelected;

		_myPopup.position = new Rect( position ) {
			size = CalcWindowSize( elements ),
		};
		_myPopup.ShowPopup();
		_myPopup.Focus();
	}

	private static Vector2 CalcWindowSize( string[] elements )
	{
		var size = Vector2.zero;
		for( int i = 0 ; i < elements.Length ; i++ ) {
			var content = new GUIContent( elements[ i ] );
			var contentSize = STYLE.CalcSize( content );
			size.y += contentSize.y + STYLE.margin.top;
			if( size.x < contentSize.x ) {
				size.x = contentSize.x;
			}
		}
		size += new Vector2( STYLE.margin.right + STYLE.margin.left, STYLE.margin.bottom );

		return size;
	}

	#endregion methods


	#region unity eventss

	void OnGUI()
	{
		GUILayout.BeginVertical();
		{
			var e = Event.current;
			var size = Vector2.zero;
			for( int i = 0 ; i < _elements.Length ; i++ ) {
				if( GUILayout.Button( _elements[ i ], STYLE ) ) {
					if( _onSelected != null ) {
						_onSelected( i );
						Close();
					}
				}
			}
		}
		GUILayout.EndVertical();
	}

	void OnLostFocus()
	{
		_myPopup.Close();
	}

	#endregion unity events
}