/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System.Net.Sockets;

using Bdt.Shared.Logs;
using Bdt.Shared.Service;
using Bdt.Client.Resources;
#endregion

namespace Bdt.Client.Sockets
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Serveur pour les forwards
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class GatewayServer : TcpServer
    {

        #region " Attributs "
        protected ITunnel m_tunnel;
        protected string m_address;
        protected int m_remoteport;
        protected int m_uid;
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="localport">port local côté client</param>
        /// <param name="shared">bind sur toutes les ip/ip locale</param>
        /// <param name="remoteport">port distant côté serveur</param>
        /// <param name="address">adresse côté serveur</param>
        /// <param name="tunnel">tunnel de communication</param>
        /// <param name="uid">user-id</param>
        /// -----------------------------------------------------------------------------
        public GatewayServer(int localport, bool shared, int remoteport, string address, ITunnel tunnel, int uid)
            : base(localport, shared)
        {
            Log(string.Format(Strings.FORWARDING, Ip, localport, address, remoteport), ESeverity.INFO);

            m_tunnel = tunnel;
            m_uid = uid;
            m_address = address;
            m_remoteport = remoteport;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Callback en cas de nouvelle connexion
        /// </summary>
        /// <param name="client">le socket client</param>
        /// -----------------------------------------------------------------------------
        protected override void OnNewConnection(TcpClient client)
        {
            new Gateway(client, m_tunnel, m_uid, m_address, m_remoteport);
        }
        #endregion

    }

}

