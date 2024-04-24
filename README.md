# DoubleA_Online
Projet programmation réseau pour le JV

Installation des packages :

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore

to run the app :
dotnet run 

Register : 
curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\", \"roleId\":1}" http://localhost:5204/auth/register   

Login :
curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\"}" http://localhost:5204/auth/login    -> Return a token

Ask for achievements :
curl -X GET http://localhost:5204/api/achievements -H "Authorization: Bearer your_token"

database name : my_api_rest
user name: martin
password : password
