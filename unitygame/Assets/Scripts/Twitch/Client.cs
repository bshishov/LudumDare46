using System;
using System.Collections.Generic;
using Gameplay;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Client.Services;
using UnityEngine;

namespace Twitch
{
    public class Client
    {
        public event Action<User, string> OnUserMessage;
        public event Action<User, string, List<string>> OnUserCommand;
        
        private readonly TwitchLib.Unity.Client _client;
        private readonly UserDb _db;

        public Client(UserDb db)
        {
            _db = db;
            var credentials = new ConnectionCredentials(Secrets.BotUsername, Secrets.BotAccessToken);

            _client = new TwitchLib.Unity.Client();
            _client.Initialize(credentials, Secrets.ChannelName, '!', '!', true);

            _client.Connect();
            _client.OnConnected += ClientOnOnConnected;
            _client.OnConnectionError += ClientOnOnConnectionError;
            _client.OnMessageReceived += ClientOnOnMessageReceived;
            _client.OnUserJoined += ClientOnUserJoined;
            _client.OnUserLeft += ClientOnUserLeft;
            _client.OnUserTimedout += ClientOnUserTimedout;
            _client.OnChatCommandReceived += ClientOnOnChatCommandReceived;
            
            _client.OnLeftChannel += ClientOnOnLeftChannel;
            _client.OnDisconnected += ClientOnOnDisconnected;
        }

        private void ClientOnOnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Debug.Log($"[Client] Connection error: {e.Error.Message}");
        }

        private void ClientOnOnConnected(object sender, OnConnectedArgs e)
        {
            Debug.Log($"[Client] Connected. Automatically joined channel: {e.AutoJoinChannel}");
        }

        private void ClientOnOnDisconnected(object sender, OnDisconnectedArgs e)
        {
            Debug.Log($"[Client] Disconnected, reconnecting");
            _client.Reconnect();
        }

        private void ClientOnOnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            Debug.Log($"[Client] Left channel {e.Channel}, reconnecting");
            _client.Reconnect();
        }

        private void ClientOnOnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            Debug.Log($"[Client] Chat command {e.Command.ChatMessage.Username}: {e.Command.CommandText} args = [ {string.Join(", ", e.Command.ArgumentsAsList)} ]");
            var command = e.Command.CommandText;
            var userName = e.Command.ChatMessage.Username;
            
            if(string.IsNullOrEmpty(command))
               return; 
            
            var user = _db.GetOrCreate(userName);
            user.IsActive = true;
            user.LastInteraction = Time.time;
            
            user.IsModerator = e.Command.ChatMessage.IsModerator;
            user.IsBroadcaster = e.Command.ChatMessage.IsBroadcaster;
            user.IsSubscriber = e.Command.ChatMessage.IsSubscriber;
            
            var args = e.Command.ArgumentsAsList;
            
            OnUserCommand?.Invoke(user, command, args);
        }

        private void ClientOnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            var user = _db.GetOrCreate(e.UserTimeout.Username);
            user.IsActive = false;
            Debug.Log($"[Client] {e.UserTimeout.Username} timed out");
        }

        private void ClientOnUserLeft(object sender, OnUserLeftArgs e)
        {
            var user = _db.GetOrCreate(e.Username);
            user.IsActive = false;
            Debug.Log($"[Client] {e.Username} left");
        }

        private void ClientOnOnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var userName = e.ChatMessage.Username;
            var message = e.ChatMessage.Message.Trim();
            Debug.Log($"[Client] {userName}: {message}");
            
            var user = _db.GetOrCreate(userName);
            user.IsActive = true;
            user.LastInteraction = Time.time;
            
            user.IsModerator = e.ChatMessage.IsModerator;
            user.IsBroadcaster = e.ChatMessage.IsBroadcaster;
            user.IsSubscriber = e.ChatMessage.IsSubscriber;
            
            OnUserMessage?.Invoke(user, message);
        }

        private void ClientOnUserJoined(object sender, OnUserJoinedArgs e)
        {
            var user = _db.GetOrCreate(e.Username);
            user.IsActive = true;
            user.LastInteraction = Time.time;
            
            Debug.Log($"[Client] {e.Username} joined");
        }

        public void SendMessage(string text)
        {
            if (!_client.IsConnected)
            {
                Debug.LogWarning($"[Client] Trying to send message \"{text}\" but client is not connected, reconnecting");
                _client.Reconnect();
                return;
            }
            
            if (_client.JoinedChannels.Count == 0)
            {
                Debug.LogWarning($"[Client] Trying to send message \"{text}\" but client has not joined any channels, reconnecting");
                _client.Reconnect();
                return;
            }
            
            try
            {
                _client.SendMessage(_client.JoinedChannels[0], text);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Client] Failed to send message: \"{text}\", got following exception");
                Debug.LogException(e);
            }
        }
    }
}