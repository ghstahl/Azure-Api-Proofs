# Auth Configurations

## A Crazy Query
```
query q($idValue: String!){
  hero {
    ... IdFragment
    ... NameFragment  
    ... AppearsInFragment 
  }
  human(id:$idValue){
    ... IdFragment
    ... NameFragment  
    ... AppearsInFragment 
  }
}
fragment AppearsInFragment on Character {appearsIn}
fragment IdFragment on Character {id}
fragment NameFragment on Character {name}
```

```
{
  "idValue":"1"
}
```

This query is really 2 in one, and reuses fragments.

## The Auth Config Entry
```
{
  "operationType": "query",
  "fieldPath": "/human/appearsIn",
  "claims": []
}
```
What this states is that you must be authenticated to see what a human appearsIn.  i.e. Enter your header Authorization: Bearer {token}.

## The error
```
{"errors":[{"message":"You are not authorized to run this query.","locations":[{"line":13,"column":42}]}]}
```

Line 13 is where the fragment AppearsInFragment is.  So the hint show you that it has something to do with that.


## Make it work Un-Authorized
If you want the query to go through in an Un-Authorized state, simply remove the ... AppearsInFragment  from the human query;
```
human(id:$idValue){
    ... IdFragment
    ... NameFragment  
  }
```
results in the following;
```
{
  "data": {
    "hero": {
      "id": "3",
      "name": "R2-D2",
      "appearsIn": [
        "NEWHOPE",
        "EMPIRE",
        "JEDI"
      ]
    },
    "human": {
      "id": "1",
      "name": "Luke"
    }
  }
}
```

NOTE: The way the graphql works is that we don't give away why we failed.  We simply fail everything.


