/* BoutDuTunnel Copyright (c) 2007-2010 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

#region " Inclusions "
using System;
using System.Collections;
#endregion

namespace Bdt.Shared.Configuration
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Repr�sente une source de configuration g�n�rique. Permet de servir de base pour l'�laboration
    /// d'autres sources.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class BaseConfig : IComparable
    {

        #region " Constantes "
        public const string SOURCE_PATH_SEPARATOR = "/";
        public const string SOURCE_ITEM_ATTRIBUTE = "@";
        public const string SOURCE_ITEM_EQUALS = "=";
        public const string SOURCE_SCRAMBLED_START = "[";
        public const string SOURCE_SCRAMBLED_END = "]";
        #endregion

        #region " Attributs "
        protected SortedList m_values = new SortedList(); // Les elements class�s par code
        protected int m_priority = 0; // La priorit� de cette source
        #endregion

        #region " Propri�t�s "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/Fixe la priorit� de la source
        /// </summary>
        /// <returns>la priorit� de la source</returns>
        /// -----------------------------------------------------------------------------
        public int Priority
        {
            get
            {
                return m_priority;
            }
            set
            {
                m_priority = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/Fixe la priorit� de la source
        /// </summary>
        /// <returns>la priorit� de la source</returns>
        /// -----------------------------------------------------------------------------
        public string Value(string code, string defaultValue)
        {
            if (m_values.ContainsKey(code))
            {
                return System.Convert.ToString(m_values[code]);
            }
            else
            {
                return defaultValue;
            }
        }
        public void SetValue(string code, string Value)
        {
            if (m_values.ContainsKey(code))
            {
                m_values[code] = Value;
            }
            else
            {
                m_values.Add(code, Value);
            }
        }
        #endregion

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur d'une source avec un d�crypteur de donn�es optionnel
        /// Les valeurs entre SOURCE_SCRAMBLED_START et SOURCE_SCRAMBLED_END seront
        /// automatiquement d�crypt�es
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected BaseConfig(int priority)
        {
            this.Priority = priority;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Force le rechargement de la source de donn�e
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract void Rehash();

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Concat�ne tous les �l�ments depuis cette source
        /// </summary>
        /// <returns>le format de chaque ligne est classe,priorit�,code,valeur</returns>
        /// -----------------------------------------------------------------------------
        public sealed override string ToString()
        {
            string returnValue;
            returnValue = string.Empty;

            foreach (string key in m_values.Keys)
            {
                returnValue += "   <" + this.GetType().Name + "(" + Priority + ")" + "> [" + key + "] " + SOURCE_ITEM_EQUALS + " [" + Value(key, String.Empty) + "]" + "\r\n";
            }
            return returnValue;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Comparateur par priorit�
        /// </summary>
        /// <param name="obj">la config � comparer</param>
        /// <returns>voir IComparable.CompareTo</returns>
        /// -----------------------------------------------------------------------------
        public int CompareTo(object obj)
        {
            if ((obj) is BaseConfig)
            {
                return this.Priority - ((BaseConfig)obj).Priority;
            }
            else
            {
                return 0;
            }
        }
        #endregion

    }
}

