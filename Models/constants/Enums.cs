using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTP_Project.Models.constants
{

    public enum TaskStatus { Initial, InProgress, Completed};
    public enum ProjectStatus { Initial, InProgress, Completed, Rejected};
    public enum OrderStatus { Initial, InProgress, Rejected, Completed};
    public enum TemplateSiteTypes { Blog, VisitCard, ECommerce, Forum, Enterprise};
}
