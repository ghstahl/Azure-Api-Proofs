using System;
using System.Collections.Generic;
using AuthHandler.Models;
using GraphQL;
using P7.GraphQLCore;

namespace AuthHandler.GraphQL.Query
{
    public class IdentityQuery : IQueryFieldRecordRegistration
    {

        private IBindStore _bindStore;
        public IdentityQuery(IBindStore bindStore)
        {
            _bindStore = bindStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<IdentityModelType>(name: "identity",
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
