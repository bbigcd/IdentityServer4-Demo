using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace QuickstartIdentityServer {
    public class Config {
        public static IEnumerable<IdentityResource> GetIdentityResources () {
            return new IdentityResource[] {
                new IdentityResources.OpenId ()
            };
        }

        public static IEnumerable<ApiResource> GetApis () {
            return new List<ApiResource> {
                // new ApiResource ("api1", "My API")
                new ApiResource ("api1", "My API", new List<string> { JwtClaimTypes.Role, "Username" })
            };
        }

        public static IEnumerable<Client> GetClients () {
            return new List<Client> {
                new Client () {
                    ClientId = "client",
                        // no interactive user, use the clientid/secret for authentication
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        // secret for authentication
                        ClientSecrets = {
                            new Secret ("secret".Sha256 ())
                            },
                            // scopes that client has access to
                            AllowedScopes = { "api1" }
                            },
                            // 新的客户端 密码授权方式
                            new Client () {
                            ClientId = "ro.client",
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            ClientSecrets = {
                            new Secret ("secret".Sha256 ())
                            },
                            AllowedScopes = { "api1" }
                            }
            };
        }
        
        // 用户数据
        public static List<TestUser> GetUsers () {
            return new List<TestUser> () {
                new TestUser () {
                        SubjectId = "1",
                            Username = "alice",
                            Password = "password",
                            Claims = new List<Claim> () {
                                new Claim (JwtClaimTypes.Role, "superadmin"),
                                new Claim ("Username", "alice")
                                }
                                },
                                new TestUser () {
                                SubjectId = "2",
                                Username = "bob",
                                Password = "password",
                                Claims = new List<Claim> () {
                                new Claim (JwtClaimTypes.Role, "admin"),
                                new Claim ("Username", "bob")
                                }
                                }
            };
        }
    }
}