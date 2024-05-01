# DoubleA_Online
Projet programmation réseau pour le JV




Command to start the API:
docker-compose up --build

Command to initialize the API database:
Bash:
./init.sh
PowerShell
.\init.ps1

The server and clients needs to run on the same machine

To connect players
player 1: 
     username: example
     password: password123
palyer 2:
     username: example2
     password: password123



Connected players can try to find a match.
The matchmaking waits for 2 players to be in the same bucket and for a server to host the game.
When you start the server, it is added to the matchmaking system of the API.
Whenever 2 player and a server are ready to start, the game launch. (server can be started before or after the clients are added to matchmaking).

The achievement button prints the achievements list of the player, by default there are no achievements.
A DGS can add an achievement to a player (see below).












## TEST COMMAND

Register : 

Players:

     curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\", \"email\":\"example@doubleA.com\", \"rank\":\"argent\", \"role\":\"Player\"}" https://localhost:8080/auth/register

     curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example2\", \"password\":\"password123\", \"email\":\"example2@doubleA.com\", \"rank\":\"argent\", \"role\":\"Player\"}" https://localhost:8080/auth/register

DGS:

     curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"exampledgs\", \"password\":\"password123\", \"email\":\"exampledgs@doubleA.com\", \"rank\":\"unranked\", \"role\":\"DGS\"}" https://localhost:8080/auth/register


Login :

Players:
     curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\"}" https://localhost:8080/auth/login    -> Return a token

     curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example2\", \"password\":\"password123\"}" https://localhost:8080/auth/login

DGS:

     curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"exampledgs\", \"password\":\"password123\", \"ipaddress\":\"serverip\"}" https://localhost:8080/auth/login 

Ask for achievements :
     curl -X GET https://localhost:8080/api/achievements -H "Authorization: Bearer your_token"

To grant an achievement (token from a DGS):

          curl -X POST https://localhost:8080/api/achievements/grant -H "Authorization: Bearer DGS_token" -H "Content-Type: application/json" -d "{\"userId\":\"user_uuid\", \"achievementId\":\"achievement_uuid\"}"


To add achievement for a user in pgsql : 
INSERT INTO public.UserAchievements (user_id, achievement_id)
VALUES ('example_user_id', 'example_achievement_id');

To add a Session to Redis :
     curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer your_token" -d "{\"sessionId\":\"YOUR_SESSION_ID\",\"address\":\"YOUR_SESSION_ADDRESS\",\"players\":[\"PLAYER1_ID\",\"PLAYER2_ID\"]}" https://localhost:8080/api/sessions


To add a player to matchmaking :
     curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer YOUR_ACCESS_TOKEN" https://localhost:8080/api/matchmaking/player

To add a server to matchmaking :

     curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer YOUR_ACCESS_TOKEN" https://localhost:8080/api/Matchmaking/dgs


To pull update from matchmaking as player: (return Server IP if game found, null otherwise)

     curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer YOUR_ACCESS_TOKEN" https://localhost:8080/api/matchmaking/player/update

To pull update from matchmaking as dgs:

     curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer YOUR_ACCESS_TOKEN" https://localhost:8080/api/matchmaking/dgs/update



To install docker compose:
https://docs.docker.com/desktop/install/windows-install/

Tu build docker image:
docker build -t docker_image .

To run the docker compose:
docker-compose up

database name : my_api_rest
user name: martin
password : password
