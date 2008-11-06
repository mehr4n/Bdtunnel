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
    /// Une réponse générique de login
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct LoginResponse : IMinimalResponse 
    {

        #region " Attributs "
        private int m_sid;
        private bool m_success;
        private string m_message;
        #endregion

        #region " Proprietes "
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
        /// Le jeton de session
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Sid
        {
            get
            {
                return m_sid;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">Login réussi/échoué</param>
        /// <param name="message">Le message d'information</param>
        /// <param name="sid">Le jeton de session affecté</param>
        /// -----------------------------------------------------------------------------
        public LoginResponse(bool success, string message, int sid)
        {
            this.m_success = success;
            this.m_message = message;
            this.m_sid = sid;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">Login réussi/échoué</param>
        /// <param name="sid">Le jeton de session affecté</param>
        /// -----------------------------------------------------------------------------
        public LoginResponse(bool success, int sid)
        {
            this.m_success = success;
            this.m_message = string.Empty;
            this.m_sid = sid;
        }
        #endregion

    }

}


