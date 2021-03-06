﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using JabboServer.Updater;
using JabboServer.Core;
using JabboServer.Game;
using JabboServer.Game.Users;
using JabboServer.Game.Rooms;
using JabboServer.Game.Rooms.Chatlogs;
using JabboServer.Net.WebSocket;
using JabboServer.Messages.WebSocket;

namespace JabboServer
{
    internal class Engine
    {
        private static Interface Interface;
        private static Config Config;

        private static WebSocket WebSocket;

        private static RoomManager RoomManager;
        private static string WelcomeMessage = "";

        internal static void Initialize(Interface GUI)
        {
            Interface = GUI;

            if (UpdateChecker.NeedsUpdate())
            {
                Helpers.WriteLine("There is a update availible.", false);
                Helpers.WriteLine("Please update the emulator through SVN,", false);
                Helpers.WriteLine("Or you won't be able to use JabboServer", false);

                return;
            }

            Helpers.WriteLine("Credits to:", false);
            Helpers.WriteLine("- PEjump2 for coding 'the base' and GUI.", false);
            Helpers.WriteLine("- joopie for coding 'the base' of the Packet structure.", false);
            Helpers.WriteLine("- TopErwin for fixing exploits.", false);
            Helpers.WriteLine("- wichard for recoding the whole JabboServer.", false);
            Helpers.WriteLine(Environment.NewLine, false);

            Config = new Config();
            Config.Initialize();

            WelcomeMessage = Config.GetValue("hotelalert.text");

            WebSocket = new WebSocket(Config.GetValue("websocket.ip"), int.Parse(Config.GetValue("websocket.port")), int.Parse(Config.GetValue("websocket.max.connections")));
            WebSocket.Request();

            RoomManager = new RoomManager();
            RoomManager.InitRooms();

            Helpers.WriteLine("Succesfully Initialized JabboServer.");

            UpdateTitle();
        }

        internal static void UpdateTitle()
        {
            Interface.SetTitle(GetConsoleTitle);
        }

        internal static void Dispose()
        {
            if (Config != null)
            {
                Config = null;
                Helpers.WriteLine("Destroyed Config.");
            }

            if (WebSocket != null)
            {
                WebSocket.Dispose();
                WebSocket = null;
                Helpers.WriteLine("Destroyed WebSocket.");
            }

            if (RoomManager != null)
            {
                RoomManager.GetChatlogManager().Save();
                RoomManager.Dispose();
                RoomManager = null;
                Helpers.WriteLine("Destroyed RoomManager.");
            }

            Environment.Exit(0);
        }

        internal static string GetConsoleTitle
        {
            get
            {
                return "JabboServer [C#] - Users online: " + GetWebSocket().ConnectionCount + " - Rooms loaded: " + RoomManager.RoomCount();
            }
        }

        internal static Response GetWelcomeMessage
        {
            get
            {
                Response Response = new Response();

                Response.Init("hotelAlert");
                Response.AppendObject(WelcomeMessage);

                return Response;
            }
        }

        internal static bool HasWelcomeMessage
        {
            get
            {
                return (WelcomeMessage != "");
            }
        }

        internal static Config GetConfig()
        {
            return Config;
        }

        internal static WebSocket GetWebSocket()
        {
            return WebSocket;
        }

        internal static RoomManager GetRoomManager()
        {
            return RoomManager;
        }

        internal static Interface GetInterface()
        {
            return Interface;
        }
    }
}
