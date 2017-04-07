using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTP_Project.Models.constants
{

    public enum TaskStatus { Initial, InProgress, OnHold, Completed};
    public enum ProjectStatus { Initial, InProgress, OnHold, Completed};
    public enum OrderStatus { Initiating, Processiong,Canceled,Rejected, Completed};
    public enum TemplateSiteTypes { Blog, VisitCard, Oficial, Shop, Amazing };
}
