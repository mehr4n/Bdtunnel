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
    /// Une réponse de connexion
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct ConnectResponse : IConnectionContextResponse 
    {

        #region " Attributs "
        private bool m_success;
        private string m_message;
        private bool m_dataAvailable;
        private bool m_connected;
        private int m_cid;
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

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le jeton de connexion affecté
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Cid
        {
            get
            {
                return m_cid;
            }
            set
            {
                m_cid = value;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">La connexion a aboutie/échoué</param>
        /// <param name="message">Le message d'information</param>
        /// <param name="cid">Le jeton de connexion affecté</param>
        /// -----------------------------------------------------------------------------
        public ConnectResponse(bool success, string message, int cid)
        {
            this.m_connected = false;
            this.m_dataAvailable = false;
            this.m_success = success;
            this.m_message = message;
            this.m_cid = cid;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">La requête a aboutie/échoué</param>
        /// <param name="cid">Le jeton de connexion affecté</param>
        /// -----------------------------------------------------------------------------
        public ConnectResponse(bool success, int cid)
        {
            this.m_connected = false;
            this.m_dataAvailable = false;
            this.m_success = success;
            this.m_message = string.Empty;
            this.m_cid = cid;
        }
        #endregion

    }

}


