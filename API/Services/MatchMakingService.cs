// MatchmakingService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace API.Services
{

    public class MatchMakingHub : Hub
    {
        public async Task SendMessage(string user, string message)
            => await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public class MatchmakingService
    {
        private readonly Dictionary<RankType, Queue<string>> _buckets; // Dictionary to store bucket IDs and player connection IDs
        private readonly List<FullBucket> _fullBuckets;
        private readonly Queue<string> _dgs;
        private readonly int _bucketSize = 2; // Size of each bucket
        private readonly IHubContext<MatchMakingHub> _hubContext;

        public MatchmakingService(IHubContext<MatchMakingHub> hubContext)
        {
            _buckets = new Dictionary<RankType, Queue<string>>();
            _fullBuckets = new List<FullBucket>();
            _dgs = new Queue<string>();
            _hubContext = hubContext;
        }

        public ServiceResult JoinMatchmaking(string userId, RankType rank)
        {
            // Find the appropriate bucket based on rank

            // Add the player's connection ID to the bucket
            if (!_buckets.ContainsKey(rank))
            {
                _buckets[rank] = new Queue<string>();
            }

            _buckets[rank].Enqueue(userId);

            // Check if the bucket is full
            bool isBucketFull = _buckets[rank].Count >= _bucketSize;
            Console.WriteLine("isBucketfull: " + isBucketFull);
            Console.WriteLine("Any DGS: " + _dgs.Any());
            if (isBucketFull && _dgs.Any())
            {
                List<string> users = new List<string>();
                Console.WriteLine("Bucket is full and server available");
                string serverAddress = _dgs.Dequeue();
                for (int i = 0; i < _bucketSize; i++)
                {
                    var varUserId = _buckets[rank].Dequeue();
                    users.Add(varUserId);
                    //await SendMessageToPlayer(varuserId, serverAddress);
                }
                FullBucket fullBucket = new FullBucket
                {
                    Users = users,
                    DGS = serverAddress
                };

                _fullBuckets.Add(fullBucket);

            }
            // Return the result
            Console.WriteLine("Bucket size: " + _buckets[rank].Count.ToString());
            return ServiceResult.CreateSuccess();
        }


        public ServiceResult AddDgsToMatchmaking(string serverAddress)
        {
            // Check if there are any buckets with enough players
            var fullBuckets = _buckets.Where(bucket => bucket.Value.Count >= _bucketSize).ToList();

            if (fullBuckets.Any())
            {
                Console.WriteLine("fullBucket");
                // Send the server address to each player in the full bucket
                var rank = fullBuckets[0].Key;
                
                // Assuming you have a method to send messages to players
                List<string> users = new List<string>();
                for (int i = 0; i<_bucketSize; i++)
                {
                    var varUserId = _buckets[rank].Dequeue();
                    users.Add(varUserId);
                    //await SendMessageToPlayer(varuserId, serverAddress);
                }
                FullBucket fullBucket = new FullBucket
                {
                    Users = users,
                    DGS = serverAddress
                };

                _fullBuckets.Add(fullBucket);
                

                // Return a success result
                return ServiceResult.CreateSuccess();
            }
            _dgs.Enqueue(serverAddress);
            Console.WriteLine("Any DGS after add:" + _dgs.Any());

            return ServiceResult.CreateSuccess();
        }

        public string? PullUpdateUser(string userId)
        {
            foreach (var fullBucket in _fullBuckets)
            {
                if (fullBucket.ContainsUser(userId))
                {
                    return fullBucket.DGS;
                }
            }
            return null;
        }

        public List<string>? PullUpdateServer(string serverAddress)
        {
            foreach (var fullBucket in _fullBuckets)
            {
                if (fullBucket.ContainsDGS(serverAddress))
                {
                    return fullBucket.Users;
                }
            }
            return null;
        }


        public async Task SendMessageToPlayer(string userId, string message)
        {
            // Use SignalR HubContext to send message to the player
            await _hubContext.Clients.Client(userId).SendAsync("ReceiveMessage", message);
        }
    }   

    public class FullBucket
    {
        public required List<string> Users { get; set; }
        public required string DGS { get; set; }

        public bool ContainsUser(string user)
        {
            return Users.Contains(user);
        }
        public bool ContainsDGS (string dgs)
        {
            return DGS == dgs;
        }
    }
}