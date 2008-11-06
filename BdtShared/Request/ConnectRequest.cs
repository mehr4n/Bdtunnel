// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Request
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une demande de connexion
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct ConnectRequest : ISessionContextRequest 
    {

        #region " Attributs "
        private string m_address;
        private int m_port;
        private int m_sid;
        #endregion

        #region " Proprietes "
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

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'adresse distante
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Address
        {
            get
            {
                return m_address;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le port distant
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Port
        {
            get
            {
                return m_port;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="sid">Le jeton de session</param>
        /// <param name="address">L'adresse distante</param>
        /// <param name="port">Le port distant</param>
        /// -----------------------------------------------------------------------------
        public ConnectRequest(int sid, string address, int port)
        {
            this.m_sid = sid;
            this.m_address = address;
            this.m_port = port;
        }
        #endregion

    }

}

