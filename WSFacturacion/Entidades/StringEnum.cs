using System;
using System.Collections;
using System.Reflection;

namespace Telectronica.Peaje
{

	#region Clase StringEnum

	/// <summary>
	/// Helper class para trabajar con enums usando atributos<see cref="StringValueAttribute"/>.
	/// </summary>
	public class StringEnum
	{
		#region Implementacion

		private Type _enumType;
		private static Hashtable _stringValues = new Hashtable();

		/// <summary>
		/// Crea una nueva instancia de <see cref="StringEnum"/>.
		/// </summary>
		/// <param name="enumType">Enum type.</param>
		public StringEnum(Type enumType)
		{
			if (!enumType.IsEnum)
				throw new ArgumentException(String.Format("El type debe ser un Enum. Type es {0}", enumType.ToString()));

			_enumType = enumType;
		}

		/// <summary>
		/// Obetiene el valor STRING asociado con el enum recibido.
		/// </summary>
		/// <param name="valueName">Nombre del valor enum.</param>
		/// <returns>String Value</returns>
		public string GetStringValue(string valueName)
		{
			Enum enumType;
			string stringValue = null;
			try
			{
				enumType = (Enum) Enum.Parse(_enumType, valueName);
				stringValue = GetStringValue(enumType);
			}
			catch (Exception) { }

			return stringValue;
		}

		/// <summary>
        /// Obetiene el valor STRING asociado con el enum recibido.
		/// </summary>
		/// <returns>Array de valores String</returns>
		public Array GetStringValues()
		{
			ArrayList values = new ArrayList();
			//Buscamos los valores asociados con los campos en el enum
			foreach (FieldInfo fi in _enumType.GetFields())
			{
				//Buscamos por nuestro atributo
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
					values.Add(attrs[0].Value);

			}

			return values.ToArray();
		}

		/// <summary>
		/// Obtiene los valores como una lista bindeable.
		/// </summary>
		/// <returns>IList for data binding</returns>
		public IList GetListValues()
		{
			Type underlyingType = Enum.GetUnderlyingType(_enumType);
			ArrayList values = new ArrayList();
            //Buscamos los valores asociados con los campos en el enum
			foreach (FieldInfo fi in _enumType.GetFields())
			{
                //Buscamos por nuestro atributo
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
					values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(_enumType, fi.Name), underlyingType), attrs[0].Value));

			}

			return values;

		}

		/// <summary>
		/// Retorna un bool definiendo si el valor ingresado existe en el enum.
		/// </summary>
		/// <param name="stringValue">Valor String.</param>
		/// <returns>True si el valor existe</returns>
		public bool IsStringDefined(string stringValue)
		{
			return Parse(_enumType, stringValue) != null;
		}

		/// <summary>
        /// Retorna un bool definiendo si el valor ingresado existe en el enum.
		/// </summary>
        /// <param name="stringValue">Valor String.</param>
		/// <param name="ignoreCase">Establece si respeta el case-sensitive o no</param>
        /// <returns>True si el valor existe</returns>
		public bool IsStringDefined(string stringValue, bool ignoreCase)
		{
			return Parse(_enumType, stringValue, ignoreCase) != null;
		}

		/// <summary>
		/// Retorno el Type de la instancia.
		/// </summary>
		/// <value></value>
		public Type EnumType
		{
			get { return _enumType; }
		}

		#endregion

		#region Implementacion Estatica

        /// <summary>
        /// Obetiene el valor STRING asociado con el enum recibido.
        /// </summary>
        /// <param name="valueName">Nombre del valor enum.</param>
        /// <returns>String Value</returns>
		public static string GetStringValue(Enum value)
		{
			string output = null;
			Type type = value.GetType();

			if (_stringValues.ContainsKey(value))
				output = (_stringValues[value] as StringValueAttribute).Value;
			else 
			{
				FieldInfo fi = type.GetField(value.ToString());
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
				{
					_stringValues.Add(value, attrs[0]);
					output = attrs[0].Value;
				}
					
			}
			return output;

		}

		/// <summary>
		/// Parsea el enum y el valor para ver si encuntra un valor del enum asociado(case sensitive).
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">Valor String.</param>
		/// <returns>Valor del enum asociado con el stringValue o null si no lo encuentra.</returns>
		public static object Parse(Type type, string stringValue)
		{
			return Parse(type, stringValue, false);
		}

        /// <summary>
        /// Parsea el enum y el valor para ver si encuntra un valor del enum asociado(case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">Valor String.</param>
		/// <param name="ignoreCase">Indica si la busqueda es case-sensitive o no</param>
        /// <returns>Valor del enum asociado con el stringValue o null si no lo encuentra.</returns>
		public static object Parse(Type type, string stringValue, bool ignoreCase)
		{
			object output = null;
			string enumStringValue = null;

			if (!type.IsEnum)
				throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

			foreach (FieldInfo fi in type.GetFields())
			{
				StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[];
				if (attrs.Length > 0)
					enumStringValue = attrs[0].Value;

				if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
				{
					output = Enum.Parse(type, fi.Name);
					break;
				}
			}

			return output;
		}

		/// <summary>
        /// Retorna un bool definiendo si el valor ingresado existe en el enum.
		/// </summary>
        /// <param name="stringValue">Valor String.</param>
        /// <param name="enumType">Type del enum</param>
        /// <returns>True si el valor existe</returns>
		public static bool IsStringDefined(Type enumType, string stringValue)
		{
			return Parse(enumType, stringValue) != null;
		}

        /// <summary>
        /// Retorna un bool definiendo si el valor ingresado existe en el enum.
        /// </summary>
        /// <param name="stringValue">Valor String.</param>
        /// <param name="enumType">Type del enum</param>
		/// <param name="ignoreCase">Establece si la busqueda es case-sensitive o no</param>
        /// <returns>True si el valor existe</returns>
		public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
		{
			return Parse(enumType, stringValue, ignoreCase) != null;
		}

		#endregion
	}

	#endregion

	#region Class StringValueAttribute

	/// <summary>
	/// Atributo para guardar valores string.
	/// </summary>
	public class StringValueAttribute : Attribute
	{
		private string _value;

		public StringValueAttribute(string value)
		{
			_value = value;
		}

		public string Value
		{
			get { return _value; }
		}
	}

	#endregion
}