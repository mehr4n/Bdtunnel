/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Collections;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
#endregion

namespace Bdt.Shared.Protocol
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Protocole de communication basé sur le remoting .NET et sur le protocole HTTP
    /// Utilise un formateur binaire pour les données
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class HttpBinaryRemoting : GenericHttpRemoting
    {

        #region " Constantes "
        public const string CFG_NAME = "name";
        public const string CFG_PORT = "port";
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication côté client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override System.Runtime.Remoting.Channels.IChannel ClientChannel
        {
            get
            {
                if (m_clientchannel == null)
                {
                    m_clientchannel = new HttpChannel(new System.Collections.Hashtable(), new BinaryClientFormatterSinkProvider(), null);
                }
                return m_clientchannel;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication côté serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected override System.Runtime.Remoting.Channels.IChannel ServerChannel
        {
            get
            {
                if (m_serverchannel == null)
                {
                    Hashtable properties = new Hashtable();
                    properties.Add(CFG_NAME, string.Format("{0}Channel", Name));
                    properties.Add(CFG_PORT, Port.ToString());
                    m_serverchannel = new HttpChannel(properties, null, new BinaryServerFormatterSinkProvider());
                }
                return m_serverchannel;
            }
        }
        #endregion

    }

}

