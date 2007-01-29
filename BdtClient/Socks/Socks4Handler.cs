// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;

using Bdt.Shared.Logs;
using Bdt.Client.Resources;
#endregion

namespace Bdt.Client.Socks
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Gestionnaire Socks v4 (sans DNS)
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class Socks4Handler : GenericSocksHandler
    {

        #region " Constantes "
        public const int SOCKS4_REPLY_VERSION = 0;
        public const int SOCKS4_OK = 90; // Requête acceptée
        public const int SOCKS4_KO = 91; // Requête refusée
        public const int SOCKS4_CONNECT_COMMAND = 1; // commande CONNECT
        public const int SOCKS4_BIND_COMMAND = 2; // commande BIND (non supportée)
        public const int REPLY_SIZE = 8; // octets de réponse
        #endregion

        #region " Attributs "
        protected byte[] m_reply = new byte[REPLY_SIZE];
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
                m_reply[0] = SOCKS4_REPLY_VERSION;
                m_reply[1] = SOCKS4_KO;
                Array.Clear(m_reply, 2, 6);

                if (Version != 4)
                {
                    return false;
                }

                // Teste pour basic Socks4 (pas Socks4a)
                if ((Buffer[4] != 0) || (Buffer[5] != 0) || (Buffer[6] != 0))
                {
                    if (Command != SOCKS4_BIND_COMMAND)
                    {
                        RemotePort = 256 * Convert.ToInt32(Buffer[2]) + Convert.ToInt32(Buffer[3]);
                        Address = Buffer[4] + "." + Buffer[5] + "." + Buffer[6] + "." + Buffer[7];
                        // Préparation de la réponse
                        m_reply[1] = SOCKS4_OK;
                        Array.Copy(Buffer, 2, m_reply, 2, 6);
                        Log(Strings.SOCKS4_REQUEST_HANDLED, ESeverity.DEBUG);
                        return true;
                    }
                    else
                    {
                        // Socks4 BIND
                        Log(Strings.SOCKS_BIND_UNSUPPORTED, ESeverity.WARN);
                    }
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

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="buffer"></param>
        /// -----------------------------------------------------------------------------
        public Socks4Handler(byte[] buffer)
            : base(buffer)
        {
            Version = buffer[0];
            Command = buffer[1];
        }
        #endregion

    }
}

