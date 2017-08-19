using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TemplateCustomizer : UnityEditor.AssetModificationProcessor {

	public static void OnWillCreateAsset (string path) {
		path = path.Replace( ".meta", "" );
		int index = Application.dataPath.LastIndexOf( "Assets" );
		path = Application.dataPath.Substring( 0, index ) + path;

		if (IsPathValid(path)) {
			string fileContent = System.IO.File.ReadAllText( path );
			System.IO.File.WriteAllText( path, CustomizeTemplateText(path, fileContent) );
			AssetDatabase.Refresh();
		}
	}

	static bool IsPathValid (string path) {
		int index = path.LastIndexOf( "." );
		if (index < 0)
			return false;

		string file = path.Substring( index );
		if (file != ".cs")
			return false;

		if (!System.IO.File.Exists( path ))
			return false;

		return true;
	}

	static string CustomizeTemplateText (string path, string templateText) {
		templateText = InsertNamespace (path, templateText);

		return templateText;
	}

	static string InsertNamespace(string path, string templateText)
	{
		int index = path.LastIndexOf ("/");
		string nsName = path.Substring (0, index);
		index = nsName.LastIndexOf ("/");
		nsName = nsName.Substring (index + 1);

		return templateText.Replace ("$NSNAME$", nsName);
	}

}
