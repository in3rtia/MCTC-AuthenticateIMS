using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthenticateIMS.Extensions;
using AuthenticateIMS.Models;
using System.Data.Entity;

namespace AuthenticateIMS.Controllers
{
    public class WorkflowController : Controller
    {
        private StockManagementEntities db = new StockManagementEntities();
        // GET: Workflow
        public void Approve(int id, string approvalComment)
        {
            var user = User.Identity;
            var department = "Services";
            

            if (user.IsSupervisorApprover())
            {
                var request = db.Request_Details.Find(id);
                var nextApprover = db.Approvers.Where(x => x.department == department).Select(x => x.mine_number).SingleOrDefault();

                request.approval_status = 5;
                request.approver = nextApprover;
                request.workflow_ID = 6;
                request.comment = approvalComment;

                db.Entry(request).State = EntityState.Modified;
                InsertApproval(request.id, user.GetMineNumber());
                db.SaveChanges();

                //Include function to send email to next approver so as to remind them that they have a request that needs their attention
            }
            else if(user.IsManagerApprover())
            {
                var request = db.Request_Details.Find(id);

                request.approval_status = 2;
                request.workflow_ID = 7;
                request.comment = approvalComment;

                db.Entry(request).State = EntityState.Modified;
                InsertApproval(request.id, user.GetMineNumber());
                db.SaveChanges();

                //Include function to send email to next approver so as to remind them that they have a request that needs their attention
            }
            else if (user.IsAdminUser())
            {
                var request = db.Request_Details.Find(id);
                var nextApprover = db.Approvers.Where(x => x.department == department).Select(x => x.mine_number).SingleOrDefault();

                if(request.workflow_ID == 5)
                {
                    request.approval_status = 5;
                    request.approver = nextApprover;
                    request.workflow_ID = 6;
                    request.comment = approvalComment;

                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();

                    //Include function to send email to next approver so as to remind them that they have a request that needs their attention

                }
                else if(request.workflow_ID == 6)
                {
                    request.approval_status = 2;
                    request.approver = nextApprover;
                    request.workflow_ID = 7;
                    request.comment = approvalComment;

                    db.Entry(request).State = EntityState.Modified;
                    InsertApproval(request.id, user.GetMineNumber());
                    db.SaveChanges();

                    //Include function to send email to next approver so as to remind them that they have a request that needs their attention

                }
            }
        }

        public void RejectRequest(int id, string rejectComment)
        {
            var user = User.Identity;
            var request = db.Request_Details.Find(id);

            if (user.IsManagerApprover() || user.IsSupervisorApprover() || user.IsAdminUser())
            {
                if (request.workflow_ID == 5)
                { 
                    request.approval_status = 4;
                    request.comment = rejectComment;

                }
                else if(request.workflow_ID == 6)
                {
                    request.approval_status = 4;
                    request.comment = rejectComment;
                }
            }
            //else if(user.IsAdminUser())
            //{

            //}
        
        }

        //POST: Insert approval to approvers table
        private void InsertApproval(int requestDetailsId, string approver)
        {
            Approval approval = new Approval
            {
                request_details_ID = requestDetailsId,
                mine_number = approver
            };

            db.Approvals.Add(approval);
            //db.SaveChanges();
        }
    }
}