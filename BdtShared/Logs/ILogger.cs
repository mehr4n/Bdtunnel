// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.IO;

using Bdt.Shared.Configuration;
#endregion

namespace Bdt.Shared.Logs
{
    #region " Enumerations "
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Severité d'une entrée de log, Filter permet de donner un niveau minimum
    /// </summary>
    /// -----------------------------------------------------------------------------
    public enum ESeverity
    {
        DEBUG = 1,
        INFO = 2,
        WARN = 3,
        @ERROR = 4,
        FATAL = 5
    }
    #endregion

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Interface minimale pour la génération d'un log
    /// </summary>
    /// -----------------------------------------------------------------------------
    public interface ILogger
    {

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="sender">l'emetteur</param>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        void Log(object sender, string message, ESeverity severity);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture du logger
        /// </summary>
        /// -----------------------------------------------------------------------------
        void Close();
        #endregion

    }

}

