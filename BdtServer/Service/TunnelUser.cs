// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net.Sockets;

using Bdt.Shared.Service;
using Bdt.Shared.Request;
using Bdt.Shared.Response;
#endregion

namespace Bdt.Server.Service
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Un utilisateur au sein du tunnel
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class TunnelUser
    {

        #region " Attributs "
        protected int m_uid;
        protected string m_username;
        protected DateTime m_logon;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le user-id associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Uid
        {
            get
            {
                return m_uid;
            }
            set
            {
                m_uid = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le nom associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Username
        {
            get
            {
                return m_username;
            }
            set
            {
                m_username = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La date de login
        /// </summary>
        /// -----------------------------------------------------------------------------
        public DateTime Logon
        {
            get
            {
                return m_logon;
            }
            set
            {
                m_logon = value;
            }
        }
        #endregion

    }

}
