using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

using StoreApplication.Biz;
using StoreApplication.Common;

namespace StoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // confirm login information and authenticate if correct
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]Customer cust)
        {
            // shouldn't happen, but just to be safe
            if (cust == null)
            {
                return BadRequest(new { Message = "Something went wrong! Please reload the page and try again." });
            }

            // get customer from DB
            Customer customer = new Customer(cust.Email);

            // is the password correct?
            if (customer.Password == HashTool.HashString(cust.Password))
            {
                // create JSON web token
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfig.JwtSecret));
                SigningCredentials signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:44334",
                    audience: "http://localhost:44334",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signinCredentials
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                // set userid in a cookie so we can make sure authorized users aren't tampering with requests later
                SetCookie("UserId", customer.Id.ToString());

                return Ok(new { Token = tokenString, customer.Id });
            }
            else
            {
                return BadRequest(new { Message = "Invalid email address/password combination." });
            }
        }

        // Create new Customer
        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateAsync([FromBody]Customer cust)
        {
            // populate customer
            Customer customer = new Customer
            {
                Name = cust.Name,
                Email = cust.Email,
                Password = HashTool.HashString(cust.Password)
            };

            try
            {
                // check if the email address supplied already exists, return error if so
                Customer customerCheck = new Customer(cust.Email);

                if (customerCheck.Id > -1)
                {
                    return BadRequest(new { Message = "There is already an account in our system with this email address. Please log in or use a different email address to create your account." });
                }

                // create the new customer
                int id = customer.CreateFromCurrent(null);

                if (id > -1)
                {
                    // it worked, so log them in
                    using (HttpClient client = new HttpClient())
                    {
                        // reset customer password to plain text, to be hashed by login screen
                        customer.Password = cust.Password;

                        // create URI for login request
                        StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                        var request = HttpContext.Request;
                        client.BaseAddress = new UriBuilder
                        {
                            Scheme = request.Scheme,
                            Host = request.Host.Host,
                            Port = request.Host.Port.Value
                        }.Uri;

                        // call login
                        using (var response = await client.PostAsync("/api/customers/login", content))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                var createRes = JsonConvert.DeserializeObject<CreateCustomerResponse>(apiResponse);

                                // set userid in a cookie so we can make sure authorized users aren't tampering with requests later
                                SetCookie("UserId", customer.Id.ToString());

                                return Ok(new { createRes.Token, createRes.Id });
                            }
                            else
                            {
                                return BadRequest(new { Message = "An error occurred while attempting to log you in. Please go to the login page and try again." });
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest(new { Message = "An error occurred while creating your account. Please try again, or contact technical support for assistance." });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // helper function to set cookies
        private void SetCookie(string cookieName, string cookieValue)
        {
            var cookieOption = new CookieOptions();
            cookieOption.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append(cookieName, HashTool.HashString(cookieValue), cookieOption);
        }
    }
}