﻿/* BoutDuTunnel Copyright (c)  2007-2013 Sebastien LEBRETON

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
using System.Collections.Generic;

using Bdt.Shared.Logs;
using Bdt.Shared.Service;
#endregion

namespace Bdt.Client.Commands
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une commande sur la ligne de commande
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class Command
    {
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Récuperation de l'ensemble des commandes disponibles
        /// </summary>
        /// <returns>un tableau de commandes</returns>
        /// -----------------------------------------------------------------------------
        protected static IEnumerable<Command> GetCommands()
        {
            var result = new List<Command>
	        {
		        new HelpCommand(),
		        new KillConnectionCommand(),
		        new KillSessionCommand(),
		        new MonitorCommand()
	        };
	        return result.ToArray();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Les switchs disponibles pour l'appel de la commande
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract string Switch
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'aide pour la commande
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract string Help
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Les noms des paramètres de la commande
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract string[] ParametersName
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Lance la commande
        /// </summary>
        /// <param name="parameters">les parametres</param>
        /// <param name="logger">le logger</param>
        /// <param name="tunnel">le tunnel</param>
        /// <param name="sid">le jeton de session</param>
        /// -----------------------------------------------------------------------------
        public abstract void Execute(string[] parameters, ILogger logger, ITunnel tunnel, int sid);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Trouve puis execute une commande
        /// </summary>
        /// <param name="args">l'ensemble des arguments</param>
        /// <param name="logger">le logger</param>
        /// <param name="tunnel">le tunnel</param>
        /// <param name="sid">le jeton de session</param>
        /// <returns>true si la commande a été trouvée</returns>
        /// -----------------------------------------------------------------------------
        public static bool FindAndExecute(string[] args, ILogger logger, ITunnel tunnel, int sid)
        {
            if (args.Length > 0)
            {
                var sw = args[0];
                var parameters = new string[args.Length - 1];
                Array.ConstrainedCopy(args, 1, parameters, 0, parameters.Length);

                foreach (var cmd in GetCommands())
                {
	                if ((sw != cmd.Switch) || (parameters.Length != cmd.ParametersName.Length)) 
						continue;
	                
					cmd.Execute(parameters, logger, tunnel, sid);
	                return true;
                }
            }

            return false;
        }

    }
}

