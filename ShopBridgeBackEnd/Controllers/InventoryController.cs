using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DataAccessLayer;

namespace ShopBridgeBackEnd.Controllers
{
    public class InventoryController : ApiController
    {
        [HttpGet]
        public async Task<IEnumerable<Item>> GetItems()
        {
            using (ShopBridgeEntities entities = new ShopBridgeEntities())
            {
                return await entities.Items.ToListAsync();
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertItem([FromBody] Item detail)
        {
            try
            {
                using (ShopBridgeEntities entites = new ShopBridgeEntities())
                {
                    entites.Items.Add(detail);
                    await entites.SaveChangesAsync();
                    var message = Request.CreateResponse(HttpStatusCode.Created, detail);
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

        [HttpPut()]        
        public async Task<HttpResponseMessage> UpdateItem(int id,[FromBody] Item detail)
        {
            try
            {                
                using (ShopBridgeEntities entites = new ShopBridgeEntities())
                {
                    var entity = await entites.Items.FirstOrDefaultAsync(d => d.ItemId == id);
                    if(entity==null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item with id " + id + " not found");
                    }
                    else
                    {
                        entity.ItemName = detail.ItemName;
                        entity.Price = detail.Price;
                        entity.ItemDescription = detail.ItemDescription;
                        entites.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, detail);
                        return message;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteItem(int id)
        {
            try
            {
                using (ShopBridgeEntities entites = new ShopBridgeEntities())
                {
                    var entity = await entites.Items.FirstOrDefaultAsync(d => d.ItemId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item with id " + id + " not found");
                    }
                    else
                    {
                        entites.Items.Remove(entity);
                        entites.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK,"Item deleted successfully");
                        return message;
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
    }
}
