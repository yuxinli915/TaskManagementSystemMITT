﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystemMITT.Models;

namespace TaskManagementSystemMITT.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Task
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // var userId = User.Identity.GetUserId();
            var projectTasks = ProjectHelper.AllTasksByProject(db, (int)id);
            return View(projectTasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = TaskHelper.GetTask((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            var users = UserRoleHelper.AllUsersInRole("Developer");
            var userId = users.Select(i => new SelectListItem() { Text = i.UserName, Value = i.Id.ToString() }).ToList();
            ViewBag.ProjectId = ProjectHelper.AllProjectsByUser(db, User.Identity.GetUserId())
                .Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }).ToList();
            ViewBag.UserId = userId;
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                TaskHelper.CreateTask(projectTask);
                return Redirect("~/Manage/index");
            }
            var userId = User.Identity.GetUserId();
            ViewBag.ProjectId = ProjectHelper.AllProjectsByUser(db, userId);
            ViewBag.UserId = userId;
            return View(projectTask);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = TaskHelper.GetTask((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            var users = UserRoleHelper.AllUsersInRole("Developer");
            var userId = users.Select(i => new SelectListItem() { Text = i.UserName, Value = i.Id.ToString() }).ToList();
            ViewBag.ProjectId = ProjectHelper.AllProjectsByUser(db, User.Identity.GetUserId())
                .Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }).ToList();
            ViewBag.UserId = userId;
            return View(projectTask);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                TaskHelper.EditTask(projectTask);
                return RedirectToAction("Index", "Manage");
            }
            var users = UserRoleHelper.AllUsersInRole("Developer");
            var userId = users.Select(i => new SelectListItem() { Text = i.UserName, Value = i.Id.ToString() }).ToList();
            ViewBag.ProjectId = ProjectHelper.AllProjectsByUser(db, User.Identity.GetUserId())
                .Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }).ToList();
            ViewBag.UserId = userId;
            return View(projectTask);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = TaskHelper.GetTask((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        public ActionResult Comment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = TaskHelper.GetTask((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                TaskHelper.EditTask(projectTask);
                return RedirectToAction("Index", "Manage");
            }

            return View(projectTask);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaskHelper.DeleteTask(id);
            return Redirect("~/Manage/index");
        }


        public ActionResult AddNotes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = TaskHelper.GetTask((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskId = projectTask.Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(string note, int id)
        {
            var task = db.Tasks.Find(id);
            NotificationHelper.CreateManagerTaskNotifications(db, id, note, task.Project.UserId);
            return RedirectToAction("Index", "Manage");
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
