﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace TaskManagementSystemMITT.Models
{
    public class ProjectTask
    {
        public ProjectTask()
        {
            Notifications = new HashSet<Notification>();
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        public int PercentCompleted { get; set; }
        public string Comment { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        
        public string UserId { get; set; }
        public bool IsCompleted { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public Priority Priority { get; set; }
        public double GetCost(int id)
        {
            return BudgetHelper.GetTotalCostForTask(db, id);
        }
        public void MarkCompleted(int id)
        {
            TaskHelper.MarkCompleted(db, id);
        }
    }

    public enum Priority
    {
        High = 3,
        Medium = 2,
        Low = 1
    }
}