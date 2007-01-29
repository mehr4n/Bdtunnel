// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Collections.Generic;
#endregion

namespace Bdt.Shared.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Classe permettant la propagation de logs sur de multiples flux
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class MultiLogger : BaseLogger
    {

        #region " Attributs "
        private List<ILogger> m_loggers = new List<ILogger>();
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un flux multiple
        /// </summary>
        /// -----------------------------------------------------------------------------
        public MultiLogger()
            : base(null, "dd/MM/yyyy HH:mm:ss", ESeverity.DEBUG)
        {
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ajoute un logger à ce flux multiple
        /// </summary>
        /// <param name="logger"></param>
        /// -----------------------------------------------------------------------------
        public void AddLogger(ILogger logger)
        {
            m_loggers.Add(logger);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Supprime le logger de ce flux multiple
        /// </summary>
        /// <param name="logger"></param>
        /// -----------------------------------------------------------------------------
        public void RemoveLogger(ILogger logger)
        {
            m_loggers.Remove(logger);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose. L'écriture sera propagée à tous les flux associés
        /// </summary>
        /// <param name="sender">l'emetteur</param>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        public override void Log(object sender, string message, ESeverity severity)
        {
            if (Enabled)
            {
                foreach (ILogger logger in m_loggers)
                {
                    logger.Log(sender, message, severity);
                }
            }
        }
        #endregion

    }

}
