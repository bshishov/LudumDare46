using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gameplay;
using UnityEngine;
using Utils;

namespace Twitch
{
    public class UserDb
    {
        private readonly Dictionary<string, User> _users 
            = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);

        public User GetOrCreate(string name)
        {
            name = CleanUserName(name);
            if (_users.TryGetValue(name, out var user))
                return user;
            
            var newUser = new User(name, Common.InitialUserBalance, Common.InintialWorkPower);
            _users[name] = newUser;
            return newUser;
        }

        public User GetUser(string name)
        {
            if (_users.TryGetValue(CleanUserName(name), out var user))
                return user;

            return null;
        }

        private string CleanUserName(string raw)
        {
            if (!string.IsNullOrEmpty(raw) && raw[0] == '@')
                return raw.Substring(1).ToLowerInvariant();
            return raw.ToLowerInvariant();
        }

        public void Save(string path)
        {
            try
            {
                File.WriteAllText(path, JsonHelper.ToJson(_users.Values.ToArray(), true));
                Debug.Log($"[DB] Users are saved to {path}");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public void Load(string path)
        {
            try
            {
                _users.Clear();
                var users = JsonHelper.FromJson<User>(File.ReadAllText(path));                
                foreach (var user in users)
                {
                    user.OnLoad();
                    _users.Add(user.Name, user);
                }
                Debug.Log($"[DB] Users are loaded from {path}");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public IEnumerable<User> GetRichest(int n = 10)
        {
            return _users.Values.OrderByDescending(u => u.Balance).Take(n);
        }

        public int CountActive()
        {
            return _users.Values.Count(u => u.IsActive);
        }

        public IEnumerable<User> GetActiveUsers()
        {
            return _users.Values.Where(u => u.IsActive);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users.Values;
        }
    }
}