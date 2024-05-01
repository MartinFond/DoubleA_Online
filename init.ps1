Invoke-WebRequest -Uri "https://localhost:8080/auth/register" `
    -Method Post `
    -ContentType "application/json" `
    -Body "{`"username`":`"example`", `"password`":`"password123`", `"email`":`"example@doubleA.com`", `"rank`":`"argent`", `"role`":`"Player`"}"

Invoke-WebRequest -Uri "https://localhost:8080/auth/register" `
    -Method Post `
    -ContentType "application/json" `
    -Body "{`"username`":`"example2`", `"password`":`"password123`", `"email`":`"example2@doubleA.com`", `"rank`":`"argent`", `"role`":`"Player`"}"

Invoke-WebRequest -Uri "https://localhost:8080/auth/register" `
    -Method Post `
    -ContentType "application/json" `
    -Body "{`"username`":`"exampledgs`", `"password`":`"password123`", `"email`":`"exampledgs@doubleA.com`", `"rank`":`"unranked`", `"role`":`"DGS`"}"