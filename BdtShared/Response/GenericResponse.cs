// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Response
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une réponse générique
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct GenericResponse : IGenericResponse
    {

        #region " Attributs "
        private bool m_success;
        private string m_message;
        private bool m_dataAvailable;
        private bool m_connected;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Des données sont-elles disponibles?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public bool DataAvailable
        {
            get
            {
                return m_dataAvailable;
            }
            set
            {
                m_dataAvailable = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La connexion est-elle effective?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public bool Connected
        {
            get
            {
                return m_connected;
            }
            set
            {
                m_connected = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La requête a aboutie/échoué ?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public bool Success
        {
            get
            {
                return m_success;
            }
            set
            {
                m_success = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le message d'information
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Message
        {
            get
            {
                return m_message;
            }
            set
            {
                m_message = value;
            }
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">La connexion a aboutie/échoué</param>
        /// <param name="message">Le message d'information</param>
        /// -----------------------------------------------------------------------------
        public GenericResponse (bool success, string message)
        {
            this.m_connected = false;
            this.m_dataAvailable = false;
            this.m_success = success;
            this.m_message = message;
        }
        #endregion

    }

}


