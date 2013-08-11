/* BoutDuTunnel Copyright (c) 2007-2010 Sebastien LEBRETON

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
using Bdt.Shared.Configuration;
#endregion

namespace Bdt.Tests.Configuration
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Regroupe un ensemble de sources de configurations prioris�es. Permet de rechercher des
    /// valeurs suivant un code.
    /// Les sources peuvent �tre diff�rentes: base de donn�e, fichier de configuration,
    /// ligne de commande, etc
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class TestConfigPackage : ConfigPackage
    {
        #region " Constantes "
	    private const string UserLogin = "usertest";
	    private const string UserPassword = "userpassword";
        public const string UserDisabledLogin = "userdisabled";
        public const string UserDisabledPassword = "userdisabledpassword";
        public const string UserLambdaLogin = "userlambda";
        public const string UserLambdaPassword = "userlambdapassword";
        #endregion

        #region " Attributs "
        private readonly string _protocol = "";
        private readonly string _port = "";
        #endregion

        #region " Propri�t�s "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne la valeur d'un �l�ment suivant son code
        /// </summary>
        /// <param name="code">le code de l'�l�ment</param>
        /// <param name="defaultValue">la valeur par d�faut si l'�l�ment est introuvable</param>
        /// <returns>La valeur de l'�l�ment s'il existe ou defaultValue sinon</returns>
        /// -----------------------------------------------------------------------------
        public override string Value(string code, string defaultValue)
        {
            if (code.StartsWith("forward/")) {
                if (code.EndsWith("@enabled") || code.EndsWith("@shared"))
                     return "false";

                return "0";
            }

            switch (code)
            {
                case "service@username": return UserLogin;
                case "service@password": return UserPassword;
                case "service@protocol": return _protocol;
                case "service@name": return "BdtTestService";
                case "service@port": return _port;
                case "service@address": return "localhost";
                case "service@culture": return string.Empty;

                case "users/usertest@password": return "userpassword";
                case "users/usertest@enabled": return "true";
                case "users/usertest@admin": return "true";

                case "users/userdisabled@password": return UserDisabledPassword;
                case "users/userdisabled@enabled": return "false";

                case "users/userlambda@password": return UserLambdaPassword;
                case "users/userlambda@enabled": return "true";
                case "users/userlambda@admin": return "false";
                
                case "socks@enabled": return "false";
                case "socks@shared": return "false";
                case "socks@port": return "1080";

                case "proxy@enabled": return "false";
                case "proxy/configuration@auto": return "false";
                case "proxy/configuration@address": return string.Empty;
                case "proxy/configuration@port": return string.Empty;
                case "proxy/authentification@auto": return "false";
                case "proxy/authentification@password": return string.Empty;
                case "proxy/authentification@username": return string.Empty;
                case "proxy/authentification@domain": return string.Empty;

                default: return string.Empty; // throw new ArgumentException(code);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="protocol">le protocole � utiliser</param>
        /// <param name="port">le port de base</param>
        /// -----------------------------------------------------------------------------
        public TestConfigPackage(String protocol, int port)
        {
            _protocol = protocol;
            _port = port.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

    }

}

