# DoubleA_Online
Projet programmation réseau pour le JV

Installation des packages :

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore

to run the app :
dotnet run 

Register : 
curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\", \"email\":\"example@doubleA.com\", \"role\":\"Player\"}" http://localhost:5204/auth/register

curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"exampledgs\", \"password\":\"password123\", \"email\":\"exampledgs@doubleA.com\", \"role\":\"DGS\"}" http://localhost:5204/auth/register


Login :
curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\"}" http://localhost:5204/auth/login    -> Return a token

curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"exampledgs\", \"password\":\"password123\"}" http://localhost:5204/auth/login 

Ask for achievements :
curl -X GET http://localhost:5204/api/achievements -H "Authorization: Bearer your_token"

To grant an achievement (token from a DGS):

     curl -X POST http://localhost:5204/api/achievements/grant -H "Authorization: Bearer DGS_token" -H "Content-Type: application/json" -d "{\"userId\":\"user_uuid\", \"achievementId\":\"achievement_uuid\"}"


To add achievement for a user in pgsql : 
INSERT INTO public.UserAchievements (user_id, achievement_id)
VALUES ('example_user_id', 'example_achievement_id');



database name : my_api_rest
user name: martin
password : password
