using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

using StoreApplication.Biz;
using StoreApplication.Common;

namespace StoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        // GET api/values for specific user
        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("user/{id}")]
        public IActionResult GetForUser(int id)
        {
            // make sure the user isn't trying to get someone else's purchases
            if (HashTool.HashString(id.ToString()) == Request.Cookies["UserId"])
            {
                Purchase purchase = new Purchase();
                List<Purchase> list = purchase.ListForUserId(id);
                return Ok(new { list });
            }
            else
            {
                return BadRequest(new { Message = "Something went wrong! Please log out and try again." });
            }
        }

        // Create new Purchase
        [HttpPost, Route("create"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create([FromBody]Purchase pur)
        {
            // make sure the user isn't trying to make a purchase on someone else's account
            if (HashTool.HashString(pur.UserId.ToString()) == Request.Cookies["UserId"])
            {
                // populate purchase
                Purchase purchase = new Purchase
                {
                    UserId = pur.UserId,
                    ProductId = pur.ProductId
                };

                try
                {
                    // create purchase
                    int id = purchase.CreateFromCurrent(null);

                    if (id > -1)
                    {
                        // everything's good!
                        return Ok();
                    }
                    else
                    {
                        // DB insert failed
                        return BadRequest(new { Message = "An error occurred while creating your purchase. Please try again, or contact technical support for assistance." });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(new { Message = "Something went wrong! Please log out and try again." });
            }
        }

        // delete purchase
        [HttpDelete, Route("{id}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id)
        {
            // populate purchase from DB
            Purchase purchase = new Purchase(id);

            // only delete purchases associated to the logged in user
            if (HashTool.HashString(purchase.UserId.ToString()) != Request.Cookies["userId"])
            {
                return BadRequest(new { Message = "Something went wrong! Please log out and try again." });
            }

            try
            {
                // delete it
                purchase.Delete(null, id);

                return Ok();
            }
            catch
            {
                return BadRequest(new { Message = "An error occurred while deleting your purchase. Please try again, or contact technical support for assistance." });
            }
        }
    }
}