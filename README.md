# Azure-Api-Proofs

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
