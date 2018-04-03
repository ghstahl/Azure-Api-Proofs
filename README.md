# Azure-Api-Proofs

```
query q($input: authInput!) {
  auth(input: $input) {
    type
    token
  }
}
{
   "input": {
		"type": "test",
    "token": "tokenTest"
   }
}
```
