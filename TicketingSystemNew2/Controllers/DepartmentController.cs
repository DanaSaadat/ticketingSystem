
using Newtonsoft.Json;
using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketingSystemNew2.CustomFilter;

namespace TicketingSystemNew2.Controllers
{
    [AuditAttribute]
    //[MyExceptionHandler]
    public class DepartmentController : Controller
    {

        private IDepartmentService IDepartmentService;
        private IUserService IUserService;
        private IAuditService _IAuditService;

        public DepartmentController(DepartmentService IDepartmentService,UserService iUserService, AuditService iAuditService)
        {
            this.IDepartmentService = IDepartmentService;
            IUserService = iUserService;
            _IAuditService = iAuditService;
        }



        [Authorize(Roles = "Department")]
        public ActionResult Index()
        {
            try
            {
                //{
                //int value = 0;
                //value = value / 0;

                var xx = IDepartmentService.GetALL();
                //var xx = IUserService.GetALL();
                return View(xx);

            }
            catch (Exception ex)
            {
                var xx = IDepartmentService.GetALL();

                //ViewBag.listDepartment = xx;

                TempData["model"] = xx;

                throw new Exception(ex.Message);
                //throw ex;
            }

        }



        [Authorize(Roles = "Department")]
        public ActionResult Index1()
        {
            try
            {

                //int value = 0;

                //value /= value;
                ViewBag.TotalDepartment = IDepartmentService.GetALL().Count();
            return View();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public JsonResult List()
        {
            //TempData["object"] = JsonConvert.SerializeObject(IDepartmentService.GetALL());
            //TempData["action"] = "/Department/Add";
            //var xx = IStudentSevice.GetALL();
            return Json(IDepartmentService.GetALL(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "AddDepartment")]
        [HttpPost]
        public JsonResult Add1(DepartmentModel DepartmentModel)
        {

            try
            { 
            //{
            //    int val = 0;
            //    val = val / 0;
                    
                    
                if(DepartmentModel.Name =="" || DepartmentModel.Name == null)
                {
                    AuditModel AuditModel = new AuditModel()
                    {
                        // Your Audit Identifier     
                        AuditID = Guid.NewGuid(),
                        UserName = "Admin",
                        IPAddress = Request.UserHostAddress,
                        AreaAccessed = Request.Url.ToString(),
                        Time = DateTime.UtcNow,

                        Response = "Faild",


                        Bug = "username is required"
                    };
                  _IAuditService.Insert(AuditModel);   
                }
                else
                {
                    AuditModel AuditModel = new AuditModel()
                    {
                        // Your Audit Identifier     
                        AuditID = Guid.NewGuid(),
                        UserName = "Admin",
                        IPAddress = Request.UserHostAddress,
                        AreaAccessed = Request.Url.ToString(),
                        Time = DateTime.UtcNow,

                        Response = "ok",


                        Bug = null
                    };
                    _IAuditService.Insert(AuditModel);
                    DepartmentModel.ManagerID = 0;
                    var response = IDepartmentService.Insert(DepartmentModel);
                    if (response.IsDepartmentNameExist)
                    {

                        throw new Exception("Department Name Already Exists");
                        //throw new Exception("");
                        return Json(new { Error = "Department Name already  Exists", IsSuccess = false });
                    }
                }
                //int value = 0;

                //value /= value;

           //     DepartmentModel.ManagerID = 0;
           //var   response = IDepartmentService.Insert(DepartmentModel);
           // if (response.IsDepartmentNameExist)
           // {

           //         throw new Exception("Department Name Already Exists");
           //         //throw new Exception("");
           //         return Json(new { Error = "Department Name already  Exists", IsSuccess = false });
           //     }
            return Json(new { IsSuccess = true });
            }
            catch (Exception ex)
            {

                throw ex;
                //throw new Exception(ex.Message);
            }
        }










        //[AuditAttribute]
        //[SkipMyGlobalActionFilter]

        [Authorize(Roles = "AddDepartment")]
        [HttpPost]
        public JsonResult Add(DepartmentModel DepartmentModel)
        {

            try
            {
                //{
                //int val = 0;
                //val = val / 0;



                TempData["object"] = JsonConvert.SerializeObject(DepartmentModel);
                TempData["action"] = "/Department/Add";

                DepartmentModel.ManagerID = 0;
                    var response = IDepartmentService.Insert(DepartmentModel);
                    if (response.IsDepartmentNameExist)
                    {
                    TempData["Validation"] = "Department Name Already Exists";

                    TempData["object"] = JsonConvert.SerializeObject(DepartmentModel);
                    TempData["action"] = "/Department/Add";

                    //throw new Exception("Department Name Already Exists");
                    return Json(new { Error = "Department Name already  Exists", IsSuccess = false });
                    }

                //else
                //{
                //    TempData["object"] = JsonConvert.SerializeObject(DepartmentModel);
                //    //var dept = JsonConvert.DeserializeObject<DepartmentModel>(JsonConvert.SerializeObject(DepartmentModel));
                //    TempData["action"] = "/Department/Add";
                //}
                return Json(new { IsSuccess = true });

            }
           

            catch (Exception ex)
            {
                //TempData["object"] = "Department Name Already Exists";
                //TempData["action"] = "/Department/Add";
                throw ex;
                //throw new Exception(ex.Message);
            }
        }


        public JsonResult checkDepartmentName(DepartmentModel DepartmentModel)
        {
            DepartmentModel = IDepartmentService.Insert(DepartmentModel);

            return Json(DepartmentModel.IsDepartmentNameExist,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int ID)
        {

            //ViewBag.ManagerID = new SelectList(IUserService.GetALLbyDepartment(ID).ToList(), "UserID", "UserName");
            return Json(IDepartmentService.GetID(ID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getManager(int ID)
        {
            return Json(IUserService.GetALLbyDepartment(ID).ToList().Select(x => new
            {
                UserID = x.UserID,
                UserName = x.UserName
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "UpdateDepartment")]
        [HttpPost]
        public JsonResult Update(DepartmentModel DepartmentModel)
        {


            IDepartmentService.Update(DepartmentModel);
            return Json(JsonRequestBehavior.AllowGet);
        }



        public JsonResult Delete2(int ID)
        {
            try
            {
                //int vall = 0;
                //vall = vall / 0;
           
            //Student userEntity = IStudentSevice.GetID(ID);
            IDepartmentService.Delete(ID);
            return Json(JsonRequestBehavior.AllowGet);

            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "AddDepartment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentModel DepartmentModel)
        {
            try
            {

           
                if (ModelState.IsValid)
            {
                    //{
                    int value = 0;
                    value = value / 0;

                    DepartmentModel = IDepartmentService.Insert(DepartmentModel);
                    if(DepartmentModel.IsDepartmentNameExist)
                    {
                        //ViewData["msg"] = "Department Name Already Exists";
                    //ModelState.AddModelError("", "Department Name Already Exists");
                    throw new Exception("Department Name Already Exists");
                   // return View();
                    }
              
                    TempData["AlertMsg"] = "Saved Successfully";

                return RedirectToAction("Index");
            }
            
            return View(DepartmentModel);
        }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
             }


        [Authorize]
        public ActionResult Edit(int? id)
        {
            try
            {

            
            ViewBag.ManagerID = new SelectList(IUserService.GetALLbyDepartment(id.Value).ToList(), "UserID", "UserName");
           
            var GetDepartment = IDepartmentService.GetID(id);

            return View(GetDepartment);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



      






        [Authorize(Roles = "UpdateDepartment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentModel DepartmentModel)
        {
            try
            {

           
            if (ModelState.IsValid)
            {
                DepartmentModel =  IDepartmentService.Update(DepartmentModel);
                if (DepartmentModel.IsDepartmentNameExist)
                {
                    ViewData["msg"] = "Department Name already exists";
                    ViewBag.ManagerID = new SelectList(IUserService.GetALLbyDepartment(DepartmentModel.ID).ToList(), "UserID", "UserName");

                    return View();
                }
                TempData["AlertMsg"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(DepartmentModel);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            try
            {

          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DepartmentModel Model = IDepartmentService.GetIDDelete(id);

         
            if (Model.UserCount != 0)
            { 
                TempData["AlertMsg2"] = "This Department Contains Users ";
               
            }

            if (Model == null)
            {
                return HttpNotFound();
            }
            return View(Model);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }






        [Authorize(Roles = "DeleteDepartment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                //int val = 0;
                //val = val / 0;
           
            IDepartmentService.Delete(id);

            TempData["AlertMsg"] = "Deleted successfully";
            return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}