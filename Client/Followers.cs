using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Net;
using System.Timers;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

namespace Twacker
{
    class Follower
    {
        public string Name { get; set; }
        public DateTime FollowDate { get; set; }
        public DateTime UnfollowDate { get; set; }


        // It might be a better idea to use the person's ID instead of their name
        // but you can't change names so the point seems moot.
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Name == ((Follower)obj).Name;
        }
    }

    class Followers
    {
        string _server_url = Properties.Settings.Default.TwitchAPIServer;
        string _followersUrl;
        
        List<Follower> _followerCache;
        
        int _timeoutMins = Properties.Settings.Default.FollowersBufferMinutes;

        DateTime _nullDate = new DateTime();
        Follower _nullFollower = new Follower();

        System.Timers.Timer _timer;

        public Followers(string channel)
        {
            _followersUrl = _server_url + @"/channels/" + channel + "/follows?direction=desc&limit=100&offset=";
            processFollowers();
        }

        public void Start()
        {
            Stop();
            configureTimer();
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }

        public List<Follower> FollowerList()
        {
            string url = _followersUrl;
            List<Follower> followers = new List<Follower>();

            WebClient client = new WebClient();

            int total = 0;
            do {
                total = getFollowers(client, ref url, ref followers);
            } while (followers.Count < total);

            client.Dispose();

            return followers;
        }

        private void processFollowers()
        {
            if (_followerCache == null)
            {
                _followerCache = FollowerList();
                return;
            }
            List<Follower> followers = FollowerList();

            // Time stamps are UTC
            DateTime utcNow = DateTime.UtcNow;
            DateTime cutoff = utcNow.AddMinutes(_timeoutMins * -1);

            // add/update followers
            foreach (Follower follower in followers)
            {
                Follower found = _followerCache.Where(f => f.Name == follower.Name).FirstOrDefault<Follower>();

                if (found == null)
                {
                    _followerCache.Add(follower);
                    welcomeNewFollower(follower);
                }
                else
                {
                    found.FollowDate = follower.FollowDate;

                    if (found.UnfollowDate != _nullDate)
                    {
                        found.UnfollowDate = _nullDate;
                    }
                }
            }
            
            // get the followers that are in the cache but not in the twitch follower list
            List<Follower> unfollowed = _followerCache.Except<Follower>(followers).ToList<Follower>();

            // These are done in two steps for debugging.
            // In the final version you can get rid of
            // the intermidiary List<Follower> objects.

            // not in the current followers but don't have an unfollow set so these are new
            List<Follower> newlyUnfollowed = _followerCache.Join
                <Follower, Follower, string, Follower>
                (unfollowed, uf => uf.Name, f => f.Name, (uf, f) => uf)
                .Where<Follower>(d => d.UnfollowDate == _nullDate).ToList<Follower>();

            // remember unfollows for later deletion
            newlyUnfollowed.ForEach(f =>
            { f.UnfollowDate = utcNow; });

            // Remove unfollows that are older than the cuttoff
            List<Follower> deletable = _followerCache.Join
                <Follower, Follower, string, Follower>
                (unfollowed, uf => uf.Name, f => f.Name, (uf, f) => uf)
                .Where<Follower>(d => d.UnfollowDate < cutoff).ToList<Follower>();
            deletable.ForEach(d => _followerCache.Remove(d));
        
        }

        private void welcomeNewFollower(Follower newFollower)
        {
            Speech.Say("Thanks for the follow " + newFollower.Name + ". You are an awesome man and or woman.");
        }

        private void configureTimer()
        {
            _timer = new System.Timers.Timer(15000);
            _timer.Elapsed += new ElapsedEventHandler(timerFired);
            _timer.Enabled = true;
        }

        void timerFired(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            processFollowers();
            _timer.Enabled = true;
        }

        private int getFollowers(WebClient client, ref string url, ref List<Follower> followers)
        {
            string raw_json = client.DownloadString(url);

            JObject followers_json = JObject.Parse(raw_json);
            
            int total = (int)followers_json["_total"];
            url = (string)followers_json["_links"]["next"];

            List<Follower> linq_followers =
                (from f in followers_json["follows"]
                 select new Follower {
                     Name = (string)f["user"]["name"],
                     FollowDate = DateTime.Parse((string)f["created_at"])
                 }).ToList();

            followers.AddRange(linq_followers);
            return total;
        }

    }
}
