// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Bdt.Shared.Logs;
using Bdt.Shared.Configuration;
using Bdt.GuiClient.Runtime;
#endregion

namespace Bdt.GuiClient.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Un logger li� � un NotifyIcon pour la sortie des erreurs uniquement
    /// </summary>
    /// -----------------------------------------------------------------------------
    class NotifyIconLogger : BaseLogger
    {

        #region " Attributs "
        protected BdtGuiClient m_guiclient = null;
        protected string m_tipTitle = null;
        protected int m_timeout = 0;
        #endregion

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="prefix">le prefixe dans la configuration ex: application/log</param>
        /// <param name="config">la configuration pour la lecture des parametres</param>
        /// <param name="guiclient">le client bdt</param>
        /// <param name="tipTitle">le titre � utiliser</param>
        /// <param name="timeout">le timeout d'affichage</param>
        /// -----------------------------------------------------------------------------
        public NotifyIconLogger(string prefix, ConfigPackage config, BdtGuiClient guiclient, string tipTitle, int timeout)
            : base(null, prefix, config)
        {
            // on utilise le r�f�rence d'un BdtGuiClient au lieu de passer directement un NotifyIcon car � ce stade
            // on ne peut pas cr�er de formulaire, car la Culture serait incorrecte, le fichier de configuration
            // n'�tant pas d�j� pars�
            m_guiclient = guiclient;
            m_tipTitle = tipTitle;
            m_timeout = timeout;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entr�e de log. Ne sera actif que pour une s�v�rit� � ERREUR
        /// </summary>
        /// <param name="sender">l'emetteur</param>
        /// <param name="message">le message � logger</param>
        /// <param name="severity">la s�v�rit�</param>
        /// -----------------------------------------------------------------------------
        public override void Log(object sender, string message, ESeverity severity)
        {
            if ((severity == ESeverity.ERROR) && (m_guiclient != null) && (m_guiclient.MainComponent != null) && (m_guiclient.MainComponent.NotifyIcon != null))
            {
                m_guiclient.MainComponent.NotifyIcon.ShowBalloonTip(m_timeout, m_tipTitle, message, ToolTipIcon.Error);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture du logger
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void Close()
        {
            m_guiclient = null;
        }
        #endregion

    }
}
