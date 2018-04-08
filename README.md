# Azure-Api-Proofs

## Altair
This is a GraphQL IDE allows editing of a header.  You can find it [here](https://chrome.google.com/webstore/detail/altair-graphql-client/flnheeellpciglgpaodhkhmapeljopja?hl=en)  


# Bind 

## Query
```
query q($input: bindInput!){
  bind(input: $input){
    type
    token
    sPOCEntity
  }
}
```
```
{
   "input": {
      "type": "test",
      "token": "tokenTest"
   }
}

```
## Result  
```
{
  "data": {
    "bind": {
      "type": "test",
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYm9iIiwiZXhwIjoxNTIzMTkxNjQzLCJpc3MiOiJ5b3VyZG9tYWluLmNvbSIsImF1ZCI6InlvdXJkb21haW4uY29tIn0.nXyqQ32vVhrPpYFfTUuNOg93cGxpSkQYk1_P0erOM-k",
      "sPOCEntity": "053b1663-5756-4fd7-850b-370985a6898f"
    }
  }
}
```

# Verify the bind  
Remember to add the header.
```
Authorization: Bearer {token}
```
## Query
```
query qMyAuth{
  identity{
    claims{
      ... ClaimFragment
    }
  }
}

fragment ClaimFragment on claim {
 	name
  value 
}
```
## Result  
```
{
  "data": {
    "identity": {
      "claims": [
        {
          "name": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
          "value": "bob"
        },
        {
          "name": "exp",
          "value": "1523197262"
        },
        {
          "name": "iss",
          "value": "yourdomain.com"
        },
        {
          "name": "aud",
          "value": "yourdomain.com"
        }
      ]
    }
  }
}
```
