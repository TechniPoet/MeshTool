using UnityEditor;
using UnityEngine;

namespace BitStrap
{
	/// <summary>
	/// Makes it easy to work with EditorPrefs treating them as properties.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public abstract class EditorPrefProperty<T>
	{
		[SerializeField]
		protected string key;

		private T value;
		private bool initialized = false;

		/// <summary>
		/// Use this property to get/set this editor pref.
		/// </summary>
		public T Value
		{
			get { RetrieveValue(); return value; }
			set { SaveValue( value ); }
		}

		protected EditorPrefProperty( string prefKey )
		{
			key = prefKey;
			value = default( T );
			initialized = false;
		}

		protected void RetrieveValue()
		{
			if( !initialized )
			{
				value = OnRetrieveValue();
				initialized = true;
			}
		}

		protected void SaveValue( T newValue )
		{
			value = newValue;
			OnSaveValue( value );
		}

		protected abstract T OnRetrieveValue();

		protected abstract void OnSaveValue( T value );
	}

	/// <summary>
	/// A specialization of EditorPrefProperty for strings.
	/// </summary>
	[System.Serializable]
	public class EditorPrefString : EditorPrefProperty<string>
	{
		private string defaultValue = "";

		public EditorPrefString( string key ) : base( key )
		{
		}

		public EditorPrefString( string key, string defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override string OnRetrieveValue()
		{
			return EditorPrefs.GetString( key, defaultValue );
		}

		protected override void OnSaveValue( string value )
		{
			EditorPrefs.SetString( key, value );
		}
	}

	/// <summary>
	/// A specialization of EditorPrefProperty class for ints.
	/// </summary>
	[System.Serializable]
	public class EditorPrefInt : EditorPrefProperty<int>
	{
		private int defaultValue = 0;

		public EditorPrefInt( string key ) : base( key )
		{
		}

		public EditorPrefInt( string key, int defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override int OnRetrieveValue()
		{
			return EditorPrefs.GetInt( key, defaultValue );
		}

		protected override void OnSaveValue( int value )
		{
			EditorPrefs.SetInt( key, value );
		}
	}

	/// <summary>
	/// A specialization of EditorPrefProperty class for floats.
	/// </summary>
	[System.Serializable]
	public class EditorPrefFloat : EditorPrefProperty<float>
	{
		private float defaultValue = 0.0f;

		public EditorPrefFloat( string key ) : base( key )
		{
		}

		public EditorPrefFloat( string key, float defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override float OnRetrieveValue()
		{
			return EditorPrefs.GetFloat( key, defaultValue );
		}

		protected override void OnSaveValue( float value )
		{
			EditorPrefs.SetFloat( key, value );
		}
	}

	/// <summary>
	/// A specialization of EditorPrefProperty class for bool.
	/// </summary>
	[System.Serializable]
	public class EditorPrefBool : EditorPrefProperty<bool>
	{
		private bool defaultValue = false;

		public EditorPrefBool( string key ) : base( key )
		{
		}

		public EditorPrefBool( string key, bool defaultValue ) : base( key )
		{
			this.defaultValue = defaultValue;
		}

		protected override bool OnRetrieveValue()
		{
			return EditorPrefs.GetBool( key, defaultValue );
		}

		protected override void OnSaveValue( bool value )
		{
			EditorPrefs.SetBool( key, value );
		}
	}
}
