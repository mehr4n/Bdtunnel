// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net.Sockets;

using Bdt.Server.Resources;
using Bdt.Shared.Logs;
#endregion

namespace Bdt.Server.Service
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une connexion au sein du tunnel
    /// </summary>
    /// -----------------------------------------------------------------------------
    internal class TunnelConnection : TimeoutObject 
    {
        #region " Attributs "
        protected TcpClient m_tcpClient;
        protected NetworkStream m_stream;
        protected int m_readcount;
        protected int m_writecount;
        protected string m_host;
        protected string m_address;
        protected int m_port;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'adresse distant
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Address
        {
            get
            {
                return m_address;
            }
            set
            {
                m_address = value;
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
            set
            {
                m_port = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'hôte distant
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Host
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le client TCP associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public TcpClient TcpClient
        {
            get
            {
                return m_tcpClient;
            }
            set
            {
                m_tcpClient = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le flux associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public NetworkStream Stream
        {
            get
            {
                return m_stream;
            }
            set
            {
                m_stream = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le nombre d'octets lus
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int ReadCount
        {
            get
            {
                return m_readcount;
            }
            set
            {
                m_readcount = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le nombre d'octets écrits
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int WriteCount
        {
            get
            {
                return m_writecount;
            }
            set
            {
                m_writecount = value;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="timeoutdelay">valeur du timeout</param>
        /// -----------------------------------------------------------------------------
        public TunnelConnection(int timeoutdelay) : base(timeoutdelay)
        {
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Timeout de l'objet
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected override void Timeout(ILogger logger)
        {
            logger.Log(this, String.Format(Strings.CONNECTION_TIMEOUT, TcpClient.Client.RemoteEndPoint), ESeverity.INFO);
            SafeDisconnect();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture de la connexion
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void SafeDisconnect()
        {
            try
            {
                if (Stream != null)
                {
                    Stream.Flush();
                    Stream.Close();
                }
            }
            catch (Exception) { }

            try
            {
                if (TcpClient != null)
                {
                    TcpClient.Close();
                }
            }
            catch (Exception) { }

            Stream = null;
            TcpClient = null;
        }
        #endregion

    }

}
