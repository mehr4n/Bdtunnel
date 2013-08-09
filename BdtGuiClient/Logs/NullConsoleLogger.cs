// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Collections.Generic;
using System.Text;

using Bdt.Shared.Logs;
using Bdt.Shared.Configuration;
#endregion

namespace Bdt.GuiClient.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Un logger console fictif: permet juste de contenir les donn�es de config
    /// </summary>
    /// -----------------------------------------------------------------------------
    class NullConsoleLogger : ConsoleLogger
    {

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un log
        /// </summary>
        /// <param name="dateFormat">le format des dates de timestamp</param>
        /// <param name="filter">le niveau de filtrage pour la sortie des logs</param>
        /// -----------------------------------------------------------------------------
        public NullConsoleLogger (string dateFormat, ESeverity filter)
        {
            DateFormat = dateFormat;
            Filter = filter;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entr�e de log: non support�e
        /// </summary>
        /// <param name="sender">l'emetteur</param>
        /// <param name="message">le message � logger</param>
        /// <param name="severity">la s�v�rit�</param>
        /// -----------------------------------------------------------------------------
        public override void Log (object sender, string message, ESeverity severity)
        {
            throw new NotSupportedException();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture du logger
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void Close()
        {
        }

        #endregion

    }
}
