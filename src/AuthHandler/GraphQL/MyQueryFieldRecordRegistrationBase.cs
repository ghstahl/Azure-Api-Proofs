using System;
using System.Collections.Generic;
using System.Linq;
using AuthHandler.Models;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace AuthHandler.GraphQL
{
    public class MyQueryFieldRecordRegistrationBase : IQueryFieldRecordRegistration
    {
        private IBindStore _bindStore;

        public MyQueryFieldRecordRegistrationBase(
            IBindStore bindStore)
        {
            _bindStore = bindStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            var fieldName = "bind";
            var fieldType = queryCore.FieldAsync<BindResultType>(name: fieldName,
                description: null,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<BindInput>> {Name = "input"}),
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var input = context.GetArgument<BindInputHandle>("input");
                       
                        var result = await _bindStore.BindAsync(input.Type,input.Token);
                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
            fieldName = "myAuth";
            fieldType = queryCore.FieldAsync<IdentityModelType>(name: fieldName,
                description: null,
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var result = new AuthHandler.Models.IdentityModel();
                        result.Claims = new List<ClaimHandle>();
                        foreach (var claim in userContext.HttpContextAccessor.HttpContext.User.Claims)
                        {
                            result.Claims.Add(new ClaimHandle()
                            {
                                Name = claim.Type,
                                Value = claim.Value
                            });
                        }
                        return result;
                    }
                    catch (Exception e)
                    {

                    }
                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
        }
    }
}
