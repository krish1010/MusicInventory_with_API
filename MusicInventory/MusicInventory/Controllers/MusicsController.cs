using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MusicInventory.Models;
using Newtonsoft.Json.Linq;

namespace MusicInventory.Controllers
{
    public class MusicsController : Controller
    {
        MusicList musicList = new MusicList();
        Repository ob = new Repository();
        // GET: Musics

        public async Task<ActionResult> Index(string id)
        {
            if (id == null)
                return View(await ob.ToList());
            else
                return View(await ob.GetMusicByGenre(id));
        }

        // GET: Musics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Music music = await ob.Find(id);
            if (music == null)
            {
                return HttpNotFound();
            }
            return View(music);
        }

        public async Task<ActionResult> DisplayAllGenres()
        {
            var temp = ob.GetAllGenres().Result;
            var res = temp.Result;
            return PartialView("DisplayAllGenres",res);
        }


        public async Task<ActionResult> AddToCart(int id)
        {
            if(Session["Music"]!=null)
            {
                MusicList musicList = (MusicList)Session["Music"];
                musicList.musicList=ob.AddToPurchaseCart(id).Result;
                Session["Music"] = musicList;
            }
            else
            {
                MusicList musicList = new MusicList();
                musicList.musicList = ob.AddToPurchaseCart(id).Result;
                Session["Music"] = musicList;
            }

            return View();
        }
        public ActionResult ShowCart()
        {

            if(Request.IsAjaxRequest())
            {
                if (Session["Music"] != null)
                {
                    musicList = (MusicList)Session["Music"];
                    musicList.musicList = ob.ShowCart();
                    Session["Music"] = musicList;
                }
                else
                {
                    musicList = new MusicList();
                    musicList.musicList = ob.ShowCart();
                    Session["Music"] = musicList;
                }
                return PartialView("ShowCart", musicList);
            }
            else
            {
                if (Session["Music"] != null)
                {
                    musicList = (MusicList)Session["Music"];
                    musicList.musicList = ob.ShowCart();
                    Session["Music"] = musicList;
                }
                else
                {
                    musicList = new MusicList();
                    musicList.musicList = ob.ShowCart();
                    Session["Music"] = musicList;
                }
                return View("ShowCart", musicList);
            }
        }
            
        /*[HttpPost]*/
        public ActionResult DeleteFromCart(int id)
        {
            musicList.musicList = ob.Delete(id);
            return RedirectToAction("ShowCart", musicList);
        }
        public ActionResult CheckOut()
        {
            musicList.musicList = ob.CheckOut();
            return View("CheckOut", musicList);
        }
        public async Task<ActionResult> DeleteAll()
        {
            ob.DeleteAll();
            return View("Index",await ob.ToList());
        }
    }
}