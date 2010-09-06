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
using System.ComponentModel;
#endregion

namespace Bdt.Shared.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Classe de base pour un objet utilisant un flux de log
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable(), TypeConverter(typeof(ExpandableObjectConverter))]
    public class LoggedObject : ILogger
    {

        #region " Attributs "
        protected static BaseLogger m_globalLogger = null;
        protected DateTime startmarker = DateTime.Now;
        protected BaseLogger m_logger = null;
        #endregion

        #region " Propri�t�s "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/retourne le loggueur assoc� � cet objet
        /// </summary>
        /// <returns>le loggueur assoc� � cet objet</returns>
        /// -----------------------------------------------------------------------------
        public BaseLogger Logger
        {
            get
            {
                return m_logger;
            }
            set
            {
                m_logger = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/retourne le loggueur assoc� � tous les objets d�riv�s.
        /// </summary>
        /// <returns>le loggueur assoc� � tous les objets d�riv�s</returns>
        /// -----------------------------------------------------------------------------
        public static BaseLogger GlobalLogger
        {
            get
            {
                return m_globalLogger;
            }
            set
            {
                m_globalLogger = value;
            }
        }
        #endregion

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entr�e de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="message">le message � logger</param>
        /// <param name="severity">la s�v�rit�</param>
        /// -----------------------------------------------------------------------------
        public virtual void Log(string message, ESeverity severity)
        {
            Log(this, message, severity);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entr�e de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="message">le message � logger</param>
        /// <param name="severity">la s�v�rit�</param>
        /// -----------------------------------------------------------------------------
        public virtual void Log(object sender, string message, ESeverity severity)
        {
            if (m_logger != null)
            {
                m_logger.Log(sender, message, severity);
            }
            if (m_globalLogger != null)
            {
                m_globalLogger.Log(sender, message, severity);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture du logger
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void Close()
        {
            if (m_logger != null)
            {
                m_logger.Close();
            }
        }
        #endregion

    }

}

