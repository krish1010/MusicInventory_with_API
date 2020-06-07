using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MusicInventory.Models;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace MusicInventory.Controllers
{
    public class Repository
    {
        //private MusicInventoryDbEntities db = new MusicInventoryDbEntities();
        public static List<Music> cartList = new List<Music>();
        public static int grand_total = 0;

        internal async Task<dynamic> ToList()
        {
            var musics = await GetMusicAsync();
            return musics;
        }

        private static async Task<dynamic> GetMusicAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Musics");//t3

                response.EnsureSuccessStatusCode(); //Break

                if (response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsAsync<IEnumerable<Music>>();
                    return msg;
                }
                else
                {
                    return null;
                }
            }

        }


        internal async Task<dynamic> Find(int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/musics/getmusic/"+id);//t3

                response.EnsureSuccessStatusCode(); //Break

                if (response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsAsync<Music>();
                    return msg;
                }
                else
                {
                    return null;
                }
            }
        }

        internal async Task<dynamic> GetMusicByGenre(string id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Musics/getmusicsbygenre/"+id);//t3

                response.EnsureSuccessStatusCode(); //Break

                if (response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsAsync<IEnumerable<Music>>();
                    return msg;
                }
                else
                {
                    return null;
                }
            }
        }

        internal async Task<dynamic> GetAllGenres()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response =  await client.GetAsync("api/Musics/getallgenres").ConfigureAwait(false);//t3

                response.EnsureSuccessStatusCode(); //Break

                if (response.IsSuccessStatusCode)
                {
                    var syncContext = SynchronizationContext.Current;
                    SynchronizationContext.SetSynchronizationContext(null);
                    var msg =  response.Content.ReadAsAsync<IEnumerable<string>>();
                    SynchronizationContext.SetSynchronizationContext(syncContext);

                    return msg;
                }
                else
                {
                    return null;
                }
            }
        }

        internal async Task<dynamic> AddToPurchaseCart(int id)
        {
            if(cartList.Find((x)=>x.Id==id)==null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44305/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response =  client.GetAsync("api/musics/getmusic/"+id).Result;//t3

                    response.EnsureSuccessStatusCode(); //Break

                    if (response.IsSuccessStatusCode)
                    {
                        var msg = await response.Content.ReadAsAsync<Music>();
                        cartList.Add(msg);
                    }
                    else
                    {

                    }
                }

            }
            else
            {
                var ob = cartList.Find((x) => x.Id == id);
                ob.Quantity += 1;
            }
            return cartList;
        }

        internal List<Music> ShowCart()
        {
            return cartList;
        }

        internal List<Music> Delete(int id)
        {
            Music ob = cartList.Find((x)=>x.Id==id);
            if(ob.Quantity==1)
            {
                cartList.Remove(ob);
                
            }
            else
            {
                --ob.Quantity;
            }
            
            
            return cartList;
        }

        internal List<Music> CheckOut()
        {
            grand_total = 0;
            foreach (var item in cartList)
                grand_total += Convert.ToInt32(item.Price*item.Quantity);
            return cartList;
        }

        internal void DeleteAll()
        {
            cartList.Clear();
        }
    }
}