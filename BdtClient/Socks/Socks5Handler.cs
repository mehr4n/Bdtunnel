/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net.Sockets;

using Bdt.Shared.Logs;
using Bdt.Client.Resources;
#endregion

namespace Bdt.Client.Socks
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Gestionnaire Socks v5
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class Socks5Handler : GenericSocksHandler
    {

        #region " Constantes "
        // Méthodes d'authentication
        public const int SOCKS5_NO_AUTHENTICATION_REQUIRED = 0;
        public const int SOCKS5_GSSAPI = 1; // Non supportée
        public const int SOCKS5_USERNAME_PASSWORD = 2; // Non supportée
        public const int SOCKS5_NO_ACCEPTABLE_METHODS = 255;

        // Commandes
        public const int SOCKS5_CONNECT_COMMAND = 1;
        public const int SOCKS5_BIND_COMMAND = 2;
        public const int SOCKS5_UDP_ASSOCIATE_COMMAND = 3;

        // Types d'adresses
        public const int SOCKS5_IPV4 = 1;
        public const int SOCKS5_DOMAIN = 3;
        public const int SOCKS5_IPV6 = 4;

        // Responses
        public const int SOCKS5_OK = 0;
        public const int SOCKS5_KO = 1;
        public const int SOCKS5_REPLY_VERSION = 5;

        public const int REPLY_SIZE = 10; // octets de réponse
        #endregion

        #region " Attributs "
        protected byte[] m_reply = new byte[REPLY_SIZE];
        protected NetworkStream m_stream;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le handler est-il adapté à la requête?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override bool IsHandled
        {
            get
            {
                // Préparation pessimiste de la réponse
                m_reply[0] = SOCKS5_REPLY_VERSION;
                m_reply[1] = SOCKS5_KO;
                m_reply[2] = 0;
                m_reply[3] = SOCKS5_IPV4;
                Array.Clear(m_reply, 4, 6);

                if (Version != 5)
                {
                    return false;
                }

                int numMethods = Buffer[1];
                bool methodAccepted = false;

                if ((numMethods <= 0) || (numMethods != Buffer.Length - 2))
                {
                    Log(Strings.SOCKS5_MALFORMED_METHOD_ENUM, ESeverity.WARN);
                    return false;
                }

                byte[] handshake = new byte[2];
                int i = 0;
                while ((i < numMethods) && (!methodAccepted))
                {
                    methodAccepted = (Buffer[i + 2] == SOCKS5_NO_AUTHENTICATION_REQUIRED);
                    i += 1;
                }
                handshake[0] = 5; // version

                if (!methodAccepted)
                {
                    // Ecriture du handshake
                    Log(Strings.SOCKS5_NO_AUTHENTICATION_SUPPORTED, ESeverity.WARN);
                    handshake[1] = SOCKS5_NO_ACCEPTABLE_METHODS;
                    m_stream.Write(handshake, 0, handshake.Length);
                    return false;
                }

                // Ecriture du handshake
                handshake[1] = SOCKS5_NO_AUTHENTICATION_REQUIRED;
                m_stream.Write(handshake, 0, handshake.Length);

                // Lecture de la requete de connexion
                byte[] request = new byte[BUFFER_SIZE];
                m_stream.Read(request, 0, request.Length);

                Version = request[0];
                Command = request[1];
                int addressType = request[3];

                if (Version != 5)
                {
                    Log(Strings.SOCKS5_BAD_VERSION, ESeverity.WARN);
                    return false;
                }

                switch (Command)
                {
                    case SOCKS5_CONNECT_COMMAND:
                        switch (addressType)
                        {
                            case SOCKS5_IPV4:
                                RemotePort = 256 * Convert.ToInt32(request[8]) + Convert.ToInt32(request[9]);
                                Address = request[4] + "." + request[5] + "." + request[6] + "." + request[7];
                                // Préparation de la réponse
                                m_reply[1] = SOCKS5_OK;
                                Array.Copy(request, 4, m_reply, 4, 6);
                                Log(Strings.SOCKS5_REQUEST_HANDLED, ESeverity.DEBUG);
                                return true;
                            //break;
                            case SOCKS5_DOMAIN:
                                int length = request[4];
                                Address = new string(System.Text.Encoding.ASCII.GetChars(request), 5, length);
                                RemotePort = 256 * Convert.ToInt32(request[length + 5]) + Convert.ToInt32(request[length + 6]);
                                // Préparation de la réponse
                                m_reply[1] = SOCKS5_OK;
                                Array.Clear(m_reply, 4, 6);
                                Log(Strings.SOCKS5_REQUEST_HANDLED, ESeverity.DEBUG);
                                return true;
                            //break;
                            case SOCKS5_IPV6:
                                Log(Strings.SOCKS5_IPV6_UNSUPPORTED, ESeverity.WARN);
                                break;
                            default:
                                Log(Strings.SOCKS5_ADDRESS_TYPE_UNKNOWN, ESeverity.WARN);
                                break;
                        }
                        break;
                    case SOCKS5_BIND_COMMAND:
                        Log(Strings.SOCKS_BIND_UNSUPPORTED, ESeverity.WARN);
                        break;
                    case SOCKS5_UDP_ASSOCIATE_COMMAND:
                        Log(Strings.SOCKS5_UDP_UNSUPPORTED, ESeverity.WARN);
                        break;
                    default:
                        Log(Strings.SOCKS5_UNKNOWN_COMMAND, ESeverity.WARN);
                        break;
                }

                return false;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Les données de réponse
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override byte[] Reply
        {
            get
            {
                return m_reply;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="client">Le client TCP</param>
        /// <param name="buffer">Les données de la requête</param>
        /// -----------------------------------------------------------------------------
        public Socks5Handler(TcpClient client, byte[] buffer)
            : base(buffer)
        {
            Version = buffer[0];
            Command = buffer[1];
            m_stream = client.GetStream();
        }
        #endregion

    }
}
