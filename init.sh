#!/bin/bash

curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example\", \"password\":\"password123\", \"email\":\"example@doubleA.com\", \"rank\":\"argent\", \"role\":\"Player\"}" https://localhost:8080/auth/register

curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"example2\", \"password\":\"password123\", \"email\":\"example2@doubleA.com\", \"rank\":\"argent\", \"role\":\"Player\"}" https://localhost:8080/auth/register

curl -X POST -H "Content-Type: application/json" -d "{\"username\":\"exampledgs\", \"password\":\"password123\", \"email\":\"exampledgs@doubleA.com\", \"rank\":\"unranked\", \"role\":\"DGS\"}" https://localhost:8080/auth/register