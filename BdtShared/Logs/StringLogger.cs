// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.IO;
using System.Text;

using Bdt.Shared.Configuration;
#endregion

namespace Bdt.Shared.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Génération des logs dans une chaine
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class StringLogger : BaseLogger
    {

        #region " Attributs "
        private string m_lastline = string.Empty;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne le texte complet loggé dans cet objet
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Text
        {
            get
            {
                return ((StringWriter)m_writer).GetStringBuilder().ToString();
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne la dernière ligne de log
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string LastLine
        {
            get
            {
                return m_lastline;
            }
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un log à partir des données fournies dans une configuration
        /// </summary>
        /// <param name="prefix">le prefixe dans la configuration ex: application/log</param>
        /// <param name="config">la configuration pour la lecture des parametres</param>
        /// -----------------------------------------------------------------------------
        public StringLogger(string prefix, Bdt.Shared.Configuration.ConfigPackage config)
            : base(new StringWriter(), prefix, config)
        {
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un log
        /// </summary>
        /// <param name="dateFormat">le format des dates de timestamp</param>
        /// <param name="filter">le niveau de filtrage pour la sortie des logs</param>
        /// -----------------------------------------------------------------------------
        public StringLogger(string dateFormat, ESeverity filter)
            : base(new StringWriter(), dateFormat, filter)
        {
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="sender">l'emetteur</param>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        public override void Log(object sender, string message, ESeverity severity)
        {
            StringBuilder sb = ((StringWriter)m_writer).GetStringBuilder();
            int index = sb.Length;
            base.Log(sender, message, severity);
            if ((m_enabled) && (severity >= m_filter))
            {
                m_lastline = sb.ToString(index, sb.Length - index);
            }
        }
        #endregion

    }

}
