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
using System.Collections.Generic;
#endregion

namespace Bdt.Shared.Configuration
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Regroupe un ensemble de sources de configurations prioris�es. Permet de rechercher des
    /// valeurs suivant un code.
    /// Les sources peuvent �tre diff�rentes: base de donn�e, fichier de configuration,
    /// ligne de commande, etc
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class ConfigPackage
    {

        #region " Evenements "
        public delegate void ReadStringEventHandler(ConfigPackage sender, ref string value);
        private ReadStringEventHandler ReadStringEvent;

        public event ReadStringEventHandler ReadString
        {
            add
            {
                ReadStringEvent = (ReadStringEventHandler)System.Delegate.Combine(ReadStringEvent, value);
            }
            remove
            {
                ReadStringEvent = (ReadStringEventHandler)System.Delegate.Remove(ReadStringEvent, value);
            }
        }

        public delegate void ReadIntEventHandler(ConfigPackage sender, ref int value);
        private ReadIntEventHandler ReadIntEvent;

        public event ReadIntEventHandler ReadInt
        {
            add
            {
                ReadIntEvent = (ReadIntEventHandler)System.Delegate.Combine(ReadIntEvent, value);
            }
            remove
            {
                ReadIntEvent = (ReadIntEventHandler)System.Delegate.Remove(ReadIntEvent, value);
            }
        }

        public delegate void ReadBoolEventHandler(ConfigPackage sender, ref bool value);
        private ReadBoolEventHandler ReadBoolEvent;

        public event ReadBoolEventHandler ReadBool
        {
            add
            {
                ReadBoolEvent = (ReadBoolEventHandler)System.Delegate.Combine(ReadBoolEvent, value);
            }
            remove
            {
                ReadBoolEvent = (ReadBoolEventHandler)System.Delegate.Remove(ReadBoolEvent, value);
            }
        }

        #endregion

        #region " Attributs "
        //Les sources sont tri�es par priorit� gr�ce au compareTo de SourceConfiguration (IComparable)
        private List<BaseConfig> m_sources = new List<BaseConfig>();
        #endregion

        #region " Propri�t�s "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne la valeur d'un �l�ment suivant son code
        /// </summary>
        /// <param name="code">le code de l'�l�ment</param>
        /// <param name="defaultValue">la valeur par d�faut si l'�l�ment est introuvable</param>
        /// <returns>La valeur de l'�l�ment s'il existe ou defaultValue sinon</returns>
        /// -----------------------------------------------------------------------------
        public virtual string Value(string code, string defaultValue)
        {
            foreach (BaseConfig source in m_sources)
            {
                string result = source.Value(code, null);
                if (result != null)
                {
                    if (ReadStringEvent != null)
                        ReadStringEvent(this, ref result);
                    return result;
                }
            }
            return defaultValue;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/Retourne la valeur enti�re d'un �l�ment suivant son code
        /// </summary>
        /// <param name="code">le code de l'�l�ment</param>
        /// <param name="defaultValue">la valeur par d�faut si l'�l�ment est introuvable</param>
        /// <returns>La valeur de l'�l�ment s'il existe et s'il repr�sente un entier ou defaultValue sinon</returns>
        /// -----------------------------------------------------------------------------
        public int ValueInt(string code, int defaultValue)
        {
            try
            {
                int result = int.Parse(Value(code, defaultValue.ToString()));
                if (ReadIntEvent != null)
                    ReadIntEvent(this, ref result);
                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/Retourne la valeur bool�enne d'un �l�ment suivant son code
        /// </summary>
        /// <param name="code">le code de l'�l�ment</param>
        /// <param name="defaultValue">la valeur par d�faut si l'�l�ment est introuvable</param>
        /// <returns>La valeur de l'�l�ment s'il existe et s'il repr�sente un bool�en (true/false) ou defaultValue sinon</returns>
        /// -----------------------------------------------------------------------------
        public bool ValueBool(string code, bool defaultValue)
        {
            try
            {
                bool result = bool.Parse(Value(code, defaultValue.ToString()));
                if (ReadBoolEvent != null)
                    ReadBoolEvent(this, ref result);
                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        #endregion

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ajoute une source � ce contexte de configuration
        /// </summary>
        /// <param name="source">la source � ajouter, qui sera class�e par SourceConfiguration.Priority()</param>
        /// -----------------------------------------------------------------------------
        public void AddSource(BaseConfig source)
        {
            m_sources.Add(source);
            m_sources.Sort();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Enleve une source de ce contexte de configuration
        /// </summary>
        /// <param name="source">la source � supprimer</param>
        /// -----------------------------------------------------------------------------
        public void RemoveSource(BaseConfig source)
        {
            m_sources.Remove(source);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Force un rechargement de toutes les sources de donn�es li�es
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void RehashAll()
        {
            foreach (BaseConfig source in m_sources)
            {
                source.Rehash();
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Concat�ne tous les �l�ments depuis toutes les sources
        /// </summary>
        /// <returns>le format de chaque ligne est classe,priorit�,code,valeur</returns>
        /// -----------------------------------------------------------------------------
        public override string ToString()
        {
            string returnValue;
            returnValue = string.Empty;
            foreach (BaseConfig source in m_sources)
            {
                returnValue += source.ToString();
            }
            return returnValue;
        }
        #endregion

    }

}

