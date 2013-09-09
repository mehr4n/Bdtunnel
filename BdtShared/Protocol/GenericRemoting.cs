/* BoutDuTunnel Copyright (c)  2007-2013 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

#region " Inclusions "
using System;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;

using Bdt.Shared.Resources;
using Bdt.Shared.Service;
using Bdt.Shared.Logs;
using System.Collections;
#endregion

namespace Bdt.Shared.Protocol
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Protocole de communication bas� sur le remoting .NET
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class GenericRemoting<T> : GenericProtocol where T: IChannel
    {

        #region " Constantes "
	    private const string CfgName = "name";
	    private const string CfgPortName = "portName";
	    private const string CfgPort = "port";
        #endregion

        #region " Attributs "
        protected T ClientChannelField;
        protected T ServerChannelField;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication c�t� serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected abstract T ServerChannel
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication c�t� client
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected abstract T ClientChannel
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'URL n�cessaire pour se connecter au serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected abstract string ServerURL
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le service est-il s�curis�
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected virtual bool IsSecured
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region " M�thodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Configuration c�t� client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void ConfigureClient()
        {
            Log(string.Format(Strings.CONFIGURING_CLIENT, GetType().Name, ServerURL), ESeverity.DEBUG);
            ChannelServices.RegisterChannel(ClientChannel, IsSecured);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// D�-configuration c�t� client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UnConfigureClient()
        {
            Log(string.Format(Strings.UNCONFIGURING_CLIENT, GetType().Name), ESeverity.DEBUG);
            ChannelServices.UnregisterChannel(ClientChannel);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Configuration c�t� serveur
        /// </summary>
        /// <param name="type">le type d'objet � rendre distant</param>
        /// -----------------------------------------------------------------------------
        public override void ConfigureServer(Type type)
        {
            Log(string.Format(Strings.CONFIGURING_SERVER, GetType().Name, Port), ESeverity.INFO);
            ChannelServices.RegisterChannel(ServerChannel, IsSecured);
            var wks = new WellKnownServiceTypeEntry(type, Name, WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(wks);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// D�configuration c�t� serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UnConfigureServer()
        {
            Log(string.Format(Strings.UNCONFIGURING_SERVER, GetType().Name, Port), ESeverity.INFO);
            ChannelServices.UnregisterChannel(ServerChannel);
	        var channel = ServerChannel as IChannelReceiver;
	        if (channel != null)
                channel.StopListening(null);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne une instance de tunnel
        /// </summary>
        /// <returns>une instance de tunnel</returns>
        /// -----------------------------------------------------------------------------
        public override ITunnel GetTunnel()
        {
            return ((ITunnel)Activator.GetObject(typeof(ITunnel), ServerURL));
        }

	    protected Hashtable CreateClientChannelProperties()
        {
            var properties = new Hashtable {{CfgName, string.Format("{0}.Client", Name)}};
		    properties.Add(CfgPortName, properties[CfgName]);
            return properties;
        }

	    protected Hashtable CreateServerChannelProperties()
        {
            var properties = new Hashtable {{CfgName, Name}, {CfgPort, Port.ToString(CultureInfo.InvariantCulture)}};
		    properties.Add(CfgPortName, properties[CfgName]);
            return properties;
        }

        #endregion

    }

}

